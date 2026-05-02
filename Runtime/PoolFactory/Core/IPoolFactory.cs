namespace CreativeArcana.Factory
{
    /// <summary>
    /// Generic object pool interface for efficient object reuse.
    /// </summary>
    /// <typeparam name="T">Product</typeparam>
    public interface IPoolFactory<T> where T : class
    {
        int CountActive { get; }
        int CountInactive { get; }
        
        T Get();
        void Release(T instance);
        void PreWarm(int count);
        void ClearPool();
        
        /// <summary>
        /// PreWarm(PoolSettings.PreWarmCount)
        /// </summary>
        void ApplyInitialPreWarm();
    }
}