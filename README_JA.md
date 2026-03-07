# VContainerPlus
[![unity-meta-check](https://github.com/AndanteTribe/VContainerPlus/actions/workflows/unity-meta-check.yml/badge.svg)](https://github.com/AndanteTribe/VContainerPlus/actions/workflows/unity-meta-check.yml)
[![Releases](https://img.shields.io/github/release/AndanteTribe/VContainerPlus.svg)](https://github.com/AndanteTribe/VContainerPlus/releases)
[![GitHub license](https://img.shields.io/github/license/AndanteTribe/VContainerPlus.svg)](./LICENSE)
[![openupm](https://img.shields.io/npm/v/jp.andantetribe.vcontainerplus?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/jp.andantetribe.vcontainerplus/)

[English](README.md) | 日本語

## 概要
**VContainerPlus** は、Unity 向け DI フレームワーク [VContainer](https://github.com/hadashiA/VContainer) を拡張するユーティリティライブラリです。

- **`LifetimeScopeBase`**: 自動コンポーネントバインディングと Editor バリデーションをサポートする `LifetimeScope` の基底クラスです。

## 要件
- Unity 2021.3 以上
- [VContainer](https://github.com/hadashiA/VContainer) 1.17.0 以上

## インストール
`Window > Package Manager` から Package Manager ウィンドウを開き、`[+] > Add package from git URL` を選択して以下の URL を入力します。

```
https://github.com/AndanteTribe/VContainerPlus.git?path=src/VContainerPlus.Unity/Packages/jp.andantetribe.vcontainerplus
```

## クイックスタート

### LifetimeScopeBase

`LifetimeScopeBase` は VContainer の `LifetimeScope` を継承した基底クラスです。インスペクター上の **Auto Bind Components** にコンポーネントを設定するだけで、登録コードを書かずにシーン上のコンポーネントを DI コンテナに登録できます。

![LifetimeScopeBase Inspector](https://github.com/user-attachments/assets/360cd14b-8da8-429a-bb0d-03d95d134439)

```csharp
using VContainer;
using VContainerPlus;

public class GameLifetimeScope : LifetimeScopeBase
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder); // 重要: 自動バインドを有効にするために必ず base を呼び出してください

        // 追加のサービスを登録
        builder.Register<GameService>(Lifetime.Singleton);
    }
}
```

インスペクターの **Auto Bind Components** に設定されたコンポーネントは、それぞれの型および実装インターフェースとして自動的に登録されます。

## API

### LifetimeScopeBase

| メンバー | 説明 |
|---------|------|
| `_autoBindComponents`（インスペクターフィールド） | `RegisterInstance` を使用して、型および実装インターフェースとして自動的に登録されるコンポーネント。 |
| `Configure(IContainerBuilder builder)` | すべての自動バインドコンポーネントを登録します。オーバーライドする際は必ず `base.Configure(builder)` を呼び出してください。 |

### DependencyValidator（エディター）

| メソッド | 説明 |
|---------|------|
| `Validate(LifetimeScopeBase lifetimeScope, bool useSourceGeneration = true)` | 指定した `LifetimeScopeBase` の依存関係注入に関する問題を検証し、`ValidateResult` を返します。 |

インスペクターの **Validation** ボタンからもバリデーションを実行できます。

## ライセンス
このライブラリは MIT ライセンスで公開しています。
