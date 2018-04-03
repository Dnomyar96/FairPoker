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

        /// <summary>
        /// Method to check if a user has a change for a straight.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsAlmostStraight(IEnumerable<Card> cards)
        {
            List<Card> straightList = new List<Card>();
            Debug.WriteLine("#########");
            List<Card> sortedList = cards.OrderBy(o => o.Value).ToList();
            Debug.WriteLine("number of cards: " + sortedList.Count());
            int previousCard = 0;
            int sequenceCount = 0;
            bool alreadyMissingCard = false;
            foreach (Card card in sortedList)
            {
                Debug.WriteLine((int)card.Value);
            
                if (previousCard == 0)
                {
                    previousCard = (int)card.Value;
                }
                else
                {
                    if((int)card.Value - previousCard == 1)
                    {
                        sequenceCount++;
                    }
                    else if((int)card.Value - previousCard == 2 && alreadyMissingCard == false)
                    {
                        alreadyMissingCard = true;
                    }
                    else
                    {
                        sequenceCount = 0;
                    }
                    previousCard = (int)card.Value;
                }
            }
            Debug.WriteLine("@@@@@@@");
            Debug.WriteLine(sequenceCount);
            Debug.WriteLine(alreadyMissingCard);
            if (sequenceCount == 2 && alreadyMissingCard == true || sequenceCount == 3)
            {
                Debug.WriteLine("Change is" + true);
                return true;
            }
            else
            {
                Debug.WriteLine("Change is" + false);
                return false;
            }
        }

        /// <summary>
        /// Method to check if a user has a change for a straight.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsAlmostFlush(IEnumerable<Card> cards)
        {             int countSpades = 0;
            int countHearts = 0;
            int countDiamonds = 0;
            int countClovers = 0;
            foreach (Card card in cards)
            {

              if (card.Color == CardColor.Hearts)
                {
                    countHearts++;
                }
                if (card.Color == CardColor.Spades)
                {
                    countSpades++;
                }
                if (card.Color == CardColor.Clovers)
                {
                    countClovers++;
                }
                if (card.Color == CardColor.Diamonds)
                {
                    countDiamonds++;
                }
            }

            Debug.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            Debug.WriteLine(countClovers);
            Debug.WriteLine(countDiamonds);
                     Debug.WriteLine(countHearts);
                     Debug.WriteLine(countSpades);
            Debug.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            if (countClovers >= 4 || countDiamonds >= 4 || countHearts >= 4 || countSpades >= 4)
            {
                return true;
            }
            else
            {
                return false;
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