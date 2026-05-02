using System;
using CreativeArcana.Factory.Tests.Helpers;
using NUnit.Framework;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class GenericFactoryTests
    {
        [Test]
        public void GenericFactory_Create_ReturnsCreatedInstance()
        {
            var expected = new TestProduct();
            var factory = new GenericFactory<TestProduct>(() => expected);

            var result = factory.Create();

            Assert.AreSame(expected, result);
        }

        [Test]
        public void GenericFactory_Create_InvokesAfterCreate()
        {
            var afterCreateCalled = false;
            var product = new TestProduct();

            var factory = new GenericFactory<TestProduct>(
                () => product,
                _ => afterCreateCalled = true);

            var result = factory.Create();

            Assert.AreSame(product, result);
            Assert.IsTrue(afterCreateCalled);
        }

        [Test]
        public void GenericFactory_Ctor_NullCreateFunc_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new GenericFactory<TestProduct>(null));
        }

        [Test]
        public void FactoryBase_Create_WhenCreateInstanceReturnsNull_Throws()
        {
            var factory = new NullFactory();

            Assert.Throws<InvalidOperationException>(() => factory.Create());
        }

        [Test]
        public void FactoryBase_Create_CallsOnAfterCreate()
        {
            var factory = new ValidFactory();

            var result = factory.Create();

            Assert.NotNull(result);
            Assert.IsTrue(factory.AfterCreateCalled);
        }
    }
}