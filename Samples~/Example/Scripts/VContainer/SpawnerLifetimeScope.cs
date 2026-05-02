#if FACTORY_VCONTAINER
using CreativeArcana.Factory.Example.VContainer.EnemySpawner;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CreativeArcana.Factory.Example.VContainer
{
    public class SpawnerLifetimeScope : LifetimeScope
    {
        [SerializeField] private BulletSpawnerReferences _bulletSpawnerReferences;
        [SerializeField] private RockGeneratorReferences _rockGeneratorReferences;
        [SerializeField] private EnemySpawnerReferences _enemySpawnerReferences;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<FakeService>(Lifetime.Scoped);
            
            builder.InstallBulletSpawner(_bulletSpawnerReferences);
            builder.InstallRockGenerator(_rockGeneratorReferences);
            builder.InstallEnemySpawner(_enemySpawnerReferences);
        }
    }
}
#endif