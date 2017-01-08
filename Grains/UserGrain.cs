using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace Grains
{
    public class UserGrain : Grain, IUserGrain
    {
        

        public Task InitUser()
        {
            return TaskDone.Done;
        }

        
    }
}
