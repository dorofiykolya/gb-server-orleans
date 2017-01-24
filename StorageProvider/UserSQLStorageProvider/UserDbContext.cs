using System.Data.Entity;
using Grains;

namespace StorageProvider.UserSQLStorageProvider
{
    [DbConfigurationType(typeof(KeyValueDbConfiguration))]
    public class UserDbContext : DbContext
    {
        public UserDbContext(string connString) : base(connString)
        {

        }

        public DbSet<UserInfoState> UserInfo { get; set; }
    }
}
