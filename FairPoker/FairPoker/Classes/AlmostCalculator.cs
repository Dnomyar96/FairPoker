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
            var groupedCards = cards.GroupBy(c => c.Color);

            if(groupedCards.Any(c => c.Count() == 4))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to check if a user has a change for a straight.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsAlmostStraight(IEnumerable<Card> cards, out int possibleCount)
        {
            List<Card> straightList = new List<Card>();
            List<Card> sortedList = cards.OrderBy(o => o.Value).ToList();
            int previousCard = 0;
            possibleCount = 0;
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
                    else if ((int)card.Value - previousCard == 2 && !alreadyMissingCard)
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
            if (sequenceCount == 2 && alreadyMissingCard)
            {
                possibleCount = 4;
                return true;
            }
            else if (sequenceCount == 3)
            {
                possibleCount = 8;
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