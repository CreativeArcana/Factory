# Creative Arcana Factory

A lightweight Unity factory package with a strong focus on **object pooling**.

This package is primarily built around two main interfaces:

- `IPoolFactory<T>`
- `IKeyedPoolFactory<TKey, TProduct>`

It provides reusable pool factories for `Component`, `GameObject`, and generic class types, plus keyed pool factories for managing multiple pools behind a single API.

## Main Features

- Built on top of Unity's `ObjectPool<T>`
- Reusable pool factories for common Unity workflows
- Key-based pooled spawning with lazy pool creation
- `IPoolable` lifecycle support
- Context-based pool retrieval through `IPoolable<TContext>`
- ScriptableObject-based keyed registrations
- Optional **VContainer** integration for instantiation with dependency injection

## Main Pool Factories

### `IPoolFactory<T>`
The core pooled factory contract.
```csharp
T Get();
void Release(T instance);
void PreWarm(int count);
void ClearPool();
void ApplyInitialPreWarm();
```
Implemented by:
- `ComponentPoolFactory<T>`
- `GameObjectPoolFactory`
- `GenericPoolFactory<T>`

### `IKeyedPoolFactory<TKey, TProduct>`
A key-based pooled factory that manages multiple pools.

```csharp
TProduct Get(TKey key);
void Release(TKey key, TProduct instance);
void Release(TProduct instance);
void PreWarm(TKey key, int count);
bool IsRegistered(TKey key);
void ClearAll();
void ApplyInitialPreWarm(TKey key);
```
Implemented by:
- `KeyedPoolFactory<TKey, TProduct>`
- `VContainerKeyedPoolFactory<TKey, TComponent>`

## Keyed Pool Factories

`KeyedPoolFactory<TKey, TProduct>` allows you to register a pool per key and retrieve instances through a single factory.

Typical keys can be:
- enum
- string
- type
- ScriptableObject reference

Each key maps to its own internal `IPoolFactory<TProduct>`.

### How it works
- You register a pool creator for a key.
- The internal pool is created lazily on first use.
- Calling `Get(key)` retrieves an instance from that key's pool.
- Calling `Release(instance)` can automatically resolve the correct key using internal instance tracking.
- Calling `Release(key, instance)` releases directly to a specific keyed pool.

## VContainer Support

If `FACTORY_VCONTAINER` is enabled, the package provides VContainer-based pool factories.

These factories do not just create objects — they also instantiate them through `IObjectResolver`, which means **dependencies are injected automatically**.

Available types:
- `VContainerComponentPoolFactory<T>`
- `VContainerGameObjectPoolFactory`
- `VContainerKeyedPoolFactory<TKey, TComponent>`

## Quick Example

```csharp
var enemyFactory = new ComponentPoolFactory<Enemy>(enemyPrefab, parent, new PoolSettings
{
    DefaultCapacity = 10,
    MaxSize = 50,
    PreWarmCount = 10
});

enemyFactory.ApplyInitialPreWarm();

var enemy = enemyFactory.Get();
enemyFactory.Release(enemy);
```

## Keyed Example

```csharp
var factory = new KeyedPoolFactory<string, Bullet>();

factory.Register("Player", () => new ComponentPoolFactory<Bullet>(playerBulletPrefab));
factory.Register("Enemy", () => new ComponentPoolFactory<Bullet>(enemyBulletPrefab));

var playerBullet = factory.Get("Player");
factory.Release(playerBullet);
```
## Poolable Lifecycle

If your pooled object implements `IPoolable`, lifecycle callbacks are triggered automatically:

- `OnPoolCreate()`
- `OnPoolGet()`
- `OnPoolRelease()`
- `OnPoolDestroy()`

If your product needs spawn context(ex. Enemy Config), implement `IPoolable<TContext>` and use:

```csharp
factory.Get(context);
```

## Interface Pool Adapter

Sometimes a pool is created for a concrete type, but the consuming code should only depend on an interface.

For example, a pool may internally create `EnemyProjectile`, while gameplay systems only need `IProjectile`.

Use `AsInterface<TConcrete, TInterface>()` to expose an `IPoolFactory<TConcrete>` as an `IPoolFactory<TInterface>`.
```csharp
public interface IProjectile
{
    void Launch();
}

public sealed class EnemyProjectile : IProjectile
{
    public void Launch()
    {
        // Launch logic
    }
}

IPoolFactory<EnemyProjectile> concretePool =
new GenericPoolFactory<EnemyProjectile>(() => new EnemyProjectile());

IPoolFactory<IProjectile> projectilePool =
concretePool.AsInterface<EnemyProjectile, IProjectile>();

IProjectile projectile = projectilePool.Get();

projectile.Launch();

projectilePool.Release(projectile);
```
`PoolFactoryAdapter<TConcrete, TInterface>` delegates all pool operations to the original concrete pool:

- `Get`
- `Release`
- `PreWarm`
- `ClearPool`
- `ApplyInitialPreWarm`
- `CountActive`
- `CountInactive`

The adapter validates released instances. Only instances compatible with `TConcrete` can be released back into the adapted pool.

```csharp
IPoolFactory<IProjectile> pool =
concretePool.AsInterface<EnemyProjectile, IProjectile>();

IProjectile projectile = pool.Get();

pool.Release(projectile); // Valid if projectile is EnemyProjectile
```

## Notes

- Non-pooled factories are included, but pooling is the primary use case of this package.
- `PoolSettings` controls capacity, max size, pre-warm count, and collection safety checks.
- `KeyedFactoryCollection<TKey, TComponent>` can be used to register keyed component pools from data assets.

See `Documentation.md` for more details.