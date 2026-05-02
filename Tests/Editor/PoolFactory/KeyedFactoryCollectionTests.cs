using System.Collections.Generic;
using CreativeArcana.Factory.Tests.Helpers;
using NUnit.Framework;
using UnityEngine;

namespace CreativeArcana.Factory.Tests.Editor
{
    public class KeyedFactoryCollectionTests
    {
        [Test]
        public void KeyedFactoryCollection_Entries_ReturnsAssignedEntries()
        {
            var collection = ScriptableObject.CreateInstance<TestKeyedFactoryCollection>();

            var prefabGo = new GameObject("prefab");
            var prefab = prefabGo.AddComponent<TestMonoBehaviour>();

            var entries = new List<KeyedFactoryEntry<string, TestMonoBehaviour>>
            {
                new KeyedFactoryEntry<string, TestMonoBehaviour>
                {
                    Key = "Player",
                    Component = prefab,
                    PoolSettings = new PoolSettings()
                }
            };

            collection.SetEntries(entries);

            Assert.AreEqual(1, collection.Entries.Count);
            Assert.AreEqual("Player", collection.Entries[0].Key);
            Assert.AreSame(prefab, collection.Entries[0].Component);

            UnityEngine.Object.DestroyImmediate(prefabGo);
            UnityEngine.Object.DestroyImmediate(collection);
        }

        [Test]
        public void KeyedPoolFactory_Register_FromCollection_RegistersValidEntries()
        {
            var collection = ScriptableObject.CreateInstance<TestKeyedFactoryCollection>();
            var prefabGo = new GameObject("prefab");
            var prefab = prefabGo.AddComponent<TestMonoBehaviour>();

            collection.SetEntries(new List<KeyedFactoryEntry<string, TestMonoBehaviour>>
            {
                new KeyedFactoryEntry<string, TestMonoBehaviour>
                {
                    Key = "Enemy",
                    Component = prefab,
                    PoolSettings = new PoolSettings()
                }
            });

            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();
            factory.Register(collection);

            Assert.IsTrue(factory.IsRegistered("Enemy"));

            var instance = factory.Get("Enemy");
            Assert.NotNull(instance);

            UnityEngine.Object.DestroyImmediate(prefabGo);
            UnityEngine.Object.DestroyImmediate(instance.gameObject);
            UnityEngine.Object.DestroyImmediate(collection);
        }

        [Test]
        public void KeyedPoolFactory_Register_FromNullCollection_DoesNothing()
        {
            var factory = new KeyedPoolFactory<string, TestMonoBehaviour>();

            Assert.DoesNotThrow(() => factory.Register((KeyedFactoryCollection<string, TestMonoBehaviour>)null));
        }

    }
}