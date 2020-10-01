using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Common.Composite.Events;
using Common.Events;

namespace Common.Composite
{
  public class Component : EventDispatcher
  {
    private static readonly ConcurrentDictionary<Type, ConcurrentStack<Component>> PoolStack = new ConcurrentDictionary<Type, ConcurrentStack<Component>>();

    public static Component Instantiate(Type type)
    {
      ConcurrentStack<Component> stack;
      if (PoolStack.TryGetValue(type, out stack) && !stack.IsEmpty)
      {
        Component result;
        if (stack.TryPop(out result))
        {
          result._disposed = false;
          result.Reinitialize();
          return result;
        }
      }

      return Activator.CreateInstance(type) as Component;
    }

    public static T Instantiate<T>() where T : Component, new()
    {
      return (T)Instantiate(typeof(T));
    }

    private static void DisposeComponent(Component component)
    {
      if (component == null) throw new ArgumentNullException();
      if (!component._disposed)
      {
        ConcurrentStack<Component> stack;
        if (!PoolStack.TryGetValue(component.GetType(), out stack))
        {
          stack = new ConcurrentStack<Component>();
          PoolStack[component.GetType()] = stack;
        }
        component._disposed = true;
        stack.Push(component);
      }
      else
      {
        throw new ArgumentException("component exist in pool, " + component.GetType());
      }
    }

    internal bool _disposed;
    internal Entity _parent;
    internal Entity _entity;
    internal string _name = "";
    internal bool _enabled = true;

    public event Action<Event> ADDED;
    public event Action<Event> REMOVED;

    public Component()
    {
      MapEvent<Event>(Event.ADDED, DispatchAdded);
      MapEvent<Event>(Event.REMOVED, DispatchRemoved);
    }

    public string Name
    {
      get { return _name; }
      set
      {
        if (value == null)
        {
          value = string.Empty;
        }
        _name = value;
      }
    }

    public bool Enabled
    {
      get { return _enabled; }
      set
      {
        if (_enabled != value)
        {
          _enabled = value;
          if (_enabled)
          {
            OnEnable();
          }
          else
          {
            OnDisable();
          }
        }
      }
    }

    public bool Active
    {
      get
      {
        if (_enabled)
        {
          if (_parent != null)
          {
            return _parent.Active;
          }
          return true;
        }
        return false;
      }
    }

    public virtual Component AddComponent(Component component)
    {
      if (_entity != null)
      {
        return _entity.AddComponent(component);
      }
      return null;
    }

    public virtual Component AddComponent(Type componentType)
    {
      if (_entity != null)
      {
        return _entity.AddComponent(componentType);
      }
      return null;
    }

    public T AddComponent<T>() where T : Component
    {
      return (T)AddComponent(typeof(T));
    }

    public virtual bool RemoveComponent(Component component)
    {
      if (_entity != null)
      {
        return _entity.RemoveComponent(component);
      }
      return false;
    }

    public virtual bool RemoveComponents(Type componentType)
    {
      if (_entity != null)
      {
        return _entity.RemoveComponents(componentType);
      }
      return false;
    }

    public bool RemoveComponents<T>() where T : Component
    {
      return RemoveComponents(typeof(T));
    }

    public virtual Component GetComponent(Type componentType)
    {
      if (_entity != null)
      {
        return _entity.GetComponent(componentType);
      }
      return null;
    }

    public virtual Component GetComponentByName(string name)
    {
      if (_entity != null)
      {
        return _entity.GetComponentByName(name);
      }
      return null;
    }

    public virtual T GetComponent<T>() where T : class
    {
      return GetComponent(typeof(T)) as T;
    }

    public virtual List<Component> GetComponents(Type type = null, bool includeInactive = false, List<Component> result = null)
    {
      if (_entity != null)
      {
        return _entity.GetComponents(type, includeInactive, result);
      }
      return null;
    }

    public virtual List<Component> GetComponentsInChildren(Type compoentType = null, bool recursive = true, bool includeInactive = false, List<Component> result = null)
    {
      if (_entity != null)
      {
        return _entity.GetComponentsInChildren(compoentType, recursive, includeInactive, result);
      }
      return null;
    }

    public virtual List<T> GetComponentsInChildren<T>(bool recursive = true, bool includeInactive = false, List<Component> result = null)
    {
      if (_entity != null)
      {
        return _entity.GetComponentsInChildren<T>(recursive, includeInactive, result);
      }
      return null;
    }

    public virtual Entity Parent
    {
      get { return _parent; }
      set
      {
        if (value == this)
        {
          throw new ArgumentException(this + ", An parent cannot be added as a child to itself or one of its children (or children's children, etc.)");
        }
        if (value != _parent)
        {
          if (_parent != null)
          {
            _parent.RemoveComponent(this);
          }
          if (value != null)
          {
            value.AddComponent(this);
          }
        }
      }
    }

    public virtual Entity Entity
    {
      get { return _entity; }
    }

    public Component Root
    {
      get
      {
        var target = this;
        while (target._parent != null)
        {
          target = target._parent;
        }
        return target;
      }
    }

    public bool Disposed { get { return _disposed; } }

    public override void Dispose()
    {
      if (!_disposed)
      {
        OnDispose();
        DisposeInternal();
        if (_entity != null)
        {
          _entity.RemoveComponent(this);
        }
        base.Dispose();
        Component.DisposeComponent(this);
      }
    }

    internal virtual void DisposeInternal()
    {

    }

    internal void SetParent(Entity value)
    {
      var ancestor = value;
      while (ancestor != this && ancestor != null)
      {
        ancestor = ancestor._parent;
      }
      if (ancestor == this)
      {
        throw new ArgumentException(GetType() + ", An object cannot be added as a child to itself or one of its children (or children's children, etc.)");
      }
      _parent = _entity = value;
    }

    internal void AttachToParent()
    {
      OnAttach();
    }

    internal void DetachFromParent()
    {
      OnDetach();
    }

    internal void DispatchAdded(Event evt)
    {
      ADDED?.Invoke(evt);
    }

    internal void DispatchRemoved(Event evt)
    {
      REMOVED?.Invoke(evt);
    }

    protected virtual void Reinitialize()
    {

    }

    protected virtual void OnAttach()
    {

    }

    protected virtual void OnDetach()
    {

    }

    protected virtual void OnDispose()
    {

    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }
  }
}
