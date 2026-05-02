using UnityEngine;

namespace CreativeArcana.Factory.Example
{
    [CreateAssetMenu(fileName = "EnemyKeyedFactoryCollection",menuName = "CreativeArcana/Factory/Example/Data/EnemyKeyedFactoryCollection")]
    public class EnemyKeyedFactoryCollection : KeyedFactoryCollection<EnemyType,Enemy>
    {
        
    }
}
