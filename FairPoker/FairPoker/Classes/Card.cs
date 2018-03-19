using FairPoker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public class Card
    {
        public CardColor Color { get; set; }

        public CardValue Value { get; set; }

        public string ImgUrl { get; set; }
    }
}
