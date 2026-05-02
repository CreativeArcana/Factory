using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CreativeArcana.Factory
{
    public class GameObjectPoolFactory : PoolFactoryBase<GameObject>
    {
        protected readonly GameObject _prefab;
        protected readonly Transform _parent;

        public GameObjectPoolFactory(
            GameObject prefab,
            Transform parent = null,
            PoolSettings poolSettings = null)
            : base(poolSettings)
        {
            _prefab = prefab != null
                ? prefab
                : throw new ArgumentNullException(nameof(prefab));

            _parent = parent;
        }

        protected override GameObject CreatePooled()
        {
            var instance = Instantiate();
            instance.SetActive(false);
            return instance;
        }
        
        protected override void OnGetFromPool(GameObject instance)
        {
            if (instance != null)
                instance.SetActive(true);
        }

        protected override void OnReleaseToPool(GameObject instance)
        {
            if (instance != null)
                instance.SetActive(false);
        }

        protected override void OnDestroyPooled(GameObject instance)
        {
            if (instance != null)
                Object.Destroy(instance);
        }
        
        protected virtual GameObject Instantiate()
        {
            return Object.Instantiate(_prefab, _parent);
        }
    }
}