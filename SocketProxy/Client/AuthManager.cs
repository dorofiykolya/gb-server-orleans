using System.Threading.Tasks;
using Database;
using Newtonsoft.Json.Linq;
using SocketProxy.Packets;

namespace SocketProxy
{
    public class AuthManager
    {
        public Task<Auth> GetAuthByDevelopers(string developerId)
        {
            var user = Task.Run(() =>
            {
                var table = DatabaseUsers.GetByDeveloperId(developerId);
                return new Auth
                {
                    UserId = table.UserId,
                    AuthKey = table.AuthKey,
                    AuthTime = table.AuthTime
                };
            });
            return user;
        }

        public Task<AuthState> CheckAuth(int userAuthUserKey, string userAuthAuthKey, int userAuthAuthTs, bool userAuthIsBrowser)
        {
            return Task.FromResult(AuthState.Success);
        }
    }

    public enum AuthState
    {
        Success = 0,
        IncorrectUserKey = 1,
        SessionTimeOut = 2,
        IncorrectAuthKey = 3,
        Error = 4
    }
}
