using FairPoker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public class Deck
    {
        private IEnumerable<Card> cards;

        public Deck()
        {
            var cards = new List<Card>();

            foreach(CardColor color in Enum.GetValues(typeof(CardColor)))
            {
                foreach(CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    cards.Add(new Card
                    {
                        Color = color,
                        Value = value,
                        ImgUrl = ""
                    });
                }
            }

            this.cards = cards;
        }
    }
}
