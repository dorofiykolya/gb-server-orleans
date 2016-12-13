using System.Threading.Tasks;
using Database;
using Newtonsoft.Json.Linq;

namespace SocketProxy
{
    public class AuthManager
    {
        public Task<bool> CheckAuthByDevelopers(string developerId)
        {
            var userId = Task.Run(() => DatabaseUsers.GetUserIdByDeveloperId(developerId));

            return Task.FromResult(userId.Result > 0);
        }

        public Task<bool> CheckAuthByBrowser(JToken keyValue)
        {
            return Task.FromResult(true);
        }
    }
}
