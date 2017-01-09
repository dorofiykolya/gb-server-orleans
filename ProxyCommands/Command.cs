using System.Threading.Tasks;

namespace ProxyCommands
{
    public interface ICommand
    {
        Task Execute(IUserContextProvider context, object data);
    }

    public abstract class Command<T> : ICommand
    {
        public T Data { get; private set; }

        public IUserContextProvider Context { get; private set; }

        public int UserId { get; private set; }

        protected abstract Task Execute();

        public Task Execute(IUserContextProvider context, object data)
        {
            Data = (T)data;
            Context = context;
            UserId = context.UserId;
            return Execute();
        }
    }
}
