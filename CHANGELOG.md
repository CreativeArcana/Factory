# Changelog

All notable changes to this package will be documented in this file.

The format is based on Keep a Changelog.

## [1.1.0] - 2026-April-29

### Added

- Added `PoolFactoryAdapter<TConcrete, TInterface>`.
- Added `PoolFactoryExtensions.AsInterface<TConcrete, TInterface>()`.
- Concrete pool factories can now be exposed through interface-based APIs.

## [1.0.0] - 2026-April-22

### Added
- Core pooled factory contract via `IPoolFactory<T>`.
- Base pooled implementation `PoolFactoryBase<T>` built on `UnityEngine.Pool.ObjectPool<T>`.
- `ComponentPoolFactory<T>` for pooling `Component` prefabs.
- `GameObjectPoolFactory` for pooling `GameObject` prefabs.
- `GenericPoolFactory<T>` for delegate-based pooled object creation.
- `PoolSettings` for pool capacity, max size, pre-warm, and collection checks.
- `IPoolable` and `IPoolable<TContext>` lifecycle support.
- `PoolFactoryExtensions.Get<T, TContext>()` for contextual pool retrieval.
- Key-based pooled factory contract via `IKeyedPoolFactory<TKey, TProduct>`.
- Shared registration contract via `IRegisterKeyFactory<TKey, TProduct>`.
- `KeyedPoolFactory<TKey, TProduct>` with lazy per-key pool creation and reverse instance-to-key release mapping.
- `KeyedFactoryCollection<TKey, TComponent>` and `KeyedFactoryEntry<TKey, TComponent>` for ScriptableObject-based keyed pool registration.
- Optional VContainer integration:
    - `VContainerComponentPoolFactory<T>`
    - `VContainerGameObjectPoolFactory`
    - `VContainerKeyedPoolFactory<TKey, TComponent>`
- Non-pooled factory support:
    - `IFactory<T>`
    - `FactoryBase<T>`
    - `GenericFactory<T>`
    - `IKeyedFactory<TKey, TProduct>`
    - `KeyedFactoryBase<TKey, TProduct>`

### Notes
- Main package usage is centered around pooled factories.
- `IPoolFactory<T>` and `IKeyedPoolFactory<TKey, TProduct>` are the primary contracts for runtime object reuse.
- VContainer variants support both object creation and dependency injection during instantiation.
