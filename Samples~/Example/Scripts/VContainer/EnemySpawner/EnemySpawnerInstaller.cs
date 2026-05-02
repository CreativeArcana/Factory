#if FACTORY_VCONTAINER
using CreativeArcana.Factory.VContainer;
using VContainer;
using VContainer.Unity;

namespace CreativeArcana.Factory.Example.VContainer.EnemySpawner
{
    public static class EnemySpawnerInstaller
    {
        public static void InstallEnemySpawner(this IContainerBuilder builder, EnemySpawnerReferences references)
        {
            builder.Register<IKeyedPoolFactory<EnemyType,Enemy>>(
                (resolver) => new VContainerKeyedPoolFactory<EnemyType, Enemy>(resolver),
                Lifetime.Scoped);
            
            builder.RegisterInstance(references.Config).As<KeyedFactoryCollection<EnemyType, Enemy>>();
            builder.RegisterComponent(references.Spawner);
        }
    }
}
#endif