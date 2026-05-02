namespace CreativeArcana.Factory.Example
{
    public class Orc:Enemy
    {
        public override void Attack()
        {
            UnityEngine.Debug.Log("Orc Attacked!");
        }

        public override void OnPoolGet(string context)
        {
            UnityEngine.Debug.Log($"Orc initialized with context {context}");
        }
    }
}