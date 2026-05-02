#if FACTORY_VCONTAINER
namespace CreativeArcana.Factory.Example.VContainer
{
    public class FakeService
    {
        public void Execute(string name)
        {
            UnityEngine.Debug.Log($"{name} => FakeService Executed!");
        }
    }
}
#endif