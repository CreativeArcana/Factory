using System;

namespace CreativeArcana.Factory
{
    public sealed class GenericPoolFactory<T> : PoolFactoryBase<T> where T : class
    {
        private readonly Func<T> _createPooled;
        private readonly Action<T> _onGetFromPool;
        private readonly Action<T> _onReleaseToPool;
        private readonly Action<T> _onDestroyPooled;

        public GenericPoolFactory(
            Func<T> createPooled,
            Action<T> onGetFromPool = null,
            Action<T> onReleaseToPool = null,
            Action<T> onDestroyPooled = null,
            PoolSettings poolSettings = null)
            : base(poolSettings)
        {
            _createPooled = createPooled ?? throw new ArgumentNullException(nameof(createPooled));
            _onGetFromPool = onGetFromPool;
            _onReleaseToPool = onReleaseToPool;
            _onDestroyPooled = onDestroyPooled;
        }

        protected override T CreatePooled() => _createPooled();

        protected override void OnGetFromPool(T instance) => _onGetFromPool?.Invoke(instance);
        protected override void OnReleaseToPool(T instance) => _onReleaseToPool?.Invoke(instance);
        protected override void OnDestroyPooled(T instance) => _onDestroyPooled?.Invoke(instance);
    }
}