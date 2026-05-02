namespace CreativeArcana.Factory.Tests.Helpers
{
    public class NullFactory: FactoryBase<TestProduct>
    {
        protected override TestProduct CreateInstance() => null;
    }
}