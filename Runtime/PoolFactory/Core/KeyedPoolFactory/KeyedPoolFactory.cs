using System;
using System.Collections.Generic;
using UnityEngine;

namespace CreativeArcana.Factory
{
    public class KeyedPoolFactory<TKey, TProduct> :
        IKeyedPoolFactory<TKey, TProduct>,
        IDisposable
        where TProduct : Component //TODO: TProduct type should change from Component to class
    {
        private readonly Dictionary<TKey, IPoolFactory<TProduct>> _activeFactories = new();
        private readonly Dictionary<TKey, Func<IPoolFactory<TProduct>>> _factoryCreators = new();

        private readonly Dictionary<TProduct, TKey> _productsKeyMap = new();

        private bool _isDisposed;

        #region IRegisterKeyFactory

        public void Register(TKey key, Func<IPoolFactory<TProduct>> poolCreator)
        {
            ThrowIfDisposed();

            if (poolCreator == null)
                throw new ArgumentNullException(nameof(poolCreator));

            if (_factoryCreators.ContainsKey(key))
            {
                throw new InvalidOperationException($"[KeyedPoolFactory] for key '{key}' is already registered.");
            }
            
            _factoryCreators[key] = poolCreator;
        }

        public virtual void Register(KeyedFactoryCollection<TKey, TProduct> configCollection, Transform parent = null)
        {
            if (configCollection == null || configCollection.Entries == null)
                return;

            foreach (var entry in configCollection.Entries)
            {
                if (entry == null || entry.Component == null)
                    continue;

                var capturedEntry = entry;
                Register(capturedEntry.Key,
                    () => CreateComponentPoolFactory(parent, capturedEntry));
            }
        }
        
        public void Register(
            TKey key,
            Func<TProduct> createFunc,
            Action<TProduct> onGet = null,
            Action<TProduct> onRelease = null,
            Action<TProduct> onDestroy = null,
            PoolSettings settings = null)
        {
            Register(key, () => new GenericPoolFactory<TProduct>(
                createFunc,
                onGet,
                onRelease,
                onDestroy,
                settings));
        }

        public void Unregister(TKey key)
        {
            ThrowIfDisposed();

            if (_activeFactories.TryGetValue(key, out var activeFactory))
            {
                activeFactory.ClearPool();
                (activeFactory as IDisposable)?.Dispose();
                _activeFactories.Remove(key);
            }

            _factoryCreators.Remove(key);
        }

        #endregion

        #region IKeyedPoolFactory

        public virtual TProduct Get(TKey key)
        {
            ThrowIfDisposed();
            var product = GetOrCreatePool(key).Get();
            _productsKeyMap[product] = key;
            return product;
        }

        public virtual void Release(TKey key, TProduct instance)
        {
            ThrowIfDisposed();

            if (instance == null)
                return;

            GetOrCreatePool(key).Release(instance);
            
            _productsKeyMap.Remove(instance);
        }

        public void Release(TProduct instance)
        {
            ThrowIfDisposed();

            if (instance == null)
                return;

            if (_productsKeyMap.TryGetValue(instance, out var key))
            {
                Release(key, instance);
            }
        }

        public virtual void PreWarm(TKey key, int count)
        {
            ThrowIfDisposed();

            if (count <= 0)
                return;

            GetOrCreatePool(key).PreWarm(count);
        }

        public virtual bool IsRegistered(TKey key)
        {
            ThrowIfDisposed();
            return _activeFactories.ContainsKey(key) || _factoryCreators.ContainsKey(key);
        }

        public virtual void ClearAll()
        {
            ThrowIfDisposed();

            foreach (var factory in _activeFactories.Values)
            {
                factory.ClearPool();
                (factory as IDisposable)?.Dispose();
            }

            _activeFactories.Clear();
            _factoryCreators.Clear();
            _productsKeyMap.Clear();
        }

        public void ApplyInitialPreWarm(TKey key)
        {
            ThrowIfDisposed();

            GetOrCreatePool(key).ApplyInitialPreWarm();
        }

        #endregion

        #region Helpers

        protected virtual ComponentPoolFactory<TProduct> CreateComponentPoolFactory(Transform parent, KeyedFactoryEntry<TKey, TProduct> capturedEntry)
        {
            return new ComponentPoolFactory<TProduct>(capturedEntry.Component, parent,
                capturedEntry.PoolSettings);
        }
        
        private IPoolFactory<TProduct> GetOrCreatePool(TKey key)
        {
            if (_activeFactories.TryGetValue(key, out var existingFactory))
                return existingFactory;

            if (!_factoryCreators.TryGetValue(key, out var creator))
                throw new KeyNotFoundException($"No pool factory registered for key '{key}'.");

            var createdFactory = creator();
            _activeFactories[key] = createdFactory;
            return createdFactory;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_isDisposed)
                return;

            ClearAll();

            _isDisposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        #endregion
    }
}