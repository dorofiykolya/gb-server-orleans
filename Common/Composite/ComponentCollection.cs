using System;

namespace Common.Composite
{
    public class ComponentCollection : IComponentCollection, IDisposable
    {
        private readonly Entity _entity;

        public ComponentCollection(Entity entity)
        {
            _entity = entity;
        }

        public void Dispose()
        {
        }
    }
}
