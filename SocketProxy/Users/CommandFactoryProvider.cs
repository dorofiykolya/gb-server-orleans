using System;
using System.Collections.Generic;
using ProxyCommands;

namespace SocketProxy.Users
{
    public class CommandFactoryProvider
    {
        private readonly Dictionary<Type, Type> _commands = new Dictionary<Type, Type>();

        public void Add<TCommandType, TCommand>() where TCommand : ICommand, new()
        {
            Add(typeof(TCommandType), typeof(TCommand));
        }

        protected void Add(Type commandType, Type command)
        {
            _commands[commandType] = command;
        }

        public ICommand CreateInstance(Type packetContentType)
        {
            Type commandType;
            if (TryGetCommand(packetContentType, out commandType))
            {
                return (ICommand)Activator.CreateInstance(commandType);
            }
            return null;
        }

        public bool TryGetCommand(Type packetContentType, out Type commandType)
        {
            return _commands.TryGetValue(packetContentType, out commandType);
        }
    }
}
