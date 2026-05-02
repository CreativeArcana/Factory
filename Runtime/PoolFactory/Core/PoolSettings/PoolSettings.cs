using System;
using UnityEngine;

namespace CreativeArcana.Factory
{
    [Serializable]
    public sealed class PoolSettings
    {
        [Tooltip("Throw if pool grows beyond this cap.")]
        public int MaxSize = 1000;

        [Tooltip("Default capacity when the pool is first created.")]
        public int DefaultCapacity = 10;

        [Tooltip("How many instances to pre-warm on initialization.")]
        public int PreWarmCount = 0;

        [Tooltip("Check collection when releasing (slower but safer in debug).")]
        public bool CollectionCheck = true;

        private static readonly PoolSettings _default = new();
        public static PoolSettings Default => _default;

        public void Validate()
        {
            if (MaxSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(MaxSize), "MaxSize must be greater than 0.");

            if (DefaultCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(DefaultCapacity), "DefaultCapacity cannot be negative.");

            if (PreWarmCount < 0)
                throw new ArgumentOutOfRangeException(nameof(PreWarmCount), "PreWarmCount cannot be negative.");

            if (DefaultCapacity > MaxSize)
                throw new ArgumentException("DefaultCapacity cannot be greater than MaxSize.");

            if (PreWarmCount > MaxSize)
                throw new ArgumentException("PreWarmCount cannot be greater than MaxSize.");
        }

        public PoolSettings Clone()
        {
            return new PoolSettings
            {
                MaxSize = MaxSize,
                DefaultCapacity = DefaultCapacity,
                PreWarmCount = PreWarmCount,
                CollectionCheck = CollectionCheck
            };
        }
    }
}