using FairPoker.Enums;
using System;
using System.Collections.Generic;
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
        if (sequenceCount == 2 && alreadyMissingCard == true || sequenceCount == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
