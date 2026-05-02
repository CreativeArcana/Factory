#if FACTORY_VCONTAINER
using CreativeArcana.Factory.VContainer;
using VContainer;
using VContainer.Unity;

namespace CreativeArcana.Factory.Example.VContainer
{
    public static class BulletSpawnerInstaller
    {
        public static void InstallBulletSpawner(this IContainerBuilder builder, BulletSpawnerReferences references)
        {
            builder.Register<IPoolFactory<IBullet>>(
                (resolver) =>
                    new VContainerComponentPoolFactory<Bullet>(
                    resolver,
                    references.Prefab,
                    references.Parent,
                    references.PoolSettings)
                        .AsInterface<Bullet,IBullet>(),
                Lifetime.Scoped);
            builder.RegisterComponent(references.Spawner);
        }
    }
}
#endif