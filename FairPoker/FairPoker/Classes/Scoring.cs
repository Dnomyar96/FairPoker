using FairPoker.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public static class Scoring
    {
        /// <summary>
        /// Gets the score for a set of cards
        /// </summary>
        public static Score GetScore(IEnumerable<Card> cards)
        {
            if (IsRoyalFlush(cards))
                return Score.RoyalFlush;

            if (IsStraightFlush(cards))
                return Score.StraightFlush;

            if (IsFourOfKind(cards))
                return Score.FourOfKind;

            if (IsFullHouse(cards))
                return Score.FullHouse;

            if (IsFlush(cards))
                return Score.Flush;

            if (IsStraight(cards))
                return Score.Straight;

            if (IsThreeOfKind(cards))
                return Score.ThreeOfKind;

            if (IsTwoPair(cards))
                return Score.TwoPair;

            if (IsPair(cards))
                return Score.Pair;
           
            return Score.HighCard;
        }

        /// <summary>
        /// Checks whether a set of cards has a royal flush
        /// </summary>
        public static bool IsRoyalFlush(IEnumerable<Card> cards)
        {
            var possibleCards = cards.Where(c => (int)c.Value >= 10);

            if (possibleCards.GroupBy(c => c.Color).Any(s => s.Count() == 5))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether a set of cards has a straight flush
        /// </summary>
        public static bool IsStraightFlush(IEnumerable<Card> cards)
        {
            var orderedCards = cards.OrderBy(c => c.Value).OrderBy(c => c.Color);

            var count = 1;
            Card previousCard = null;

            foreach (var card in orderedCards)
            {
                if (previousCard != null)
                {
                    if (card.Value - 1 == previousCard.Value && card.Color == previousCard.Color)
                        count++;
                    else
                        count = 1;
                }

                if (count >= 5)
                    return true;

                previousCard = card;
            }

            return false;
        }

        /// <summary>
        /// Checks whether a set of cards has a four of a kind
        /// </summary>
        public static bool IsFourOfKind(IEnumerable<Card> cards)
        {
            var sets = cards.GroupBy(c => c.Value);

            if (sets.Any(s => s.Count() > 3))
                return true;

            return false;
        }

        /// <summary>
        /// Checks whether a set of cards has a full house
        /// </summary>
        public static bool IsFullHouse(IEnumerable<Card> cards)
        {
            var groupedCards = cards.GroupBy(c => c.Value);

            if (groupedCards.Any(c => c.Count() == 3) && groupedCards.Any(c => c.Count() == 2))
                return true;

            return false;
        }

        /// <summary>
        /// Checks whether a set of cards has a flush
        /// </summary>
        public static bool IsFlush(IEnumerable<Card> cards)
        {
            if (cards.GroupBy(c => c.Color).Any(c => c.Count() == 5))
                return true;

            return false;
        }

        /// <summary>
        /// Checks whether a set of cards has a straight
        /// </summary>
        public static bool IsStraight(IEnumerable<Card> cards)
        {
            var orderedCards = cards.OrderBy(c => c.Value);

            var count = 1;
            Card previousCard = null;

            foreach (var card in orderedCards)
            {
                if (previousCard != null)
                {
                    if (card.Value - 1 == previousCard.Value)
                        count++;
                    else
                        count = 1;
                }

                if (count >= 5)
                    return true;

                previousCard = card;
            }

            return false;
        }

        /// <summary>
        /// Checks whether a set of cards has a three of a kind
        /// </summary>
        public static bool IsThreeOfKind(IEnumerable<Card> cards)
        {
            var sets = cards.GroupBy(c => c.Value);

            if (sets.Any(s => s.Count() > 2))
                return true;

            return false;
        }

        /// <summary>
        /// Checks whether a set of cards has two pairs
        /// </summary>
        public static bool IsTwoPair(IEnumerable<Card> cards)
        {
            var sets = cards.GroupBy(c => c.Value);

            if (sets.Where(s => s.Count() > 1).Count() > 1)
                return true;


            return false;
        }

        /// <summary>
        /// Checks whether a set of cards has a pair
        /// </summary>
        public static bool IsPair(IEnumerable<Card> cards)
        {
            var sets = cards.GroupBy(c => c.Value);

            if (sets.Any(s => s.Count() > 1))
                return true;

            return false;
        }
    }
}
