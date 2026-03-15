#nullable enable

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AndanteTribe.Unity.Extensions.Editor
{
    [CustomEditor(typeof(TapEffect), true)]
    public class TapEffectInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            root.Add(new PropertyField(serializedObject.FindProperty("_material")));

            var maxCountField = new PropertyField(serializedObject.FindProperty("_maxCount"));
            root.Add(maxCountField);
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                maxCountField.SetEnabled(false);
            }

            root.Add(new PropertyField(serializedObject.FindProperty("_lifetime")));
            return root;
        }
    }
}