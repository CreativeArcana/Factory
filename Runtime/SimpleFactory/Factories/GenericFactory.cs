using System;

namespace CreativeArcana.Factory
{
    /// <summary>
    /// Concrete generic factory powered by a user-supplied delegate. No pooling.
    /// </summary>
    /// <typeparam name="T">Product</typeparam>
    public sealed class GenericFactory<T> : FactoryBase<T> where T : class
    {
        private readonly Func<T> _createFunc;
        private readonly Action<T> _afterCreate;

        public GenericFactory(Func<T> createFunc, Action<T> afterCreate = null)
        {
            _createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
            _afterCreate = afterCreate;
        }

        protected override T CreateInstance() => _createFunc();

        protected override void OnAfterCreate(T instance) => _afterCreate?.Invoke(instance);
    }
}