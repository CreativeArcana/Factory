using UnityEngine;

namespace CreativeArcana.Factory.Example
{
    public class Undead:Enemy
    {
        public override void Attack()
        {
            Debug.Log("Undead Attacked!");
        }
        
        public override void OnPoolGet(string context)
        {
            UnityEngine.Debug.Log($"Undead initialized with context {context}");
        }
    }
}