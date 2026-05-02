#if FACTORY_VCONTAINER
using System;
using UnityEngine;

namespace CreativeArcana.Factory.Example.VContainer.EnemySpawner
{
    [Serializable]
    public class EnemySpawnerReferences
    {
        [SerializeField] private VContainerEnemySpawner _spawner;
        [SerializeField] private EnemyKeyedFactoryCollection _config;

        public VContainerEnemySpawner Spawner => _spawner;
        public EnemyKeyedFactoryCollection  Config => _config;
    }
}
#endif