using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Composite
{
    public class ComponentCollection : IComponentCollection, IDisposable
    {
        internal int _count;
        internal Entity _entity;
        internal List<Component> _collection;
        internal List<Component> _iteratorCollection;

        public ComponentCollection(Entity entity)
        {
            _entity = entity;
            _count = 0;
            _entity = entity;
            _collection = new List<Component>();
        }

        /* INTERFACE engine.IChildList */

        public int count
        {
            get { return _count; }
        }

        public Component[] components
        {
            get { return _collection.ToArray(); }
        }

        public Component Add(Component child)
        {
            return AddAt(child, _count);
        }

        public Component AddAt(Component child, int index)
        {
            if (child == null)
            {
                throw new ArgumentException(this + ", child can not be null");
            }
            if (child == _entity)
            {
                throw new ArgumentException(this + ", An object cannot be added as a child to itself or one " + "of its children (or children's children, etc.)");
            }
            if (index >= 0 && index <= _count)
            {
                if (child.Parent != _entity)
                {
                    if (child.Parent != null)
                    {
                        child.Parent._components.Remove(child);
                    }
                    if (index == _count)
                    {
                        _collection[_count] = child;
                    }
                    else
                    {
                        _collection.Insert(index, child);
                    }
                    _count++;
                    child.SetParent(_entity);
                    child.AttachToParent();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("index", "Invalid child index");
            }
            return child;
        }

        public void Swap(Component child1, Component child2)
        {
            var index1 = GetIndex(child1);
            var index2 = GetIndex(child2);
            if (index1 == -1 || index2 == -1)
            {
                throw new ArgumentException("Not a child of this entity");
            }
            SwapIndex(index1, index2);
        }

        public void SwapIndex(int index1, int index2)
        {
            var child1 = GetAt(index1);
            var child2 = GetAt(index2);
            _collection[index1] = child2;
            _collection[index2] = child1;
        }

        public void SetIndex(Component child, int index)
        {
            var oldIndex = GetIndex(child);
            if (oldIndex == index)
            {
                return;
            }
            if (oldIndex == -1)
            {
                throw new ArgumentException("Not a child of this entity");
            }
            _collection.RemoveAt(oldIndex);
            _collection.Insert(index, child);
        }

        public int GetIndex(Component child)
        {
            return _collection.IndexOf(child);
        }

        public Component GetByName(string name)
        {
            foreach (var item in _collection)
            {
                if (item._name == name)
                {
                    return item;
                }
            }
            return null;
        }

        public bool Remove(Component child)
        {
            if (child == null)
            {
                throw new ArgumentException(this + ", child can not be null");
            }
            if (child == _entity)
            {
                throw new ArgumentException(this + ", An object cannot be removed as a child to itself or one " + "of its children (or children's children, etc.)");
            }
            var index = GetIndex(child);
            if (index != -1)
            {
                return RemoveAt(index) != null;
            }
            return false;
        }

        public Component RemoveAt(int index)
        {
            if (index >= 0 && index < _count)
            {
                var child = _collection[index];
                child.DetachFromParent();
                child.SetParent(null);
                index = _collection.IndexOf(child);
                if (index >= 0)
                {
                    _collection.RemoveAt(index);
                    _count--;
                }
                return child;
            }
            return null;
        }

        public Component[] GetCollection()
        {
            return _collection.ToArray();
        }

        public bool Contains(Component child, bool includingChildren = false)
        {
            if (child == null)
            {
                throw new ArgumentException(this + ", child can not be null");
            }
            if (child == _entity)
            {
                throw new ArgumentException(this + ", An object cannot be added as a child to itself or one " + "of its children (or children's children, etc.)");
            }
            if (includingChildren)
            {
                while (child != null)
                {
                    if (child == _entity)
                        return true;
                    else
                        child = child.Parent;
                }
                return false;
            }
            return child.Parent == _entity;
        }

        internal List<Component> GetComponents(Type type = null, bool includeInactive = false, List<Component> result = null)
        {
            result = result ?? (result = new List<Component>());
            var index = result.Count;
            foreach (var component in _collection)
            {
                if ((type == null || component.GetType().IsAssignableFrom(type)) && (includeInactive || component._enabled))
                {
                    result[index] = component;
                    index++;
                }
            }
            result.RemoveRange(index, result.Count - index);
            return result;
        }

        internal List<Component> GetComponentsInChildren(Type type = null, bool recursive = false, bool includeInactive = false, List<Component> result = null)
        {
            result = result ?? (result = new List<Component>());
            var index = result.Count;
            foreach (var component in _collection)
            {
                if ((type == null || component.GetType().IsAssignableFrom(type)) && (includeInactive || component._enabled))
                {
                    result[index] = component;
                    index++;
                }
                var entity = component as Entity;
                if (entity != null)
                {
                    entity.GetComponentsInChildren(type, recursive, includeInactive, result);
                    index = result.Count;
                }
            }
            result.RemoveRange(index, result.Count - index);
            return result;
        }

        public void RemoveAll()
        {
            RemoveRange();
            _collection.Clear();
            _count = 0;
        }

        public Component GetAt(int index)
        {
            return _collection[index];
        }

        /* INTERFACE common.entity.IChildList */

        public void AddRange(IEnumerable<Component> collection)
        {
            if (collection == null)
            {
                throw new ArgumentException(this + ", collection can not be null");
            }
            foreach (var item in collection)
            {
                AddAt(item, _count);
            }
        }

        public Component GetByType(Type type)
        {
            if (type != typeof(Component) && !type.GetType().IsAssignableFrom(type))
            {
                throw new ArgumentException(this + ", type must extend the " + typeof(Component));
            }
            foreach (var item in _collection)
            {
                if (item.GetType().IsAssignableFrom(type))
                {
                    return item;
                }
            }
            return null;
        }

        public Component[] GetRange(int index, int count)
        {
            if (index >= _count)
            {
                throw new ArgumentOutOfRangeException("index", "Invalid index");
            }
            return _collection.GetRange(index, index + count).ToArray();
        }

        public Component[] RemoveRange(int beginIndex = 0, int endIndex = -1)
        {
            if (beginIndex < 0 || endIndex >= _count)
            {
                throw new ArgumentOutOfRangeException("index", "Invalid index");
            }

            var result = new List<Component>();
            if (endIndex < 0 || endIndex >= _count)
            {
                endIndex = _count - 1;
            }
            for (var i = beginIndex; i <= endIndex; ++i)
            {
                result.Add(RemoveAt(beginIndex));
            }
            return result.ToArray();
        }

        public void Reverse()
        {
            _collection.Reverse();
        }

        public void Sort(IComparer<Component> sortFunc)
        {
            _collection.Sort(sortFunc);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public IEnumerator<Component> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public void Dispose()
        {
            var temp = _collection.ToArray();
            var index = temp.Length;
            while (index > 0)
            {
                --index;
                temp[index].Dispose();
            }
            _iteratorCollection = null;
            _count = 0;
            _collection.Clear();
        }
    }
}
