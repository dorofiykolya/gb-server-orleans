using System.Threading.Tasks;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;

namespace StorageProvider
{
    public class JSONFileStorageProvider : IStorageProvider
    {
        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;
            Path = config.Properties["Path"];
            Log = providerRuntime.GetLogger(GetType().FullName);
            DataManager = new FileDataManager(Path);
            return TaskDone.Done;
        }

        public Task Close()
        {
            DataManager.Dispose();
            return TaskDone.Done;
        }

        internal FileDataManager DataManager { get; private set; }
        public string Path { get; private set; }
        public string Name { get; private set; }

        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var data = await DataManager.Read(grainState.GetType().Name, grainReference.ToKeyString());
            ConvertFromStorageFormat(grainState, data);
        }

        public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var data = ConvertToStorageFormat(grainState);
            return DataManager.Write(grainState.GetType().Name, grainReference.ToKeyString(), data);
        }

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            DataManager.Delete(grainState.GetType().Name, grainReference.ToKeyString());
            return TaskDone.Done;
        }

        public Logger Log { get; private set; }

        protected static string ConvertToStorageFormat(IGrainState grainState)
        {
            return JsonConvert.SerializeObject(grainState.State);
        }

        protected static void ConvertFromStorageFormat(IGrainState grainState, string entityData)
        {
            grainState.State = JsonConvert.DeserializeObject(entityData, grainState.State.GetType()); ;
        }
    }
}
