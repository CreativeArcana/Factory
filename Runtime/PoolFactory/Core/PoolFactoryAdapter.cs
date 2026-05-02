using System;

namespace CreativeArcana.Factory
{
    public sealed class PoolFactoryAdapter<TConcrete, TInterface> : IPoolFactory<TInterface>
        where TConcrete : class, TInterface
        where TInterface : class
    {
        private readonly IPoolFactory<TConcrete> _inner;

        public PoolFactoryAdapter(IPoolFactory<TConcrete> inner)
        {
            _inner = inner;
        }

        public int CountActive => _inner.CountActive;
        public int CountInactive => _inner.CountInactive;

        public TInterface Get()
        {
            return _inner.Get();
        }

        public void Release(TInterface instance)
        {
            if (instance is TConcrete concrete)
            {
                _inner.Release(concrete);
                return;
            }

            throw new InvalidOperationException(
                $"Cannot release instance of type {instance?.GetType().Name} into pool of {typeof(TConcrete).Name}");
        }

        public void PreWarm(int count) => _inner.PreWarm(count);
        public void ClearPool() => _inner.ClearPool();
        public void ApplyInitialPreWarm() => _inner.ApplyInitialPreWarm();
    }
}