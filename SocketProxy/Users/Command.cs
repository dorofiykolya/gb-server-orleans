using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketProxy.Users.Commands
{
    public interface ICommand
    {
        Task Execute(UserContext context, object data);
    }

    public abstract class Command<T> : ICommand
    {
        public T Data { get; private set; }

        public UserContext Context { get; private set; }

        public int UserId { get; private set; }

        protected abstract Task Execute();

        public Task Execute(UserContext context, object data)
        {
            Data = (T)data;
            Context = context;
            UserId = context.UserId;
            return Execute();
        }
    }
}
