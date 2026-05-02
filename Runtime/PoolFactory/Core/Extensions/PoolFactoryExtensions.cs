namespace CreativeArcana.Factory
{
    public static class PoolFactoryExtensions
    {
        /// <summary>
        /// Get with context for IPoolable|TContext| products.
        /// </summary>
        public static T Get<T, TContext>(
            this IPoolFactory<T> factory,
            TContext context)
            where T : class, IPoolable<TContext>
        {
            var product = factory.Get();
            product.OnPoolGet(context);
            return product;
        }
        
        public static IPoolFactory<TInterface> AsInterface<TConcrete, TInterface>(
            this IPoolFactory<TConcrete> factory)
            where TConcrete : class, TInterface
            where TInterface : class
        {
            return new PoolFactoryAdapter<TConcrete, TInterface>(factory);
        }
    }
}