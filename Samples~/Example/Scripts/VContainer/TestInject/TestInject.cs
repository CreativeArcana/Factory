#if FACTORY_VCONTAINER
using UnityEngine;
using VContainer;

namespace CreativeArcana.Factory.Example.VContainer
{
    public class TestInject : MonoBehaviour
    {
        [Inject]
        public void Construct(FakeService service)
        {
            service.Execute(gameObject.name + " " + transform.GetInstanceID().ToString());
        }
    }
}
#endif