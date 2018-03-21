using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Exceptions
{
    public class DeckEmptyException : Exception
    {
        public DeckEmptyException() { }

        public DeckEmptyException(string message) : base(message) { }

        public DeckEmptyException(string message, Exception inner) : base(message, inner) { }
    }
}
