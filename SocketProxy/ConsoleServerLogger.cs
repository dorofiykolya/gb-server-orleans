using System;
using DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.Logging;

namespace SocketProxy
{
    public class ConsoleServerLogger : IInternalLogger
    {
        public void Trace(string msg) => Console.WriteLine("Trace: " + msg);

        public void Trace(string format, object arg) => Console.WriteLine("Trace: " + format, arg);

        public void Trace(string format, object argA, object argB) => Console.WriteLine("Trace: " + format, argA, argB);

        public void Trace(string format, params object[] arguments) => Console.WriteLine("Trace: " + format, arguments);

        public void Trace(string msg, Exception t) => Console.WriteLine("Trace: " + msg + ", exception:" + t);

        public void Trace(Exception t) => Console.WriteLine("Trace: " + t);

        public void Debug(string msg) => Console.WriteLine("Debug: " + msg);

        public void Debug(string format, object arg) => Console.WriteLine("Debug: " + format, arg);

        public void Debug(string format, object argA, object argB) => Console.WriteLine("Debug: " + format, argA, argB);

        public void Debug(string format, params object[] arguments) => Console.WriteLine("Debug: " + format, arguments);

        public void Debug(string msg, Exception t) => Console.WriteLine("Debug: " + msg + ", exception:" + t);

        public void Debug(Exception t) => Console.WriteLine("Debug: " + t);

        public void Info(string msg) => Console.WriteLine("Info: " + msg);

        public void Info(string format, object arg) => Console.WriteLine("Info: " + format, arg);

        public void Info(string format, object argA, object argB) => Console.WriteLine("Info: " + format, argA, argB);

        public void Info(string format, params object[] arguments) => Console.WriteLine("Info: " + format, arguments);

        public void Info(string msg, Exception t) => Console.WriteLine("Info: " + msg + ", exception:" + t);

        public void Info(Exception t) => Console.WriteLine("Info: " + t);

        public void Warn(string msg) => Console.WriteLine("Warn: " + msg);

        public void Warn(string format, object arg) => Console.WriteLine("Warn: " + format, arg);

        public void Warn(string format, params object[] arguments) => Console.WriteLine("Warn: " + format, arguments);

        public void Warn(string format, object argA, object argB) => Console.WriteLine("Warn: " + format, argA, argB);

        public void Warn(string msg, Exception t) => Console.WriteLine("Warn: " + msg + ", exception:" + t);

        public void Warn(Exception t) => Console.WriteLine("Warn: " + t);

        public void Error(string msg) => Console.WriteLine("Error: " + msg);

        public void Error(string format, object arg) => Console.WriteLine("Error: " + format, arg);

        public void Error(string format, object argA, object argB) => Console.WriteLine("Error: " + format, argA, argB);

        public void Error(string format, params object[] arguments) => Console.WriteLine("Error: " + format, arguments);

        public void Error(string msg, Exception t) => Console.WriteLine("Error: " + msg + ", exception:" + t);

        public void Error(Exception t) => Console.WriteLine("Error: " + t);

        public void Log(InternalLogLevel level, string msg) => Console.WriteLine("Log-level[" + level + "]: " + msg);

        public void Log(InternalLogLevel level, string format, object arg) => Console.WriteLine("Log-level[" + level + "]: " + format, arg);

        public void Log(InternalLogLevel level, string format, object argA, object argB) => Console.WriteLine("Log-level[" + level + "]: " + format, argA, argB);

        public void Log(InternalLogLevel level, string format, params object[] arguments) => Console.WriteLine("Log-level[" + level + "]: " + format, arguments);

        public void Log(InternalLogLevel level, string msg, Exception t) => Console.WriteLine("Log-level[" + level + "]: " + msg + ", exception: " + t);

        public void Log(InternalLogLevel level, Exception t) => Console.WriteLine("Log-level[" + level + "]: " + t);

        public bool IsEnabled(InternalLogLevel level) => true;

        public string Name => "LOGGER:";
        public bool TraceEnabled => true;
        public bool DebugEnabled => true;
        public bool InfoEnabled => true;
        public bool WarnEnabled => true;
        public bool ErrorEnabled => true;
    }
}
