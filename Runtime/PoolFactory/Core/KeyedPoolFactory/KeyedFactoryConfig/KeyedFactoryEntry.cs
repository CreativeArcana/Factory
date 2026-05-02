using System;
using UnityEngine;

namespace CreativeArcana.Factory
{
    [Serializable]
    public class KeyedFactoryEntry<TKey,TComponent>
    {
        public TKey Key;
        public TComponent Component;
        public PoolSettings PoolSettings = new();
    }
}