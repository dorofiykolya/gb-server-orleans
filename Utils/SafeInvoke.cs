using System;

namespace Utils
{
    public class SafeInvoke
    {
        public static Exception Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                return exception;
            }
            return null;
        }

        public static Exception Invoke<T>(Action<T> action, T value)
        {
            try
            {
                action(value);
            }
            catch (Exception exception)
            {
                return exception;
            }
            return null;
        }
    }
}
