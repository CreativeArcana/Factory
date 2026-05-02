#if FACTORY_VCONTAINER

using UnityEngine;
using VContainer;

namespace CreativeArcana.Factory.VContainer
{
    public class VContainerKeyedPoolFactory<TKey, TComponent> : KeyedPoolFactory<TKey, TComponent>
        where TComponent : Component
    {
        private IObjectResolver _resolver;

        public VContainerKeyedPoolFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        
        protected override ComponentPoolFactory<TComponent> CreateComponentPoolFactory(Transform parent, KeyedFactoryEntry<TKey, TComponent> capturedEntry)
        {
            return new VContainerComponentPoolFactory<TComponent>(_resolver, capturedEntry.Component, parent,
                capturedEntry.PoolSettings);
        }
    }
}

#endif