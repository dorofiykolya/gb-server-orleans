using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace Grains
{
    public interface IUserInfoGrain : IGrainWithIntegerKey
    {
        Task<string> GetName();
        Task<string> ChangeName(string name);
    }
}
