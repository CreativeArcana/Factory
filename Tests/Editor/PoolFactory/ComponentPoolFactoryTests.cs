using System;
using CreativeArcana.Factory.Tests.Helpers;
using NUnit.Framework;
using UnityEngine;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class ComponentPoolFactoryTests
    {
        
        [Test]
        public void ComponentPoolFactory_Ctor_NullPrefab_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ComponentPoolFactory<TestMonoBehaviour>(null));
        }

        [Test]
        public void ComponentPoolFactory_Get_CreatesInactiveThenActivatesInstance()
        {
            var prefabGo = new GameObject("prefab");
            var prefab = prefabGo.AddComponent<TestMonoBehaviour>();
            prefabGo.SetActive(true);
            var factory = new ComponentPoolFactory<TestMonoBehaviour>(prefab);
            var instance = factory.Get();
            Assert.NotNull(instance);
            Assert.IsTrue(instance.gameObject.activeSelf);
            Assert.AreEqual(1, instance.CreateCallCount);
            Assert.AreEqual(1, instance.GetCallCount);

            UnityEngine.Object.DestroyImmediate(prefabGo);
            UnityEngine.Object.DestroyImmediate(instance.gameObject);
        }

        [Test]
        public void ComponentPoolFactory_Release_DeactivatesInstance()
        {
            var prefabGo = new GameObject("prefab");
            var prefab = prefabGo.AddComponent<TestMonoBehaviour>();

            var factory = new ComponentPoolFactory<TestMonoBehaviour>(prefab);

            var instance = factory.Get();
            factory.Release(instance);

            Assert.IsFalse(instance.gameObject.activeSelf);
            Assert.AreEqual(1, instance.ReleaseCallCount);

            UnityEngine.Object.DestroyImmediate(prefabGo);
            UnityEngine.Object.DestroyImmediate(instance.gameObject);
        }
    }
}