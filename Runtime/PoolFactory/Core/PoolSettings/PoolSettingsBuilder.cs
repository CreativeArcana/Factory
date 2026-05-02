namespace CreativeArcana.Factory
{
    public sealed class PoolSettingsBuilder
    {
        private readonly PoolSettings _settings = new();

        public PoolSettingsBuilder MaxSize(int size)
        {
            _settings.MaxSize = size;
            return this;
        }

        public PoolSettingsBuilder DefaultCapacity(int size)
        {
            _settings.DefaultCapacity = size;
            return this;
        }

        public PoolSettingsBuilder PreWarmCount(int size)
        {
            _settings.PreWarmCount = size;
            return this;
        }

        public PoolSettingsBuilder CollectionCheck(bool enabled)
        {
            _settings.CollectionCheck = enabled;
            return this;
        }

        public PoolSettings Build()
        {
            var result = _settings.Clone();
            result.Validate();
            return result;
        }
    }
}