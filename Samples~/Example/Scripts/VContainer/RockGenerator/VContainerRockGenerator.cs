#if FACTORY_VCONTAINER
using System.Collections;
using CreativeArcana.Factory.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace CreativeArcana.Factory.Example.VContainer
{
    public class VContainerRockGenerator : MonoBehaviour
    {
        private IPoolFactory<GameObject> _factory;

        [Inject]
        private void Construct(IPoolFactory<GameObject> factory)
        {
            _factory = factory;
        }

        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
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
            rock.transform.position = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f));
            StartCoroutine(IERelease(rock, 3));
        }

        private IEnumerator IERelease(GameObject rock, float delay)
        {
            yield return new WaitForSeconds(delay);
            _factory.Release(rock);
        }
    }
}
#endif