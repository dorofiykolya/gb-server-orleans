﻿using System;
using System.Collections.Generic;

namespace BattleEngine.Utils
{
    public class PriorityQueueComparable<T> where T : IComparable
    {
        private List<T> _data;

        public PriorityQueueComparable()
        {
            _data = new List<T>();
        }

        public void enqueue(T item)
        {
            _data.Add(item);
            var ci = _data.Count - 1;
            while (ci > 0)
            {
                var pi = (ci - 1) / 2;
                if (_data[ci].CompareTo(_data[pi]) >= 0)
                {
                    break;
                }
                var tmp = _data[ci];
                _data[ci] = _data[pi];
                _data[pi] = tmp;
                ci = pi;
            }
        }

        public T dequeue()
        {
            var li = _data.Count - 1;
            var frontItem = _data[0];
            _data[0] = _data[li];
            _data.RemoveAt(li);

            --li;
            var pi = 0;
            while (true)
            {
                var ci = pi * 2 + 1;
                if (ci > li)
                {
                    break;
                }
                var rc = ci + 1;
                if (rc <= li && _data[rc].CompareTo(_data[ci]) < 0)
                    ci = rc;
                if (_data[pi].CompareTo(_data[ci]) <= 0)
                {
                    break;
                }
                var tmp = _data[pi];
                _data[pi] = _data[ci];
                _data[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }

        public T peek()
        {
            var frontItem = _data[0];
            return frontItem;
        }

        public int count
        {
            get { return _data.Count; }
        }
    }
}