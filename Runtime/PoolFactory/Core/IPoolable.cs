namespace CreativeArcana.Factory
{
    /// <summary>
    /// Contract for objects that can be pooled.
    /// Factory calls these lifecycle methods automatically.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Called when instance is first created (once per lifetime).
        /// Use for one-time initialization.
        /// </summary>
        void OnPoolCreate();

        /// <summary>
        /// Called when instance is taken from pool (Get).
        /// Use to reset state for reuse.
        /// </summary>
        void OnPoolGet();

        /// <summary>
        /// Called when instance is returned to pool (Release).
        /// Use to cleanup/disable before going back to pool.
        /// </summary>
        void OnPoolRelease();

        /// <summary>
        /// Called when instance is permanently destroyed.
        /// Use for final cleanup (unsubscribe events, etc).
        /// </summary>
        void OnPoolDestroy();
    }

    /// <summary>
    /// Optional: for products that need context when spawned.
    /// </summary>
    public interface IPoolable<in TContext> : IPoolable
    {
        void OnPoolGet(TContext context);
    }
}