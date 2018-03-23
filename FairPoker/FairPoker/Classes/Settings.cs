using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public static class Settings
    {
        public static int PlayerCount { get; set; }

        public static bool HideOtherPlayersCards { get; set; }
        public static bool HideChances { get; set; }
        public static bool SoundEffects { get; set; }


        static Settings()
        {
            PlayerCount = 4;
            HideOtherPlayersCards = true;
            HideChances = true;
            SoundEffects = true;
        }
    }
}
