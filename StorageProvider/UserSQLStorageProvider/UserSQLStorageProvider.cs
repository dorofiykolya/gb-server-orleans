using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grains;
using Newtonsoft.Json;
using Orleans;
using Orleans.Concurrency;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Serialization;
using Orleans.Storage;

namespace StorageProvider.UserSQLStorageProvider
{
    public class UserSQLStorageProvider : IStorageProvider
    {
        private SqlConnectionStringBuilder sqlconnBuilder;

        private const string CONNECTION_STRING = "ConnectionString";
        private const string USE_JSON_FORMAT_PROPERTY = "UseJsonFormat";

        private string serviceId;
        private Newtonsoft.Json.JsonSerializerSettings jsonSettings;


        /// <summary> Name of this storage provider instance. </summary>
        /// <see cref="IProvider#Name"/>
        public string Name { get; private set; }

        /// <summary> Logger used by this storage provider instance. </summary>
        /// <see cref="IStorageProvider#Log"/>
        public Logger Log { get; private set; }


        /// <summary> Initialization function for this storage provider. </summary>
        /// <see cref="IProvider#Init"/>
        public async Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            serviceId = providerRuntime.ServiceId.ToString();
            Log = providerRuntime.GetLogger("StorageProvider.SimpleSQLServerStorage." + serviceId);

            try
            {
                Name = name;
                this.jsonSettings =
                    OrleansJsonSerializer.UpdateSerializerSettings(
                        OrleansJsonSerializer.GetDefaultSerializerSettings(), config);

                if (!config.Properties.ContainsKey(CONNECTION_STRING) ||
                    string.IsNullOrWhiteSpace(config.Properties[CONNECTION_STRING]))
                {
                    throw new BadProviderConfigException($"Specify a value for: {CONNECTION_STRING}");
                }
                var connectionString = config.Properties[CONNECTION_STRING];
                sqlconnBuilder = new SqlConnectionStringBuilder(connectionString);

                //a validation of the connection would be wise to perform here
                var sqlCon = new SqlConnection(sqlconnBuilder.ConnectionString);
                await sqlCon.OpenAsync();
                sqlCon.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary> Shutdown this storage provider. </summary>
        /// <see cref="IStorageProvider#Close"/>
        public async Task Close()
        {
        }

        /// <summary> Read state data function for this storage provider. </summary>
        /// <see cref="IStorageProvider#ReadStateAsync"/>
        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var userId = grainReference.GetPrimaryKeyLong();
            try
            {
                using (var db = new UserDbContext(this.sqlconnBuilder.ConnectionString))
                {
                    var state = await db.Set(grainState.State.GetType()).FindAsync(userId);
                    if (state != null)
                    {
                        grainState.State = state;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary> Write state data function for this storage provider. </summary>
        /// <see cref="IStorageProvider#WriteStateAsync"/>
        public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var userId = grainReference.GetPrimaryKeyLong();
            try
            {
                using (var db = new UserDbContext(this.sqlconnBuilder.ConnectionString))
                {
                    var user = await db.Set(grainState.State.GetType()).FindAsync(userId);
                    if (user == null)
                    {
                        db.Set(grainState.State.GetType()).Add(grainState.State);
                    }
                    else
                    {
                        db.Set(grainState.State.GetType()).Add(grainState.State);
                    }
                    await db.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary> Clear state data function for this storage provider. </summary>
        /// <remarks>
        /// </remarks>
        /// <see cref="IStorageProvider#ClearStateAsync"/>
        public async Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            try
            {
                using (var db = new UserDbContext(this.sqlconnBuilder.ConnectionString))
                {

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

