using FairPoker.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public static class AlmostCalculator
    {
        /// <summary>
        /// Method to check if a user has a change for a straight.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsAlmostFlush(IEnumerable<Card> cards)
        {
            int countSpades = 0;
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
            if (countClovers >= 4 || countDiamonds >= 4 || countHearts >= 4 || countSpades >= 4)
            {
                return true;
            }
            else
            {
                return false;
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
            List<Card> sortedList = cards.OrderBy(o => o.Value).ToList();
            int previousCard = 0;
            int sequenceCount = 0;
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
                        sequenceCount++;
                    }
                    else if ((int)card.Value - previousCard == 2 && alreadyMissingCard == false)
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
            if (sequenceCount == 2 && alreadyMissingCard == true)
            {
                return true;
            }
            else if (sequenceCount == 3)
            {
                Debug.WriteLine("TRUEEEE");
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method to check if a user has a change for royal flush
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsAlmostRoyalFlush(IEnumerable<Card> cards)
        {
            if (Scoring.IsFlush(cards))
            {
                int count = 0;
                foreach (Card card in cards)
                {
                    if ((int)card.Value > 9)
                    {
                        count++;
                    }
                }
                if (count >= 4)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}