using System;
using System.Collections.Generic;

namespace Common.Composite
{
    public class Entity : Component
    {
        internal ComponentCollection _components;

        public Entity()
        {
            _components = new ComponentCollection(this);
        }

        public IComponentCollection Components
        {
            get { return _components; }
        }

        public override Component AddComponent(Type componentType)
        {
            return AddComponent(Component.Instantiate(componentType));
        }

        public override Component AddComponent(Component component)
        {
            var targetEntity = component._entity;
            if (targetEntity != null)
            {
                targetEntity.RemoveComponent(component);
            }

            _components.Add(component);
            return component;
        }

        public override bool RemoveComponents(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException(this + ", type can not be null");
            }
            var result = false;
            var components = GetComponents(type);
            foreach (var item in components)
            {
                item.Dispose();
                result = true;
            }
            return result;
        }

        public override bool RemoveComponent(Component component)
        {
            if (component == null)
            {
                throw new ArgumentException(this + ", component can not be null");
            }
            var result = _components.Remove(component);
            return result;
        }

        public override Component GetComponent(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException(this + ", type can not be null");
            }
            return _components.GetByType(type);
        }

        public override Component GetComponentByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentException(this + ", name can not be null");
            }
            return _components.GetByName(name);
        }

        public override List<Component> GetComponents(Type type = null, bool includeInactive = false, List<Component> result = null)
        {
            return _components.GetComponents(type, includeInactive, result);
        }

        public override List<Component> GetComponentsInChildren(Type type = null, bool recursive = true, bool includeInactive = false, List<Component> result = null)
        {
            return _components.GetComponentsInChildren(type, recursive, includeInactive, result);
        }
    }
}
