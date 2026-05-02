using UnityEngine;

namespace CreativeArcana.Factory.Example
{
    public class Bullet : MonoBehaviour,IBullet,IPoolable
    {
        [SerializeField] private Rigidbody _rb;
        
        public void Shoot()
        {
            _rb.linearVelocity = Vector3.forward;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        
        public void OnPoolCreate()
        {
            Debug.Log($"OnCreate {gameObject.name}");
        }

        public void OnPoolGet()
        {
            Debug.Log($"OnGet {gameObject.name}");
        }

        public void OnPoolRelease()
        {
            Debug.Log($"OnRelease {gameObject.name}");
        }

        public void OnPoolDestroy()
        {
            Debug.Log($"OnDestroy {gameObject.name}");
        }
    }
}