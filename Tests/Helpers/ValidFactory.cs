namespace CreativeArcana.Factory.Tests.Helpers
{
    public class ValidFactory: FactoryBase<TestProduct>
    {
        public bool AfterCreateCalled;

        protected override TestProduct CreateInstance() => new TestProduct();

        protected override void OnAfterCreate(TestProduct instance)
        {
            AfterCreateCalled = true;
        }
    }
}