#if FACTORY_VCONTAINER
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace CreativeArcana.Factory.Example.VContainer.EnemySpawner
{
    public class VContainerEnemySpawner : MonoBehaviour
    {
        private IKeyedPoolFactory<EnemyType, Enemy> _factory;
        private KeyedFactoryCollection<EnemyType, Enemy> _config;

        [Inject]
        private void Construct(
            IKeyedPoolFactory<EnemyType, Enemy> factory,
            KeyedFactoryCollection<EnemyType, Enemy> config)
        {
            _factory = factory;
            _config = config;
        }

        private void Start()
        {
            _factory.Register(_config, transform);
            _factory.ApplyInitialPreWarm(EnemyType.Orc);
        }

        private void Update()
        {
            if (Keyboard.current.oKey.wasPressedThisFrame)
            {
                var orc = _factory.Get(EnemyType.Orc,"Strong");
                orc.transform.position = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f),
                    Random.Range(-3.0f, 3.0f));
                orc.Attack();
                StartCoroutine(IERelease(orc, 3.0f));
            }

            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                var undead = _factory.Get(EnemyType.Undead,"Runner");
                undead.transform.position = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f),
                    Random.Range(-3.0f, 3.0f));
                undead.Attack();
                StartCoroutine(IERelease(undead, 3.0f));
            }
        }

        private IEnumerator IERelease(Enemy enemy, float delay)
        {
            yield return new WaitForSeconds(delay);
            _factory.Release(enemy);
        }
    }
}
#endif