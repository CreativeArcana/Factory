# Creative Arcana Factory Documentation

## Overview

Creative Arcana Factory is a Unity package for creating and reusing objects through factory patterns, with the main emphasis on **pool-based factories**.

Although the package also contains non-pooled factory utilities, the primary runtime architecture is built around:

- `IPoolFactory<T>`
- `IKeyedPoolFactory<TKey, TProduct>`

These two interfaces represent the main entry points for spawning, reusing, releasing, and organizing pooled objects.

---

## Core Concepts

## 1. `IPoolFactory<T>`

`IPoolFactory<T>` is the base contract for a single object pool.
```csharp
public interface IPoolFactory<T> where T : class
{
    int CountActive { get; }
    int CountInactive { get; }
    
    T Get();
    void Release(T instance);
    void PreWarm(int count);
    void ClearPool();
    void ApplyInitialPreWarm();
}
```
### Responsibilities
- Get an instance from the pool
- Return an instance to the pool
- Pre-warm the pool
- Clear all inactive pooled instances
- Apply initial pre-warm from `PoolSettings`

### Main implementations
- `ComponentPoolFactory<T>`
- `GameObjectPoolFactory`
- `GenericPoolFactory<T>`

---

## 2. `IKeyedPoolFactory<TKey, TProduct>`

`IKeyedPoolFactory<TKey, TProduct>` is used when one pool is not enough and you need a separate pool per key.

```csharp
public interface IKeyedPoolFactory<TKey, TProduct> : IRegisterKeyFactory<TKey, TProduct>
where TProduct : class
{
    TProduct Get(TKey key);
    void Release(TKey key, TProduct instance);
    void Release(TProduct instance);
    void PreWarm(TKey key, int count);
    bool IsRegistered(TKey key);
    void ClearAll();
    void ApplyInitialPreWarm(TKey key);
}
```
This interface is especially useful for systems like:
- bullets by faction
- enemies by type
- VFX by identifier
- UI elements by screen/state
- gameplay prefabs by enum or string key

---

## Pool Factory Implementations

## `PoolFactoryBase<T>`

`PoolFactoryBase<T>` is the abstract foundation for all pooled factories in this package.

It wraps Unity's `ObjectPool<T>` and centralizes:
- creation
- get/release callbacks
- destruction
- `IPoolable` lifecycle support
- pool settings validation
- disposal

### Overridable methods
```csharp
protected abstract T CreatePooled();
protected virtual void OnGetFromPool(T instance) { }
protected virtual void OnReleaseToPool(T instance) { }
protected virtual void OnDestroyPooled(T instance) { }
```
### Internal behavior
When an instance goes through the pool, the base class automatically invokes:

- `IPoolable.OnPoolCreate()` once when created
- `IPoolable.OnPoolGet()` when retrieved
- `IPoolable.OnPoolRelease()` when released
- `IPoolable.OnPoolDestroy()` when destroyed

This means your gameplay objects can own their own reuse lifecycle cleanly.

---

## `ComponentPoolFactory<T>`

Pools Unity `Component` prefabs.

### Behavior
- Instantiates from prefab
- Parents under an optional transform
- Disables the GameObject when created
- Activates on `Get()`
- Deactivates on `Release()`
- Destroys the GameObject on pool destruction

### Example
```csharp
var enemyFactory = new ComponentPoolFactory<Enemy>(
    enemyPrefab,
    enemiesRoot,
    new PoolSettings
    {
        DefaultCapacity = 20,
        MaxSize = 100,
        PreWarmCount = 10
    });

enemyFactory.ApplyInitialPreWarm();

var enemy = enemyFactory.Get();
enemyFactory.Release(enemy);
```
---

## `GameObjectPoolFactory`

Pools raw `GameObject` prefabs.

### Behavior
- Instantiates a GameObject prefab
- Activates on `Get()`
- Deactivates on `Release()`
- Destroys the GameObject when removed permanently

### Example
```csharp
var effectFactory = new GameObjectPoolFactory(effectPrefab, effectsRoot);
var fx = effectFactory.Get();
effectFactory.Release(fx);
```
---

## `GenericPoolFactory<T>`

Pools any reference type using delegates.

Useful when:
- the product is not a Unity `Component`
- you want custom get/release behavior
- you want pooling without prefab-based creation

### Example
```csharp
var dataFactory = new GenericPoolFactory<MyReusableData>(
    createPooled: () => new MyReusableData(),
    onGetFromPool: x => x.ResetForUse(),
    onReleaseToPool: x => x.Cleanup(),
    onDestroyPooled: x => x.Dispose(),
    poolSettings: new PoolSettings { MaxSize = 128 }
);
```
---

## Keyed Pool Factories

## `KeyedPoolFactory<TKey, TProduct>`

This is one of the most important types in the package.

It manages **multiple internal pools**, where each key corresponds to its own `IPoolFactory<TProduct>`.

### Why use it?
Instead of keeping many separate factories manually, you can centralize them behind one keyed API.

For example:
- `"PlayerBullet"` → bullet pool A
- `"EnemyBullet"` → bullet pool B
- `"Rocket"` → projectile pool C

### Internal design
`KeyedPoolFactory<TKey, TProduct>` keeps:
- a dictionary of registered pool creators
- a dictionary of active created pools
- a reverse map from product instance to key

This gives you:
- lazy pool creation
- key-based retrieval
- automatic release-by-instance

### Registration options

#### 1. Register with a pool creator
```csharp
factory.Register(key, () => new ComponentPoolFactory<MyComponent>(prefab));
```
#### 2. Register with create/release delegates
```csharp
factory.Register(
    key,
    createFunc: () => new MyObject(),
    onGet: x => x.Reset(),
    onRelease: x => x.Cleanup(),
    onDestroy: x => x.Dispose(),
    settings: new PoolSettings { MaxSize = 50 }
);
```
#### 3. Register from `KeyedFactoryCollection`
```csharp
factory.Register(configCollection, parent);
```
This is useful when keys, prefabs, and pool settings are authored as data in a ScriptableObject.

---

## How `KeyedPoolFactory` Works

### Step 1: Register keys
Each key must be registered with a pool creator or config entry.

### Step 2: Lazy creation
The pool for a key is not built immediately. It is created the first time the key is used.

This keeps startup lighter and creates pools only when actually needed.

### Step 3: Get by key
```csharp
var bullet = factory.Get(BulletType.Player);
```
The keyed factory:
- locates the key
- creates the pool if needed
- gets an instance from that internal pool
- stores a reverse mapping from instance to key

### Step 4: Release by key or by instance

#### Release with explicit key
```csharp
factory.Release(BulletType.Player, bullet);
```
#### Release with instance only
```csharp
factory.Release(bullet);
```
When releasing by instance only, the factory uses its internal instance-to-key map to find the correct pool automatically.

This is a very practical feature in gameplay code because the caller often only has a spawned instance, not the original key.

### Step 5: Pre-warm per key
```csharp
factory.PreWarm(BulletType.Player, 20);
factory.ApplyInitialPreWarm(BulletType.Player);
```
Each key has its own pool capacity and pre-warm flow.

---

## `KeyedFactoryCollection<TKey, TComponent>`

A `ScriptableObject`-based configuration collection for keyed component pools.

It contains a list of `KeyedFactoryEntry<TKey, TComponent>`.

Each entry includes:
- `Key`
- `Component`
- `PoolSettings`

This allows designers or programmers to define keyed pools through data rather than code.

### Entry type
```csharp
[Serializable]
public class KeyedFactoryEntry<TKey, TComponent>
{
    public TKey Key;
    public TComponent Component;
    public PoolSettings PoolSettings = new();
}
```
### Typical use case
Create a config asset for:
- enemy prefabs by enum
- VFX prefabs by string key
- pickups by type
- UI elements by identifier

Then register them all at once:
```csharp
factory.Register(configCollection, parent);
```
---

## Pool Lifecycle with `IPoolable`

Any pooled product can implement `IPoolable` to receive lifecycle callbacks automatically.

```csharp
public interface IPoolable
{
    void OnPoolCreate();
    void OnPoolGet();
    void OnPoolRelease();
    void OnPoolDestroy();
}
```
### Recommended usage
- `OnPoolCreate()` → one-time initialization
- `OnPoolGet()` → reset state for reuse
- `OnPoolRelease()` → stop effects, disable subscriptions, clear runtime state
- `OnPoolDestroy()` → final cleanup

### Example
```csharp
public class Enemy : MonoBehaviour, IPoolable
{
    public void OnPoolCreate()
    {
        // One-time setup
    }

    public void OnPoolGet()
    {
        // Reset health, state, AI flags
    }
    
    public void OnPoolRelease()
    {
        // Stop movement, particles, coroutines, etc.
    }
    
    public void OnPoolDestroy()
    {
        // Final cleanup
    }
}
```
---

## Contextual Spawn with `IPoolable<TContext>`

For pooled objects that need spawn-time context, implement:

```csharp
public interface IPoolable<in TContext> : IPoolable
{
    void OnPoolGet(TContext context);
}
```
Then use the extension:

```csharp
factory.Get(context);
```
### Example
```csharp
public struct ProjectileSpawnContext
{
    public Vector3 Position;
    public Vector3 Direction;
    public float Speed;
}
    
public class Projectile : MonoBehaviour, IPoolable<ProjectileSpawnContext>
{
    public void OnPoolCreate() { }
    public void OnPoolGet() { }
    public void OnPoolRelease() { }
    public void OnPoolDestroy() { }
    
    public void OnPoolGet(ProjectileSpawnContext context)
    {
        transform.position = context.Position;
        transform.forward = context.Direction;
    }
}
```
Usage:
```csharp
var projectile = projectileFactory.Get(new ProjectileSpawnContext
{
    Position = spawnPoint.position,
    Direction = spawnPoint.forward,
    Speed = 25f
});
```
---

## Pool Settings

`PoolSettings` defines the runtime behavior of each pool.

```csharp
public sealed class PoolSettings
{
    public int MaxSize = 1000;
    public int DefaultCapacity = 10;
    public int PreWarmCount = 0;
    public bool CollectionCheck = true;
}
```
### Fields
- `MaxSize`: hard cap for pooled instances
- `DefaultCapacity`: initial capacity used when the Unity pool is created
- `PreWarmCount`: number of objects to pre-create
- `CollectionCheck`: safer release validation, slower but useful in debug

### Validation rules
- `MaxSize > 0`
- `DefaultCapacity >= 0`
- `PreWarmCount >= 0`
- `DefaultCapacity <= MaxSize`
- `PreWarmCount <= MaxSize`

---

## VContainer Integration

If `FACTORY_VCONTAINER` is defined, the package adds VContainer-powered pooled factories.

This is especially useful when pooled prefabs depend on constructor or injection-based setup.

### Important point
These factories do more than instantiate objects — they also resolve and inject dependencies through VContainer's `IObjectResolver`.

### Available types

#### `VContainerComponentPoolFactory<T>`
A `ComponentPoolFactory<T>` variant that uses:

```csharp
_resolver.Instantiate(_prefab, _parent);
```
instead of regular `Object.Instantiate`.

#### `VContainerGameObjectPoolFactory`
A `GameObjectPoolFactory` variant with dependency injection-aware instantiation.

#### `VContainerKeyedPoolFactory<TKey, TComponent>`
A keyed version that registers VContainer-based component pools from a keyed config collection.

### Example
```csharp
var factory = new VContainerComponentPoolFactory<Enemy>(
    resolver,
    enemyPrefab,
    enemiesRoot,
    new PoolSettings { PreWarmCount = 10 }
);

factory.ApplyInitialPreWarm();

var enemy = factory.Get();
factory.Release(enemy);
```
### Keyed VContainer Example
```csharp
var keyedFactory = new VContainerKeyedPoolFactory<string, Enemy>(resolver);
keyedFactory.Register(enemyConfigCollection, enemiesRoot);

var enemy = keyedFactory.Get("Melee");
keyedFactory.Release(enemy);
```
---

## Non-Pooled Factories

This package also includes non-pooled factories, but they are secondary compared to the pooling API.

Included types:
- `IFactory<T>`
- `FactoryBase<T>`
- `GenericFactory<T>`
- `IKeyedFactory<TKey, TProduct>`
- `KeyedFactoryBase<TKey, TProduct>`

These are useful when:
- reuse is unnecessary
- objects are lightweight
- lifecycle ownership should remain external
- you simply need creation logic abstraction

### Example
```csharp
var factory = new GenericFactory<MyService>(() => new MyService());
var service = factory.Create();
```
---

## Recommended Usage Patterns

## Use `ComponentPoolFactory<T>` when
- you have a prefab with a specific component
- spawned objects are reused frequently
- activation/deactivation should be automatic

## Use `GameObjectPoolFactory` when
- you only care about whole GameObject prefabs
- no typed component API is needed

## Use `GenericPoolFactory<T>` when
- your product is a plain C# class
- you want pooling without prefab dependency

## Use `KeyedPoolFactory<TKey, TProduct>` when
- you have multiple variants of the same product family
- each variant needs its own pool
- you want one centralized pool entry point
- you want to release instances without manually carrying their keys everywhere

## Use VContainer variants when
- pooled objects need dependency injection
- you already use VContainer in your project
- instantiation should go through `IObjectResolver`

---
## PoolFactoryAdapter
`PoolFactoryAdapter<TConcrete, TInterface>` allows an existing concrete pool factory to be used through an interface-based pool factory contract.

This is useful when object creation must stay concrete, but consuming systems should depend on abstractions.

### Definition
```csharp
public sealed class PoolFactoryAdapter<TConcrete, TInterface> : IPoolFactory<TInterface>
where TConcrete : class, TInterface
where TInterface : class
```
### Purpose

`IPoolFactory<T>` is generic and type-specific. A factory that produces `EnemyProjectile` cannot automatically be passed to a system that expects `IPoolFactory<IProjectile>`.

`PoolFactoryAdapter<TConcrete, TInterface>` solves this by wrapping an `IPoolFactory<TConcrete>` and exposing it as `IPoolFactory<TInterface>`.

### Example

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

var concreteFactory = new GenericPoolFactory<EnemyProjectile>(
    createPooled: () => new EnemyProjectile()
);

IPoolFactory<IProjectile> interfaceFactory =
concreteFactory.AsInterface<EnemyProjectile, IProjectile>();

IProjectile projectile = interfaceFactory.Get();

projectile.Launch();

interfaceFactory.Release(projectile);
```
### Extension Method

The recommended way to create the adapter is through:

```csharp
public static IPoolFactory<TInterface> AsInterface<TConcrete, TInterface>(
this IPoolFactory<TConcrete> factory)
where TConcrete : class, TInterface
where TInterface : class
````
Example:

```csharp
IPoolFactory<IProjectile> projectileFactory =
enemyProjectileFactory.AsInterface<EnemyProjectile, IProjectile>();
```
### Delegated Members

The adapter delegates these members directly to the wrapped concrete pool factory:

```csharp
int CountActive { get; }
int CountInactive { get; }

TInterface Get();
void Release(TInterface instance);
void PreWarm(int count);
void ClearPool();
void ApplyInitialPreWarm();
```
### Release Validation

When releasing an instance through the adapter, the instance must be compatible with `TConcrete`.

Valid:

```csharp
IProjectile projectile = interfaceFactory.Get();

interfaceFactory.Release(projectile);
```
Invalid:

```csharp
public sealed class PlayerProjectile : IProjectile
{
    public void Launch() { }
}

IProjectile projectile = new PlayerProjectile();

interfaceFactory.Release(projectile); // Throws InvalidOperationException
```
The adapter prevents returning an object to a pool that did not create or does not support that concrete type.

### Recommended Use Cases

Use `AsInterface` when:

- Systems should depend on interfaces instead of concrete implementations.
- Dependency injection registrations expose abstractions.
- A concrete pooled object implements a gameplay/service interface.
- You want to hide the concrete pooled type from consumers.

Example with a consumer:

```csharp
public sealed class Weapon
{
    private readonly IPoolFactory<IProjectile> _projectilePool;
    
    public Weapon(IPoolFactory<IProjectile> projectilePool)
    {
        _projectilePool = projectilePool;
    }
    
    public void Fire()
    {
        var projectile = _projectilePool.Get();
        projectile.Launch();
    }
    
    public void ReleaseProjectile(IProjectile projectile)
    {
        _projectilePool.Release(projectile);
    }
}
```
Factory setup:

```csharp
IPoolFactory<EnemyProjectile> concretePool =
new GenericPoolFactory<EnemyProjectile>(() => new EnemyProjectile());

IPoolFactory<IProjectile> interfacePool =
concretePool.AsInterface<EnemyProjectile, IProjectile>();

var weapon = new Weapon(interfacePool);
```
---
## Summary

The package is designed mainly around pooled object creation and reuse.

The most important contracts are:

- `IPoolFactory<T>`
- `IKeyedPoolFactory<TKey, TProduct>`

For most Unity gameplay scenarios, these two interfaces should be your primary abstraction layer.

If your project uses DI, VContainer-based factories extend the same pooling workflow while also injecting dependencies during instantiation.

Non-pooled factories are included for completeness, but the real strength of this package is in its pool-driven workflow, especially:
- `ComponentPoolFactory<T>`
- `GameObjectPoolFactory`
- `GenericPoolFactory<T>`
- `KeyedPoolFactory<TKey, TProduct>`
- `VContainerKeyedPoolFactory<TKey, TComponent>`