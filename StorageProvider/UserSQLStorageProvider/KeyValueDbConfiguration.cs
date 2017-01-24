using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace StorageProvider.UserSQLStorageProvider
{
    internal class KeyValueDbConfiguration : DbConfiguration
    {
        public KeyValueDbConfiguration()
        {
            SetProviderServices(
                SqlProviderServices.ProviderInvariantName,
                SqlProviderServices.Instance);

            SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
        }
    }
}
