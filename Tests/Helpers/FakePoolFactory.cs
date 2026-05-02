using System;
using UnityEngine;

namespace CreativeArcana.Factory.Tests.Helpers
{
    public sealed class FakePoolFactory<T> : IPoolFactory<T> where T : class
    {
        private readonly Func<T> _getFunc;

        public int CountActive { get; set; }
        public int CountInactive { get; set; }

        public int GetCallCount { get; private set; }
        public int ReleaseCallCount { get; private set; }
        public int PreWarmCallCount { get; private set; }
        public int ClearPoolCallCount { get; private set; }
        public int ApplyInitialPreWarmCallCount { get; private set; }

        public int LastPreWarmCount { get; private set; }
        public T LastReleasedInstance { get; private set; }

        public FakePoolFactory(Func<T> getFunc)
        {
            _getFunc = getFunc ?? throw new ArgumentNullException(nameof(getFunc));
        }

        public T Get()
        {
            GetCallCount++;
            return _getFunc();
        }

        public void Release(T instance)
        {
            ReleaseCallCount++;
            LastReleasedInstance = instance;
        }

        public void PreWarm(int count)
        {
            PreWarmCallCount++;
            LastPreWarmCount = count;
        }

        public void ClearPool()
        {
            ClearPoolCallCount++;
        }

        public void ApplyInitialPreWarm()
        {
            ApplyInitialPreWarmCallCount++;
        }
    }
}
