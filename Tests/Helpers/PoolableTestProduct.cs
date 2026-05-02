namespace CreativeArcana.Factory.Tests.Helpers
{
    public class PoolableTestProduct: IPoolable<string>
    {
        public int CreateCallCount;
        public int GetCallCount;
        public int ReleaseCallCount;
        public int DestroyCallCount;
        public int ContextGetCallCount;
        public string LastContext;

        public void OnPoolCreate()
        {
            CreateCallCount++;
        }

        public void OnPoolGet()
        {
            GetCallCount++;
        }

        public void OnPoolGet(string context)
        {
            ContextGetCallCount++;
            LastContext = context;
        }

        public void OnPoolRelease()
        {
            ReleaseCallCount++;
        }

        public void OnPoolDestroy()
        {
            DestroyCallCount++;
        }
    }

}