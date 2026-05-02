using System;
using NUnit.Framework;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class PoolSettingsTests
    {
        [Test]
        public void PoolSettings_Validate_WithValidValues_DoesNotThrow()
        {
            var settings = new PoolSettings
            {
                MaxSize = 10,
                DefaultCapacity = 5,
                PreWarmCount = 3,
                CollectionCheck = true
            };

            Assert.DoesNotThrow(() => settings.Validate());
        }

        [Test]
        public void PoolSettings_Validate_MaxSizeLessThanOrEqualZero_Throws()
        {
            var settings = new PoolSettings
            {
                MaxSize = 0,
                DefaultCapacity = 0,
                PreWarmCount = 0
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => settings.Validate());
        }

        [Test]
        public void PoolSettings_Validate_DefaultCapacityNegative_Throws()
        {
            var settings = new PoolSettings
            {
                MaxSize = 10,
                DefaultCapacity = -1,
                PreWarmCount = 0
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => settings.Validate());
        }

        [Test]
        public void PoolSettings_Validate_PreWarmNegative_Throws()
        {
            var settings = new PoolSettings
            {
                MaxSize = 10,
                DefaultCapacity = 0,
                PreWarmCount = -1
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => settings.Validate());
        }

        [Test]
        public void PoolSettings_Validate_DefaultCapacityGreaterThanMaxSize_Throws()
        {
            var settings = new PoolSettings
            {
                MaxSize = 5,
                DefaultCapacity = 6,
                PreWarmCount = 0
            };

            Assert.Throws<ArgumentException>(() => settings.Validate());
        }

        [Test]
        public void PoolSettings_Validate_PreWarmGreaterThanMaxSize_Throws()
        {
            var settings = new PoolSettings
            {
                MaxSize = 5,
                DefaultCapacity = 1,
                PreWarmCount = 6
            };

            Assert.Throws<ArgumentException>(() => settings.Validate());
        }

        [Test]
        public void PoolSettings_Clone_ReturnsIndependentCopy()
        {
            var original = new PoolSettings
            {
                MaxSize = 20,
                DefaultCapacity = 7,
                PreWarmCount = 4,
                CollectionCheck = false
            };

            var clone = original.Clone();

            Assert.AreEqual(original.MaxSize, clone.MaxSize);
            Assert.AreEqual(original.DefaultCapacity, clone.DefaultCapacity);
            Assert.AreEqual(original.PreWarmCount, clone.PreWarmCount);
            Assert.AreEqual(original.CollectionCheck, clone.CollectionCheck);
            Assert.AreNotSame(original, clone);
        }
    }
}