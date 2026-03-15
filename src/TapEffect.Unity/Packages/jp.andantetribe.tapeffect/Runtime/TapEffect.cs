#nullable enable

using System;
using System.Buffers;
using System.Threading;
using AndanteTribe.Unity.Extensions.Internal;
using Cysharp.Threading.Tasks;
using ObjectReference;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AndanteTribe.Unity.Extensions
{
    /// <summary>
    /// Graphic component that displays a tap effect.
    /// </summary>
    /// <remarks>
    /// It is recommended to place this component on a higher-layer UI element.
    /// </remarks>
    public class TapEffect : Graphic
    {
        private const int MaxCountDefault = 10;
        private Vector3[] _records = Array.Empty<Vector3>();
        private int _recordsCount;

        private GraphicsBuffer _graphicsBuffer = null!;
        private int _recordsID;
        private int _countID;
        private int _durationID;
        private Rect _screen;
#if ENABLE_INPUT_SYSTEM
        private Action<UnityEngine.InputSystem.InputAction.CallbackContext> _onLeftClick = null!;
        private UnityEngine.InputSystem.UI.InputSystemUIInputModule _module = null!;
#endif

        [SerializeReference]
        private IObjectReference<Material> _material = null!;

        [SerializeField, Tooltip("Maximum number of simultaneous tap effects"), Range(0, int.MaxValue)]
        private uint _maxCount = MaxCountDefault;

        /// <summary>
        /// The maximum number of simultaneous tap effects.
        /// </summary>
        public uint MaxCount
        {
            get => _maxCount;
            set
            {
                _maxCount = value;
                _graphicsBuffer.SetCounterValue(value);
            }
        }

        [SerializeField, Tooltip("Duration of the tap effect")]
        private float _lifetime = 0.5f;

        /// <summary>
        /// The duration in seconds of each tap effect animation.
        /// </summary>
        public float Lifetime
        {
            get => _lifetime;
            set => _lifetime = value;
        }

        /// <inheritdoc />
        public override bool raycastTarget
        {
            get => false;
            set
            {
            }
        }

        protected TapEffect() => useLegacyMeshGeneration = false;

        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
#endif
            _graphicsBuffer = new(GraphicsBuffer.Target.Structured, (int)_maxCount, sizeof(float) * 3);
            _recordsID = Shader.PropertyToID("_Records");
            _countID = Shader.PropertyToID("_Count");
            _durationID = Shader.PropertyToID("_Duration");
            _screen = new Rect(0, 0, Screen.width, Screen.height);
            ArrayPool<Vector3>.Shared.Grow(ref _records, (int)_maxCount);
            LoadMaterialAsync(destroyCancellationToken).Forget();

            async UniTaskVoid LoadMaterialAsync(CancellationToken cancellationToken)
            {
                material = new Material(await _material.LoadAsync(cancellationToken));
            }
        }

        protected override void Start()
        {
            base.Start();

#if ENABLE_INPUT_SYSTEM
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
#endif
            _module = (UnityEngine.InputSystem.UI.InputSystemUIInputModule)(EventSystem.current.currentInputModule == null
                ? EventSystem.current.GetComponent<BaseInputModule>() : EventSystem.current.currentInputModule);
            _onLeftClick = _ => OnLeftClickAsync().Forget();
            _module.leftClick.action.performed += _onLeftClick;
#endif
        }

        private async UniTaskVoid OnLeftClickAsync()
        {
            if (_recordsCount >= MaxCount)
            {
                return;
            }

#if ENABLE_INPUT_SYSTEM
            var pointer = UnityEngine.InputSystem.Pointer.current;
            if (pointer == null)
            {
                return;
            }
            var screenPos = UnityEngine.InputSystem.Pointer.current.position.value;
#else
            var screenPos = (Vector2)Input.mousePosition;
#endif

            var normalizedPos = Rect.PointToNormalized(_screen, screenPos);
            var record = new Vector3(normalizedPos.x, normalizedPos.y, Time.time);
            ArrayPool<Vector3>.Shared.Grow(ref _records, _recordsCount + 1);
            _records[_recordsCount++] = record;

            await UniTask.Delay(TimeSpan.FromSeconds(_lifetime), cancellationToken: destroyCancellationToken);
            var index = _records.AsSpan(0, _recordsCount).IndexOf(record);
            _records[index] = _records[--_recordsCount];
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
#endif
            // update material
            if (material != null)
            {
                material.SetInt(_countID, _recordsCount);

                if (_recordsCount > 0)
                {
                    _graphicsBuffer.SetData(_records, 0, 0, _recordsCount);
                    material.SetFloat(_durationID, _lifetime);
                }

                // d3d12: Fragment Shader "UI/TapRippleEffect" requires a buffer (SRV) "_Records" at index 0, but none provided. Skipping draw calls to avoid crashing.
                material.SetBuffer(_recordsID, _graphicsBuffer);
            }

#if !ENABLE_INPUT_SYSTEM
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftClickAsync().Forget();
            }
#endif
        }

        /// <inheritdoc />
        protected override void OnDestroy()
        {
            base.OnDestroy();

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying && UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
#endif
            _graphicsBuffer.Dispose();
            _material.Dispose();
            ArrayPool<Vector3>.Shared.Return(_records);

            if (material != null)
            {
                Destroy(material);
            }

#if ENABLE_INPUT_SYSTEM
            _module.leftClick.action.performed -= _onLeftClick;
#endif
        }
    }
}