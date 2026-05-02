#if FACTORY_VCONTAINER

using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CreativeArcana.Factory.VContainer
{
    public class VContainerGameObjectPoolFactory : GameObjectPoolFactory
    {
        private readonly IObjectResolver _resolver;
        
        public VContainerGameObjectPoolFactory(
            IObjectResolver objectResolver,
            GameObject prefab,
            Transform parent = null,
            PoolSettings poolSettings = null)
            : base(prefab,parent,poolSettings)
        {
            _resolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));
        }

        protected override GameObject Instantiate()
        {
            return _resolver.Instantiate(_prefab, _parent);
        }
    }
}

#endif