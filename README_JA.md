# TapEffect
[![unity-meta-check](https://github.com/AndanteTribe/TapEffect/actions/workflows/unity-meta-file-check.yml/badge.svg)](https://github.com/AndanteTribe/TapEffect/actions/workflows/unity-meta-file-check.yml)
[![Releases](https://img.shields.io/github/release/AndanteTribe/TapEffect.svg)](https://github.com/AndanteTribe/TapEffect/releases)
[![GitHub license](https://img.shields.io/github/license/AndanteTribe/TapEffect.svg)](./LICENSE)
[![openupm](https://img.shields.io/npm/v/jp.andantetribe.tapeffect?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/jp.andantetribe.tapeffect/)

[English](README.md) | 日本語

## 概要
**TapEffect** は，Unity UI (uGUI) 上にリップル（波紋）タップエフェクトを表示する Unity パッケージです．

ユーザーが画面をタップまたはクリックすると，カスタムシェーダーを使ってリップルアニメーションが描画されます．複数のエフェクトを同時に表示でき，最大発生数と持続時間を設定できます．

## 要件
- Unity 2021.3 以降
- [UniTask](https://github.com/Cysharp/UniTask) 2.5.10 以降
- [ObjectReference](https://github.com/AndanteTribe/ObjectReference) 1.0.0 以降

## インストール
`Window > Package Manager` を開き，`[+] > Add package from git URL` を選択して，以下の URL を入力してください：

```
https://github.com/AndanteTribe/TapEffect.git?path=src/TapEffect.Unity/Packages/jp.andantetribe.tapeffect
```

## クイックスタート

1. Screen Space - Overlay の Canvas の子 UI GameObject（例：フルスクリーンのパネル）に `TapEffect` コンポーネントをアタッチする．
2. インスペクターの `_material`（`IObjectReference<Material>`）フィールドにマテリアルを割り当てる．
3. `Max Count` と `Lifetime` を必要に応じて調整する．

> **注意：** `TapEffect` コンポーネントは，他の UI 要素の上にリップルが描画されるよう，レイヤーの高い UI 要素に配置することを推奨します．

## API

| プロパティ | 説明 |
|-----------|------|
| `MaxCount` | タップエフェクトの最大同時発生数．実行時に変更すると内部のグラフィックスバッファカウンターがリセットされます． |
| `Lifetime` | 各タップリップルアニメーションの持続時間（秒）． |
| `raycastTarget` | 常に `false`．このコンポーネントはレイキャストをブロックしません． |

## ライセンス
このライブラリは MIT ライセンスのもとで公開されています．
