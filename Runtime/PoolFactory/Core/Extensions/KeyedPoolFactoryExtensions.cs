namespace CreativeArcana.Factory
{
    public static class KeyedPoolFactoryExtensions
    {
        /// <summary>
        /// Get with context for IPoolable|TContext| products.
        /// </summary>
        public static TProduct Get<TKey,TProduct,TContext>( 
            this IKeyedPoolFactory<TKey,TProduct> factory,
            TKey key,
            TContext context)
            where TProduct : class, IPoolable<TContext>
        {
            var product = factory.Get(key);
            product.OnPoolGet(context);
            return product;
        }
    }
}