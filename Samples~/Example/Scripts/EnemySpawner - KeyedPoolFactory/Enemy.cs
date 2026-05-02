using UnityEngine;

namespace CreativeArcana.Factory.Example
{
    public abstract class Enemy : MonoBehaviour, IPoolable<string>
    {
        public abstract void Attack();
        public abstract void OnPoolGet(string context);

        public virtual void OnPoolCreate()
        {
        }

        public virtual void OnPoolGet()
        {
        }

        public virtual void OnPoolRelease()
        {
        }

        public virtual void OnPoolDestroy()
        {
        }
    }
}