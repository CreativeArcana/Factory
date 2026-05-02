#if FACTORY_VCONTAINER
using System;
using UnityEngine;

namespace CreativeArcana.Factory.Example.VContainer
{
    [Serializable]
    public class RockGeneratorReferences
    {
        [SerializeField] private VContainerRockGenerator _generator;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parent;

        public VContainerRockGenerator Generator => _generator;
        public GameObject Prefab => _prefab;
        public Transform Parent => _parent;
    }
}
#endif