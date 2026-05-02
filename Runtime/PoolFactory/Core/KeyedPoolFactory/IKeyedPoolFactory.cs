namespace CreativeArcana.Factory
{
    public interface IKeyedPoolFactory<TKey, TProduct> : IRegisterKeyFactory<TKey, TProduct> where TProduct : class
    {
        TProduct Get(TKey key);
        void Release(TKey key, TProduct instance);
        void Release(TProduct instance);
        void PreWarm(TKey key, int count);
        bool IsRegistered(TKey key);
        void ClearAll();
        
        /// <summary>
        /// PreWarm(PoolSettings.PreWarmCount)
        /// </summary>
        void ApplyInitialPreWarm(TKey key);
    }
}