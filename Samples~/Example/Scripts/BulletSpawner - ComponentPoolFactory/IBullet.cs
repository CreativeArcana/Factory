using UnityEngine;

namespace CreativeArcana.Factory.Example
{
    public interface IBullet
    {
        void Shoot();
        void SetPosition(Vector3 position);
    }
}