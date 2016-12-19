using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketProxy.Users.Commands
{
    public class Command<T>
    {
        protected virtual void Execute(T data)
        {

        }

        public void TryExecute(object data)
        {
            Execute((T)data);
        }
    }
}
