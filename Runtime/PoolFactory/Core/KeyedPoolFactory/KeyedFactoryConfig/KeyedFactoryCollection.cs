using System.Collections.Generic;
using UnityEngine;

namespace CreativeArcana.Factory
{
    public abstract class KeyedFactoryCollection<TKey, TComponent> : ScriptableObject
    {
        [SerializeField] protected List<KeyedFactoryEntry<TKey, TComponent>> _entries;

        public IReadOnlyList<KeyedFactoryEntry<TKey, TComponent>> Entries => _entries;
    }
}