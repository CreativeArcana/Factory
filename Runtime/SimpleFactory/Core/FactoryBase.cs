using System;

namespace CreativeArcana.Factory
{
    /// <summary>
    /// Non-pooled base factory.
    /// It does not control the product lifecycle. It Just Creates.
    /// </summary>
    /// <typeparam name="T">Product</typeparam>
    public abstract class FactoryBase<T>:IFactory<T> where T : class
    {
        public T Create()
        {
            var instance = CreateInstance();
            if (instance == null)
            {
                throw new InvalidOperationException($"[FactoryBase] CreateInstance returned null for {typeof(T).Name}");
            }
            
            OnAfterCreate(instance);
            
            return instance;
        }
        
        protected abstract T CreateInstance();
        protected virtual void OnAfterCreate(T instance) { }
    }
}