using System;
using CreativeArcana.Factory.Tests.Helpers;
using NUnit.Framework;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class GenericPoolFactory
    {
        [Test]
        public void GenericPoolFactory_Get_CreatesAndInvokesLifecycle()
        {
            var product = new PoolableTestProduct();

            var factory = new GenericPoolFactory<PoolableTestProduct>(
                createPooled: () => product);

            var result = factory.Get();

            Assert.AreSame(product, result);
            Assert.AreEqual(1, product.CreateCallCount);
            Assert.AreEqual(1, product.GetCallCount);
        }

        [Test]
        public void GenericPoolFactory_Release_InvokesReleaseLifecycle()
        {
            var product = new PoolableTestProduct();

            var factory = new GenericPoolFactory<PoolableTestProduct>(
                createPooled: () => product);

            var instance = factory.Get();
            factory.Release(instance);

            Assert.AreEqual(1, product.ReleaseCallCount);
        }

        [Test]
        public void GenericPoolFactory_ClearPool_InvokesDestroyLifecycleForInactiveObjects()
        {
            var product = new PoolableTestProduct();

            var factory = new GenericPoolFactory<PoolableTestProduct>(
                createPooled: () => product);

            var instance = factory.Get();
            factory.Release(instance);
            factory.ClearPool();

            Assert.AreEqual(1, product.DestroyCallCount);
        }

        [Test]
        public void GenericPoolFactory_PreWarm_FillsInactivePool()
        {
            var createCount = 0;

            var factory = new GenericPoolFactory<TestProduct>(
                createPooled: () =>
                {
                    createCount++;
                    return new TestProduct();
                },
                poolSettings: new PoolSettings
                {
                    MaxSize = 10,
                    DefaultCapacity = 0,
                    PreWarmCount = 0
                });

            factory.PreWarm(3);

            Assert.AreEqual(3, factory.CountInactive);
            Assert.AreEqual(0, factory.CountActive);
            Assert.AreEqual(3, createCount);
        }

        [Test]
        public void GenericPoolFactory_PreWarm_WithZeroOrLess_DoesNothing()
        {
            var createCount = 0;

            var factory = new GenericPoolFactory<TestProduct>(
                createPooled: () =>
                {
                    createCount++;
                    return new TestProduct();
                });

            factory.PreWarm(0);

            Assert.AreEqual(0, createCount);
            Assert.AreEqual(0, factory.CountInactive);
        }

        [Test]
        public void GenericPoolFactory_ApplyInitialPreWarm_UsesSettingsValue()
        {
            var createCount = 0;

            var factory = new GenericPoolFactory<TestProduct>(
                createPooled: () =>
                {
                    createCount++;
                    return new TestProduct();
                },
                poolSettings: new PoolSettings
                {
                    MaxSize = 10,
                    DefaultCapacity = 0,
                    PreWarmCount = 2
                });

            factory.ApplyInitialPreWarm();

            Assert.AreEqual(2, createCount);
            Assert.AreEqual(2, factory.CountInactive);
        }

        [Test]
        public void GenericPoolFactory_Release_Null_DoesNothing()
        {
            var factory = new GenericPoolFactory<TestProduct>(() => new TestProduct());

            Assert.DoesNotThrow(() => factory.Release(null));
        }

        [Test]
        public void GenericPoolFactory_CreatePooledReturnsNull_Throws()
        {
            var factory = new GenericPoolFactory<TestProduct>(() => null);

            Assert.Throws<InvalidOperationException>(() => factory.Get());
        }

        [Test]
        public void GenericPoolFactory_Dispose_ThenAnyOperation_ThrowsObjectDisposedException()
        {
            var factory = new GenericPoolFactory<TestProduct>(() => new TestProduct());
            factory.Dispose();

            Assert.Throws<ObjectDisposedException>(() => factory.Get());
            Assert.Throws<ObjectDisposedException>(() => factory.Release(new TestProduct()));
            Assert.Throws<ObjectDisposedException>(() => factory.PreWarm(1));
            Assert.Throws<ObjectDisposedException>(() => factory.ClearPool());
        }

    }
}