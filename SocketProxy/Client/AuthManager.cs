using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SocketProxy
{
    public class AuthManager
    {
        public Task<bool> CheckAuthByDevelopers(JToken keyValue)
        {
            return Task.FromResult(true);
        }

        public Task<bool> CheckAuthByBrowser(JToken keyValue)
        {
            return Task.FromResult(true);
        }
    }
}
