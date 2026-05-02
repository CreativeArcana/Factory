using System;
using System.Collections.Generic;
using CreativeArcana.Factory.Tests.Helpers;
using NUnit.Framework;
using UnityEngine;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class KeyedPoolFactoryTests
    {
        [Test]
        public void KeyedPoolFactory_RegisterCreator_ThenGet_ReturnsInstance()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("enemy", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));

            var result = factory.Get("enemy");

            Assert.NotNull(result);

            UnityEngine.Object.DestroyImmediate(go);
            UnityEngine.Object.DestroyImmediate(result.gameObject);
        }

        [Test]
        public void KeyedPoolFactory_RegisterDuplicateKey_Throws()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("enemy", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));

            Assert.Throws<InvalidOperationException>(() =>
                factory.Register("enemy", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab)));

            UnityEngine.Object.DestroyImmediate(go);
        }

        [Test]
        public void KeyedPoolFactory_Register_NullPoolCreator_Throws()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();

            Assert.Throws<ArgumentNullException>(() =>
                factory.Register("enemy", (Func<IPoolFactory<TestMonoBehaviour>>)null));
        }

        [Test]
        public void KeyedPoolFactory_Get_UnregisteredKey_Throws()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();

            Assert.Throws<KeyNotFoundException>(() => factory.Get("missing"));
        }

        [Test]
        public void KeyedPoolFactory_IsRegistered_ReturnsCorrectValue()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));

            Assert.IsTrue(factory.IsRegistered("A"));
            Assert.IsFalse(factory.IsRegistered("B"));

            UnityEngine.Object.DestroyImmediate(go);
        }

        [Test]
        public void KeyedPoolFactory_Release_ByKey_ReleasesInstance()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));

            var instance = factory.Get("A");

            Assert.DoesNotThrow(() => factory.Release("A", instance));

            UnityEngine.Object.DestroyImmediate(go);
        }

        [Test]
        public void KeyedPoolFactory_Release_ByInstance_UsesTrackedKey()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));

            var instance = factory.Get("A");

            Assert.DoesNotThrow(() => factory.Release(instance));

            UnityEngine.Object.DestroyImmediate(go);
        }

        [Test]
        public void KeyedPoolFactory_Release_Null_DoesNothing()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));

            Assert.DoesNotThrow(() => factory.Release("A", null));
            Assert.DoesNotThrow(() => factory.Release((TestMonoBehaviour)null));

            UnityEngine.Object.DestroyImmediate(go);
        }

        [Test]
        public void KeyedPoolFactory_PreWarm_CreatesInstances()
        {
            TestMonoBehaviour.ResetStatics();

            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));

            factory.PreWarm("A", 3);

            Assert.AreEqual(3, TestMonoBehaviour.TotalCreateCalls);

            UnityEngine.Object.DestroyImmediate(go);
        }

        [Test]
        public void KeyedPoolFactory_PreWarm_Zero_DoesNothing()
        {
            TestMonoBehaviour.ResetStatics();

            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));

            factory.PreWarm("A", 0);

            Assert.AreEqual(0, TestMonoBehaviour.TotalCreateCalls);

            UnityEngine.Object.DestroyImmediate(go);
        }


        [Test]
        public void KeyedPoolFactory_ApplyInitialPreWarm_UsesPoolSettings()
        {
            TestMonoBehaviour.ResetStatics();

            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(
                prefab,
                null,
                new PoolSettings
                {
                    MaxSize = 10,
                    DefaultCapacity = 0,
                    PreWarmCount = 2
                }));

            factory.ApplyInitialPreWarm("A");

            Assert.AreEqual(2, TestMonoBehaviour.TotalCreateCalls);

            UnityEngine.Object.DestroyImmediate(go);
        }


        [Test]
        public void KeyedPoolFactory_Unregister_RemovesRegistration()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));

            factory.Unregister("A");

            Assert.IsFalse(factory.IsRegistered("A"));

            UnityEngine.Object.DestroyImmediate(go);
        }

        [Test]
        public void KeyedPoolFactory_ClearAll_RemovesAllRegistrationsAndPools()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();

            var goA = new GameObject("prefabA");
            var prefabA = goA.AddComponent<TestMonoBehaviour>();

            var goB = new GameObject("prefabB");
            var prefabB = goB.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(prefabA));
            factory.Register("B", () => new ComponentPoolFactory<TestMonoBehaviour>(prefabB));

            var a = factory.Get("A");
            var b = factory.Get("B");

            factory.ClearAll();

            Assert.IsFalse(factory.IsRegistered("A"));
            Assert.IsFalse(factory.IsRegistered("B"));

            if (a != null) UnityEngine.Object.DestroyImmediate(a.gameObject);
            if (b != null) UnityEngine.Object.DestroyImmediate(b.gameObject);
            UnityEngine.Object.DestroyImmediate(goA);
            UnityEngine.Object.DestroyImmediate(goB);
        }

        [Test]
        public void KeyedPoolFactory_Dispose_ThenOperationsThrow()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            var go = new GameObject("prefab");
            var prefab = go.AddComponent<TestMonoBehaviour>();

            factory.Register("A", () => new ComponentPoolFactory<TestMonoBehaviour>(prefab));
            factory.Dispose();

            Assert.Throws<ObjectDisposedException>(() => factory.Get("A"));
            Assert.Throws<ObjectDisposedException>(() => factory.IsRegistered("A"));
            Assert.Throws<ObjectDisposedException>(() => factory.ClearAll());
            Assert.Throws<ObjectDisposedException>(() => factory.PreWarm("A", 1));

            UnityEngine.Object.DestroyImmediate(go);
        }
    }
}
