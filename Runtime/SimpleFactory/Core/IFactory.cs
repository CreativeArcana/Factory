namespace CreativeArcana.Factory
{
    public interface IFactory<T> where T : class
    {
        T Create();
    }
}