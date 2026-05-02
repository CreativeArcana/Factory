using CreativeArcana.Factory.Tests.Helpers;
using NUnit.Framework;
using UnityEngine;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class KeyedPoolFactoryExtensionsTests
    {
        [Test]
        public void KeyedPoolFactoryExtensions_GetWithContext_CallsOnPoolGetWithContext()
        {
            var prefabGo = new GameObject("prefab");
            var prefab = prefabGo.AddComponent<TestMonoBehaviour>();
            prefabGo.SetActive(true);
            
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            factory.Register("TestKey",() => new ComponentPoolFactory<TestMonoBehaviour>(prefab));
            
            var product = factory.Get("TestKey","CTX");

            Assert.AreEqual(1, product.ContextGetCallCount);
            Assert.AreEqual("CTX", product.LastContext);
            
            UnityEngine.Object.DestroyImmediate(prefabGo);
            UnityEngine.Object.DestroyImmediate(product);
        }
    }
}