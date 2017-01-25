using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Composite
{
    public interface IComponentCollection : IEnumerable<Component>, IEnumerable
    {
        //--------------------------------------------------------------------------
        //     
        //	PUBLIC PROPERTIES 
        //     
        //--------------------------------------------------------------------------

        int count { get; }
        Component[] components { get; }

        //--------------------------------------------------------------------------
        //     
        //	PUBLIC METHODS 
        //     
        //--------------------------------------------------------------------------

        Component Add(Component component);
        Component AddAt(Component component, int index);
        void AddRange(IEnumerable<Component> collection);
        bool Contains(Component component, bool includingChildren = false);
        Component GetAt(int index);
        Component GetByName(string name);
        Component GetByType(Type type);
        Component[] GetCollection();
        int GetIndex(Component component);
        Component[] GetRange(int index, int count);
        bool Remove(Component component);
        void RemoveAll();
        Component RemoveAt(int index);
        Component[] RemoveRange(int beginIndex = 0, int endIndex = -1);
        void Reverse();
        void SetIndex(Component component, int index);
        void Sort(IComparer<Component> sortFunction);
        void Swap(Component component1, Component component2);
        void SwapIndex(int index1, int index2);
    }
}
