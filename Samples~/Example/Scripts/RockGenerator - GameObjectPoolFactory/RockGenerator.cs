using System.Collections;
using CreativeArcana.Factory.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CreativeArcana.Factory.Example
{
    public class RockGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _rockPrefab;
        [SerializeField] private Transform _parent;

        private IPoolFactory<GameObject> _factory;

        private void Start()
        {
            var poolSettings = new PoolSettingsBuilder()
                .MaxSize(1000)
                .DefaultCapacity(10)
                .PreWarmCount(3)
                .CollectionCheck(true)
                .Build();
            _factory = new GameObjectPoolFactory(_rockPrefab, _parent, poolSettings);
            _factory.ApplyInitialPreWarm();
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Generate();
            }

            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                PoolDiagnostics.LogStatus(_factory);
            }
        }

        private void Generate()
        {
            var rock = _factory.Get();
            rock.transform.position = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            StartCoroutine(IERelease(rock, 3));
        }

        private IEnumerator IERelease(GameObject rock, float delay)
        {
            yield return new WaitForSeconds(delay);
            _factory.Release(rock);
        }
    }
}