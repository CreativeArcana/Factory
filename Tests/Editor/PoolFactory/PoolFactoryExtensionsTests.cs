using CreativeArcana.Factory.Tests.Helpers;
using NUnit.Framework;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class PoolFactoryExtensionsTests
    {
        [Test]
        public void PoolFactoryExtensions_GetWithContext_CallsOnPoolGetWithContext()
        {
            var factory = new GenericPoolFactory<PoolableTestProduct>(() => new PoolableTestProduct());

            var product = factory.Get("CTX");

            Assert.AreEqual(1, product.ContextGetCallCount);
            Assert.AreEqual("CTX", product.LastContext);
        }
    }
}