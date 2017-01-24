using System;

namespace Common.Composite
{
    public class Entity : Component
    {
        internal ComponentCollection _components;

        public Entity()
        {
            _components = new ComponentCollection(this);
        }

        public IComponentCollection components
        {
            get { return _components; }
        }

    }
}
