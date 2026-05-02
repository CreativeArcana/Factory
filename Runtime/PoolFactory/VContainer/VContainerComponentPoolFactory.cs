#if FACTORY_VCONTAINER

using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CreativeArcana.Factory.VContainer
{
    public class VContainerComponentPoolFactory<T> : ComponentPoolFactory<T> where T : Component
    {
        private readonly IObjectResolver _resolver;

        public VContainerComponentPoolFactory(
            IObjectResolver objectResolver,
            T prefab,
            Transform parent = null,
            PoolSettings poolSettings = null)
            : base(prefab, parent, poolSettings)
        {
            _resolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));
        }

        protected override T Instantiate()
        {
            return _resolver.Instantiate(_prefab, _parent);
        }
    }
}

#endif