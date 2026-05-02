using System;
using UnityEngine.Pool;

namespace CreativeArcana.Factory
{
    /// <summary>
    /// Base class that wraps Unity's ObjectPool.
    /// Override the four callbacks: CreatePooled, OnGetFromPool, OnReleaseToPool, OnDestroyPooled.
    /// </summary>
    /// <typeparam name="T">Product</typeparam>
    public abstract class PoolFactoryBase<T> : IPoolFactory<T>, IDisposable where T : class
    {
        public int CountActive => _pool.CountActive;
        public int CountInactive => _pool.CountInactive;

        protected PoolSettings _settings { get; }
        
        private readonly ObjectPool<T> _pool;
        private bool _isDisposed;

        protected PoolFactoryBase(PoolSettings poolSettings = null)
        {
            _settings = (poolSettings ?? PoolSettings.Default).Clone();
            _settings.Validate();

            _pool = new ObjectPool<T>(
                createFunc: CreatePooledInternal,
                actionOnGet: OnGetInternal,
                actionOnRelease: OnReleaseInternal,
                actionOnDestroy: OnDestroyInternal,
                collectionCheck: _settings.CollectionCheck,
                defaultCapacity: _settings.DefaultCapacity,
                maxSize: _settings.MaxSize
            );
        }
        
        #region IPoolFactory
        
        public T Get()
        {
            ThrowIfDisposed();
            return _pool.Get();
        }

        public void Release(T instance)
        {
            ThrowIfDisposed();

            if (instance == null)
                return;

            _pool.Release(instance);
        }

        public void PreWarm(int count)
        {
            ThrowIfDisposed();

            if (count <= 0)
                return;

            var safeCount = Math.Min(count, _settings.MaxSize - (CountActive + CountInactive));
            if (safeCount <= 0)
                return;

            using var pooledObjects = ListPool<T>.Get(out var temp);

            for (var i = 0; i < safeCount; i++)
                temp.Add(_pool.Get());

            for (var i = 0; i < temp.Count; i++)
                _pool.Release(temp[i]);
        }

        public void ClearPool()
        {
            ThrowIfDisposed();
            _pool.Clear();
        }
        
        public void ApplyInitialPreWarm()
        {
            if (_settings.PreWarmCount > 0)
                PreWarm(_settings.PreWarmCount);
        }

        #endregion

        #region ObjectPool

        protected abstract T CreatePooled();
        protected virtual void OnGetFromPool(T instance) { }
        protected virtual void OnReleaseToPool(T instance) { }
        protected virtual void OnDestroyPooled(T instance) { }

        #endregion
        
        #region Internal ObjectPool
        
        private T CreatePooledInternal()
        {
            var instance = CreatePooled();
            if (instance == null)
                throw new InvalidOperationException($"{GetType().Name}: CreatePooled returned null.");

            if (instance is IPoolable poolable)
                poolable.OnPoolCreate();

            return instance;
        }

        private void OnGetInternal(T instance)
        {
            OnGetFromPool(instance);

            if (instance is IPoolable poolable)
                poolable.OnPoolGet();
        }

        private void OnReleaseInternal(T instance)
        {
            if (instance is IPoolable poolable)
                poolable.OnPoolRelease();

            OnReleaseToPool(instance);
        }

        private void OnDestroyInternal(T instance)
        {
            if (instance is IPoolable poolable)
                poolable.OnPoolDestroy();

            OnDestroyPooled(instance);
        }
        
        #endregion
        
        #region IDisposable
        
        public void Dispose()
        {
            if (_isDisposed)
                return;

            //TODO: fix this error when scene finished: 
            //  MissingReferenceException: The object of type 'CreativeArcana.Factory.Example.Bullet' has been destroyed but you are still trying to access it.
            //  Your script should either check if it is null or you should not destroy the object.
            _pool.Clear(); 
            _pool.Dispose();
            OnDispose();
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose() { }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }
        
        #endregion
    }
}
