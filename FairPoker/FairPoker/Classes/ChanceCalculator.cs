using FairPoker.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public static class ChanceCalculator
    {
        private static List<Card> cardsInDeck;
        private static int cardsPerColor;

        static ChanceCalculator()
        {
            cardsPerColor = Enum.GetValues(typeof(CardColor)).Length;

            var cards = new List<Card>();

            foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    cards.Add(new Card
                    {
                        Color = color,
                        Value = value,
                        ImgUrl = "ms-appx:///Assets/Cards/" + (value.ToString() + color.ToString()) + ".png"
                    });
                }
            }

            cardsInDeck = cards;
        }

        public static double PairChance(IEnumerable<Card> cards)
        {
            if (!Scoring.IsPair(cards))
            {
                var knownCardCount = Convert.ToDouble(cards.Count());
                var unknownCardCount = Convert.ToDouble(cardsInDeck.Count() - knownCardCount);

                var chancePerCard = 3 / unknownCardCount * 100;
                return chancePerCard * knownCardCount;
            }
            else
            {
                return 100;
            }
        }

        //public static double FlushChance(IEnumerable<Card> cards)
        //{
        //    if (!Scoring.IsFlush(cards))
        //    {

        //    }
        //    else
        //    {
        //        return 100;
        //    }
        //}
    }
}