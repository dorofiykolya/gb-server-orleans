using System.Threading.Tasks;

namespace ProxyCommands
{
    public interface IUserContextProvider
    {
        int UserId { get; }
        Task Send(object value);
    }
}
