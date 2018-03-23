using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Exceptions
{
    public class PlayerFoldedException : Exception
    {
        public PlayerFoldedException() { }

        public PlayerFoldedException(string message) : base(message) { }

        public PlayerFoldedException(string message, Exception inner) : base(message, inner) { }
    }
}
