#if FACTORY_VCONTAINER
using System.Collections;
using CreativeArcana.Factory.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace CreativeArcana.Factory.Example.VContainer
{
    public class VContainerBulletSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private PoolSettings _poolSettings;

        private IPoolFactory<IBullet> _factory;

        [Inject]
        private void Construct(IPoolFactory<IBullet> factory)
        {
            _factory = factory;
        }

        private void Start()
        {
            _factory.ApplyInitialPreWarm();
        }

        private void Update()
        {
            if (Keyboard.current.bKey.wasPressedThisFrame)
            {
                Spawn();
            }

            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                PoolDiagnostics.LogStatus(_factory);
            }
        }

        private void Spawn()
        {
            var spawnedBullet = _factory.Get();
            spawnedBullet.Shoot();
            spawnedBullet.SetPosition(_spawnPoint.position);
            StartCoroutine(IERelease(spawnedBullet, 3));
        }

        private IEnumerator IERelease(IBullet bullet, float delay)
        {
            yield return new WaitForSeconds(delay);
            _factory.Release(bullet);
        }
    }
}
#endif