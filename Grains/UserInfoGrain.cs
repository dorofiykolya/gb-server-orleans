using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;

namespace Grains
{
    [StorageProvider(ProviderName = "UserProvider")]
    public class UserInfoGrain : Grain<UserInfoState>, IUserInfoGrain
    {
        public override Task OnActivateAsync()
        {
            State.UserId = this.GetPrimaryKeyLong();
            return base.OnActivateAsync();
        }

        public Task<string> GetName()
        {
            return Task.FromResult(State.Name);
        }

        public Task<string> ChangeName(string name)
        {
            AssertValidName(name);
            State.Name = name;
            WriteStateAsync();
            return Task.FromResult(name);
        }

        private void AssertValidName(string name)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(name), "name can not bee null or empty");
        }
    }
}
