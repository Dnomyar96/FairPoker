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

        public static double CardChange(int count, IEnumerable<Card> cards)
        {
            var knownCardCount = Convert.ToDouble(cards.Count());
            var unknownCardCount = Convert.ToDouble(cardsInDeck.Count() - knownCardCount);
            Debug.WriteLine(unknownCardCount);
            Debug.WriteLine(count / unknownCardCount * 100);
            return count / unknownCardCount * 100;

        }

        public static void StraightChange(IEnumerable<Card> cards)
        {
            var sortedList = cards.OrderBy(o => o.Value).ToList();
            var possibleList = new List<Card>();
            var straightList = new List<Card>();
            var previousCard = 0;
            var missingCard = new Card();
            bool alreadyMissingCard = false;
            foreach (Card card in sortedList)
            {
                if (previousCard == 0)
                {
                    previousCard = (int)card.Value;
                }
                else
                {
                    if ((int)card.Value - previousCard == 1)
                    {
                        straightList.Add(card);
                    }
                    else if ((int)card.Value - previousCard == 2 && alreadyMissingCard == false)
                    {
                        alreadyMissingCard = true;
                        missingCard = card;
                    }
                    else
                    {
                        straightList = new List<Card>();
                    }
                    previousCard = (int)card.Value;
                }
            }
            if(alreadyMissingCard == true)
            {
                CardChange(4,cards);
            }
            else
            {
                CardChange(8, cards);
            }
        }
    }
}