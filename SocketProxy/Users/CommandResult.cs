using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketProxy.Users
{
    public class CommandResult
    {
        public CommandResult(State result)
        {
            Result = result;
        }

        public State Result { get; }

        public enum State
        {
            Success,
            NotFound
        }
    }
}
