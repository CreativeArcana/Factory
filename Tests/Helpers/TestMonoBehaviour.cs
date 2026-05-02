using CreativeArcana.Factory;
using UnityEngine;

namespace CreativeArcana.Factory.Tests.Helpers
{
    public class TestMonoBehaviour : MonoBehaviour, IPoolable<string>
    {
        public static int TotalCreateCalls;
        public static int TotalGetCalls;
        public static int TotalReleaseCalls;
        public static int TotalDestroyCalls;

        public int CreateCallCount;
        public int GetCallCount;
        public int ReleaseCallCount;
        public int DestroyCallCount;
        public int ContextGetCallCount;
        
        public string LastContext;
            
        public static void ResetStatics()
        {
            TotalCreateCalls = 0;
            TotalGetCalls = 0;
            TotalReleaseCalls = 0;
            TotalDestroyCalls = 0;
        }

        public void OnPoolGet(string context)
        {
            ContextGetCallCount++;
            LastContext = context;
        }

        public void OnPoolCreate()
        {
            CreateCallCount++;
            TotalCreateCalls++;
        }

        public void OnPoolGet()
        {
            GetCallCount++;
            TotalGetCalls++;
        }

        public void OnPoolRelease()
        {
            ReleaseCallCount++;
            TotalReleaseCalls++;
        }

        public void OnPoolDestroy()
        {
            DestroyCallCount++;
            TotalDestroyCalls++;
        }
    }
}