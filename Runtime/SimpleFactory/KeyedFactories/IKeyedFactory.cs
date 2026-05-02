namespace CreativeArcana.Factory
{
    /// <summary>
    /// Abstract Factory contract: create a product by a key.
    /// TKey can be an enum, string, Type, ScriptableObject — anything.
    /// </summary>
    public interface IKeyedFactory<in TKey,out TProduct>
    {
        TProduct Create(TKey key);
        bool CanCreate(TKey key);
    }
}