using System.Collections;
using CreativeArcana.Factory.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CreativeArcana.Factory.Example
{
    /// <summary>
    /// ComponentPoolFactory Example
    /// </summary>
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private PoolSettings _poolSettings;

        private IPoolFactory<IBullet> _factory;

        private void Start()
        {
            _factory = new ComponentPoolFactory<Bullet>(_bulletPrefab, _parent, _poolSettings)
                .AsInterface<Bullet, IBullet>();
            _factory.ApplyInitialPreWarm();
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
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