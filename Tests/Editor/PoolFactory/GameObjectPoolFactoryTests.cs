using System;
using NUnit.Framework;
using UnityEngine;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class GameObjectPoolFactoryTests
    {
        [Test]
        public void GameObjectPoolFactory_Ctor_NullPrefab_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new GameObjectPoolFactory(null));
        }

        [Test]
        public void GameObjectPoolFactory_Get_ActivatesInstance()
        {
            var prefab = new GameObject("prefab");
            var factory = new GameObjectPoolFactory(prefab);

            var instance = factory.Get();

            Assert.NotNull(instance);
            Assert.IsTrue(instance.activeSelf);

            UnityEngine.Object.DestroyImmediate(prefab);
            UnityEngine.Object.DestroyImmediate(instance);
        }

        [Test]
        public void GameObjectPoolFactory_Release_DeactivatesInstance()
        {
            var prefab = new GameObject("prefab");
            var factory = new GameObjectPoolFactory(prefab);

            var instance = factory.Get();
            factory.Release(instance);

            Assert.IsFalse(instance.activeSelf);

            UnityEngine.Object.DestroyImmediate(prefab);
            UnityEngine.Object.DestroyImmediate(instance);
        }

    }
}