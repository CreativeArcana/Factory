using System.Collections.Generic;

namespace CreativeArcana.Factory.Tests.Helpers
{
    public class TestKeyedFactoryCollection: KeyedFactoryCollection<string, TestMonoBehaviour>
    {
        public void SetEntries(List<KeyedFactoryEntry<string, TestMonoBehaviour>> entries)
        {
            _entries = entries;
        }
    }
}