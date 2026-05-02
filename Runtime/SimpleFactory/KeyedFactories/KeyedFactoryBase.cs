using System;
using System.Collections.Generic;

namespace CreativeArcana.Factory
{
    public class KeyedFactoryBase<TKey, TProduct>
        : IKeyedFactory<TKey, TProduct>
        where TProduct : class
    {
        private readonly Dictionary<TKey, Func<TProduct>> _creators = new();

        public void RegisterCreator(TKey key, Func<TProduct> creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            _creators[key] = creator;
        }

        public void UnregisterCreator(TKey key)
        {
            _creators.Remove(key);
        }
        
        public TProduct Create(TKey key)
        {
            if (!_creators.TryGetValue(key, out var creator))
            {
                throw new KeyNotFoundException($"[AbstractFactory] No creator registered for key '{key}'");
            }

            var product = creator();
            OnAfterCreate(key, product);
            return product;
        }

        public bool CanCreate(TKey key) => _creators.ContainsKey(key);

        protected virtual void OnAfterCreate(TKey key, TProduct instance)
        {
        }
    }
}