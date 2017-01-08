using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProxy.Users.Commands;

namespace SocketProxy.Users
{
    public class CommandFactoryProvider
    {
        private readonly Dictionary<Type, Type> _commands = new Dictionary<Type, Type>();

        public void Add<TCommandType, TCommand>() where TCommand : ICommand, new()
        {
            _commands[typeof(TCommandType)] = typeof(TCommand);
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
