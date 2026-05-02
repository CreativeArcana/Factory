#if FACTORY_VCONTAINER
using CreativeArcana.Factory.VContainer;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CreativeArcana.Factory.Example.VContainer
{
    public static class RockGeneratorInstaller
    {
        public static void InstallRockGenerator(this IContainerBuilder builder, RockGeneratorReferences references)
        {
            builder.Register<IPoolFactory<GameObject>>(
                (resolver) => new VContainerGameObjectPoolFactory(resolver, references.Prefab, references.Parent),
                Lifetime.Scoped);
            builder.RegisterComponent(references.Generator);
        }
    }
}
#endif