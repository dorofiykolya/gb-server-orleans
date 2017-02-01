using System.Collections.Generic;

namespace BattleEngine.Utils
{
    public class Vector<T> : List<T>
    {
        public void push(T value)
        {
            Add(value);
        }

        public T pop()
        {
            var result = this[Count - 1];
            RemoveAt(Count - 1);
            return result;
        }

        public int length
        {
            get { return Count; }
            set
            {
                if (value < Count)
                {
                    RemoveRange(value, Count - value);
                }
                while (value > Count)
                {
                    Add(default(T));
                }
            }
        }
    }
}
