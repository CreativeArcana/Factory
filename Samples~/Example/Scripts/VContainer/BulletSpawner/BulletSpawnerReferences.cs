#if FACTORY_VCONTAINER
using System;
using UnityEngine;

namespace CreativeArcana.Factory.Example.VContainer
{
    [Serializable]
    public class BulletSpawnerReferences
    {
        [SerializeField] private VContainerBulletSpawner _spawner;
        [SerializeField] private Bullet _prefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private PoolSettings _poolSettings;

        public VContainerBulletSpawner Spawner => _spawner;
        public Bullet Prefab => _prefab;
        public Transform Parent => _parent;
        public PoolSettings PoolSettings => _poolSettings;
    }
}
#endif