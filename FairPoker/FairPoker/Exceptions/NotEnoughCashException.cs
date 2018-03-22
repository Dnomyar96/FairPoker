using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Exceptions
{
    public class NotEnoughCashException : Exception
    {
        public NotEnoughCashException() { }

        public NotEnoughCashException(string message) : base(message) { }

        public NotEnoughCashException(string message, Exception inner) : base(message, inner) { }
    }
}
