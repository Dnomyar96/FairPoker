using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Enums
{
    public enum PlayerState
    {
        NotPlayed,
        Checked,
        Folded,
        Called,
        Bet,
        Raised,
        SmallBlind,
        BigBlind,
    }
}
