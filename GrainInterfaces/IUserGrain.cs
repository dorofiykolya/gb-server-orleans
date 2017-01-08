using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    public interface IUserGrain : Orleans.IGrain
    {
        Task InitUser();
    }
}
