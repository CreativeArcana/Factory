using System;
using System.Collections.Generic;
using CreativeArcana.Factory.Tests.Helpers;
using NUnit.Framework;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class KeyedFactoryBaseTests
    {
        [Test]
        public void KeyedFactoryBase_RegisterAndCreate_ReturnsExpectedProduct()
        {
            var factory = new KeyedFactoryBase<string, TestProduct>();
            var product = new TestProduct();

            factory.RegisterCreator("A", () => product);

            var result = factory.Create("A");

            Assert.AreSame(product, result);
        }

        [Test]
        public void KeyedFactoryBase_RegisterCreator_NullCreator_Throws()
        {
            var factory = new KeyedFactoryBase<string, TestProduct>();

            Assert.Throws<ArgumentNullException>(() => factory.RegisterCreator("A", null));
        }

        [Test]
        public void KeyedFactoryBase_Create_UnregisteredKey_Throws()
        {
            var factory = new KeyedFactoryBase<string, TestProduct>();

            Assert.Throws<KeyNotFoundException>(() => factory.Create("Missing"));
        }

        [Test]
        public void KeyedFactoryBase_CanCreate_ReturnsCorrectState()
        {
            var factory = new KeyedFactoryBase<string, TestProduct>();
            factory.RegisterCreator("A", () => new TestProduct());

            Assert.IsTrue(factory.CanCreate("A"));
            Assert.IsFalse(factory.CanCreate("B"));
        }

        [Test]
        public void KeyedFactoryBase_UnregisterCreator_RemovesKey()
        {
            var factory = new KeyedFactoryBase<string, TestProduct>();
            factory.RegisterCreator("A", () => new TestProduct());

            factory.UnregisterCreator("A");

            Assert.IsFalse(factory.CanCreate("A"));
        }
    }
}