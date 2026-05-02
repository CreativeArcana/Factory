using System;
using UnityEngine;

namespace CreativeArcana.Factory
{
    public interface IRegisterKeyFactory<TKey, TProduct> where TProduct : class
    {
        void Register(TKey key, Func<IPoolFactory<TProduct>> poolCreator);
        void Register(KeyedFactoryCollection<TKey, TProduct> configCollection, Transform parent = null);
        void Register(
            TKey key,
            Func<TProduct> createFunc,
            Action<TProduct> onGet = null,
            Action<TProduct> onRelease = null,
            Action<TProduct> onDestroy = null,
            PoolSettings settings = null);
        
        void Unregister(TKey key);
    }
}