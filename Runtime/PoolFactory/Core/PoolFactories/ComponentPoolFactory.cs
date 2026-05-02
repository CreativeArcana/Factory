using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CreativeArcana.Factory
{
    public class ComponentPoolFactory<T> : PoolFactoryBase<T> where T : Component
    {
        protected readonly T _prefab;
        protected readonly Transform _parent;

        public ComponentPoolFactory(
            T prefab,
            Transform parent = null,
            PoolSettings poolSettings = null)
            : base(poolSettings)
        {
            _prefab = prefab != null
                ? prefab
                : throw new ArgumentNullException(nameof(prefab));

            _parent = parent;
        }

        protected override T CreatePooled()
        {
            var instance = Instantiate();
            instance.gameObject.SetActive(false);
            return instance;
        }
        
        protected override void OnGetFromPool(T instance)
        {
            if (instance != null)
                instance.gameObject.SetActive(true);
        }

        protected override void OnReleaseToPool(T instance)
        {
            if (instance != null)
                instance.gameObject.SetActive(false);
        }

        protected override void OnDestroyPooled(T instance)
        {
            if (instance != null)
                Object.Destroy(instance.gameObject);
        }
        
        protected virtual T Instantiate()
        {
            return Object.Instantiate(_prefab, _parent);
        }
    }
}