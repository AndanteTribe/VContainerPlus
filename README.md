# VContainerPlus
[![unity-meta-check](https://github.com/AndanteTribe/VContainerPlus/actions/workflows/unity-meta-check.yml/badge.svg)](https://github.com/AndanteTribe/VContainerPlus/actions/workflows/unity-meta-check.yml)
[![Releases](https://img.shields.io/github/release/AndanteTribe/VContainerPlus.svg)](https://github.com/AndanteTribe/VContainerPlus/releases)
[![GitHub license](https://img.shields.io/github/license/AndanteTribe/VContainerPlus.svg)](./LICENSE)
[![openupm](https://img.shields.io/npm/v/jp.andantetribe.vcontainerplus?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/jp.andantetribe.vcontainerplus/)

English | [日本語](README_JA.md)

## Overview
**VContainerPlus** is a utility library that extends [VContainer](https://github.com/hadashiA/VContainer) with additional features for Unity.

- **`LifetimeScopeBase`**: A base class for `LifetimeScope` that supports automatic component binding and Editor validation.

## Requirements
- Unity 2021.3 or later
- [VContainer](https://github.com/hadashiA/VContainer) 1.17.0 or later

## Installation
Open `Window > Package Manager`, select `[+] > Add package from git URL`, and enter the following URL:

```
https://github.com/AndanteTribe/VContainerPlus.git?path=src/VContainerPlus.Unity/Packages/jp.andantetribe.vcontainerplus
```

## Quick Start

### LifetimeScopeBase

`LifetimeScopeBase` is a base class that extends VContainer's `LifetimeScope`. It allows you to register scene components directly through the Inspector (**Auto Bind Components**) without writing registration code.

![LifetimeScopeBase Inspector](https://github.com/user-attachments/assets/360cd14b-8da8-429a-bb0d-03d95d134439)

```csharp
using VContainer;
using VContainerPlus;

public class GameLifetimeScope : LifetimeScopeBase
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder); // Important: always call base to enable auto binding

        // Register your additional services
        builder.Register<GameService>(Lifetime.Singleton);
    }
}
```

Components set in **Auto Bind Components** on the Inspector are automatically registered as their respective types and implemented interfaces.

## API

### LifetimeScopeBase

| Member | Description |
|--------|-------------|
| `_autoBindComponents` (Inspector field) | Components registered automatically as their type and implemented interfaces via `RegisterInstance`. |
| `Configure(IContainerBuilder builder)` | Registers all auto-bind components. Always call `base.Configure(builder)` when overriding. |

### DependencyValidator (Editor)

| Method | Description |
|--------|-------------|
| `Validate(LifetimeScopeBase lifetimeScope, bool useSourceGeneration = true)` | Validates the specified `LifetimeScopeBase` for dependency injection issues. Returns a `ValidateResult`. |

You can also trigger validation from the **Validation** button in the Inspector.

## License
This library is released under the MIT license.
