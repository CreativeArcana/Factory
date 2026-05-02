using System;
using NUnit.Framework;
using CreativeArcana.Factory.Tests.Helpers;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class PoolFactoryAdapterTests
    {
        [Test]
        public void PoolFactoryAdapter_CountProperties_ShouldDelegateToInnerFactory()
        {
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(
                () => new AdaptablePoolableTestProduct())
            {
                CountActive = 4,
                CountInactive = 9
            };

            var adapter = new PoolFactoryAdapter<AdaptablePoolableTestProduct, ITestContract>(innerFactory);

            Assert.That(adapter.CountActive, Is.EqualTo(4));
            Assert.That(adapter.CountInactive, Is.EqualTo(9));
        }

        [Test]
        public void PoolFactoryAdapter_Get_ShouldReturnInnerInstanceAsInterface()
        {
            var expected = new AdaptablePoolableTestProduct();
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(() => expected);

            IPoolFactory<ITestContract> adapter =
                new PoolFactoryAdapter<AdaptablePoolableTestProduct, ITestContract>(innerFactory);

            var result = adapter.Get();

            Assert.That(result, Is.SameAs(expected));
            Assert.That(innerFactory.GetCallCount, Is.EqualTo(1));
        }

        [Test]
        public void PoolFactoryAdapter_Release_WithCompatibleInstance_ShouldDelegateToInnerFactory()
        {
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(
                () => new AdaptablePoolableTestProduct());

            IPoolFactory<ITestContract> adapter =
                new PoolFactoryAdapter<AdaptablePoolableTestProduct, ITestContract>(innerFactory);

            ITestContract instance = new AdaptablePoolableTestProduct();

            adapter.Release(instance);

            Assert.That(innerFactory.ReleaseCallCount, Is.EqualTo(1));
            Assert.That(innerFactory.LastReleasedInstance, Is.SameAs(instance));
        }

        [Test]
        public void PoolFactoryAdapter_Release_WithIncompatibleInstance_ShouldThrowInvalidOperationException()
        {
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(
                () => new AdaptablePoolableTestProduct());

            IPoolFactory<ITestContract> adapter =
                new PoolFactoryAdapter<AdaptablePoolableTestProduct, ITestContract>(innerFactory);

            ITestContract incompatibleInstance = new AnotherTestContractImplementation();

            var exception = Assert.Throws<InvalidOperationException>(() => adapter.Release(incompatibleInstance));

            Assert.That(exception, Is.Not.Null);
            StringAssert.Contains("Cannot release instance of type", exception.Message);
            StringAssert.Contains(nameof(AdaptablePoolableTestProduct), exception.Message);
            Assert.That(innerFactory.ReleaseCallCount, Is.EqualTo(0));
        }

        [Test]
        public void PoolFactoryAdapter_PreWarm_ShouldDelegateToInnerFactory()
        {
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(
                () => new AdaptablePoolableTestProduct());

            IPoolFactory<ITestContract> adapter =
                new PoolFactoryAdapter<AdaptablePoolableTestProduct, ITestContract>(innerFactory);

            adapter.PreWarm(12);

            Assert.That(innerFactory.PreWarmCallCount, Is.EqualTo(1));
            Assert.That(innerFactory.LastPreWarmCount, Is.EqualTo(12));
        }

        [Test]
        public void PoolFactoryAdapter_ClearPool_ShouldDelegateToInnerFactory()
        {
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(
                () => new AdaptablePoolableTestProduct());

            IPoolFactory<ITestContract> adapter =
                new PoolFactoryAdapter<AdaptablePoolableTestProduct, ITestContract>(innerFactory);

            adapter.ClearPool();

            Assert.That(innerFactory.ClearPoolCallCount, Is.EqualTo(1));
        }

        [Test]
        public void PoolFactoryAdapter_ApplyInitialPreWarm_ShouldDelegateToInnerFactory()
        {
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(
                () => new AdaptablePoolableTestProduct());

            IPoolFactory<ITestContract> adapter =
                new PoolFactoryAdapter<AdaptablePoolableTestProduct, ITestContract>(innerFactory);

            adapter.ApplyInitialPreWarm();

            Assert.That(innerFactory.ApplyInitialPreWarmCallCount, Is.EqualTo(1));
        }

        [Test]
        public void AsInterface_ShouldReturnAdapterOverSameFactory()
        {
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(
                () => new AdaptablePoolableTestProduct());

            var adaptedFactory = innerFactory.AsInterface<AdaptablePoolableTestProduct, ITestContract>();

            Assert.That(adaptedFactory, Is.Not.Null);
            Assert.That(adaptedFactory, Is.InstanceOf<IPoolFactory<ITestContract>>());
        }

        [Test]
        public void AsInterface_Get_ShouldDelegateToInnerFactory()
        {
            var expected = new AdaptablePoolableTestProduct();
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(() => expected);

            var adaptedFactory = innerFactory.AsInterface<AdaptablePoolableTestProduct, ITestContract>();

            var result = adaptedFactory.Get();

            Assert.That(result, Is.SameAs(expected));
            Assert.That(innerFactory.GetCallCount, Is.EqualTo(1));
        }

        [Test]
        public void AsInterface_Release_WithCompatibleInstance_ShouldDelegateToInnerFactory()
        {
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(
                () => new AdaptablePoolableTestProduct());

            var adaptedFactory = innerFactory.AsInterface<AdaptablePoolableTestProduct, ITestContract>();
            ITestContract instance = new AdaptablePoolableTestProduct();

            adaptedFactory.Release(instance);

            Assert.That(innerFactory.ReleaseCallCount, Is.EqualTo(1));
            Assert.That(innerFactory.LastReleasedInstance, Is.SameAs(instance));
        }

        [Test]
        public void AsInterface_Release_WithIncompatibleInstance_ShouldThrowInvalidOperationException()
        {
            var innerFactory = new FakePoolFactory<AdaptablePoolableTestProduct>(
                () => new AdaptablePoolableTestProduct());

            var adaptedFactory = innerFactory.AsInterface<AdaptablePoolableTestProduct, ITestContract>();
            ITestContract incompatibleInstance = new AnotherTestContractImplementation();

            Assert.Throws<InvalidOperationException>(() => adaptedFactory.Release(incompatibleInstance));

            Assert.That(innerFactory.ReleaseCallCount, Is.EqualTo(0));
        }
    }
}
