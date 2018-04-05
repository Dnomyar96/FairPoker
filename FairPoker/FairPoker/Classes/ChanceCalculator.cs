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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
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

        public static double TwoPairChance(IEnumerable<Card> cards)
        {
            if (Scoring.IsTwoPair(cards))
            {
                return 100;
            }
            if (Scoring.IsPair(cards))
            {
                var TwoPairChances = cards.Count() - 2;
                return CardChance(TwoPairChances  * 3,cards);
            }
            else
            {
                return 0;
            }
        }

        public static double ThreeOfAKindChance(IEnumerable<Card> cards)
        {
            if(Scoring.IsThreeOfKind(cards)){
                return 100;
            }

            if (Scoring.IsTwoPair(cards))
            {
                return 0;
            }
            if (Scoring.IsPair(cards))
            {
                return CardChance(2, cards);
            }
            else
            {
                return 0;
            }
        }

        public static double StraightChance(IEnumerable<Card> cards)
        {
            if (AlmostCalculator.IsAlmostStraight(cards, out int possibleCount))
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
                        else if ((int)card.Value - previousCard == 2 && !alreadyMissingCard)
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
                if (alreadyMissingCard == true)
                {
                    return CardChance(4, cards);
                }
                else
                {
                    return CardChance(8, cards);
                }
            }
            else
            {
                return 0;
            }

        }

        public static double FlushChance(IEnumerable<Card> cards)
        {
            if (Scoring.IsFlush(cards))
            {
                return 100;
            }
            if (AlmostCalculator.IsAlmostFlush(cards))
            {
                return CardChance(9, cards);
            }
            return 0;
        }

        public static double CardChance(int count, IEnumerable<Card> cards)
        {
            var knownCardCount = Convert.ToDouble(cards.Count());
            var unknownCardCount = Convert.ToDouble(cardsInDeck.Count() - knownCardCount);
            Debug.WriteLine(unknownCardCount);
            Debug.WriteLine(count / unknownCardCount * 100);
            return count / unknownCardCount * 100;
        } 

        public static double FullHouseChance(IEnumerable<Card> cards)
        {
            if (Scoring.IsFullHouse(cards))
            {
                return 100;
            }
            if (Scoring.IsTwoPair(cards))
            {
                return CardChance(4, cards);
            }
            if (Scoring.IsThreeOfKind(cards))
            {
                var possibleCards = cards.Count() - 3;
                return CardChance(possibleCards * 3, cards);
            }
            return 0;
        }

        public static double FourOfAKindChance(IEnumerable<Card> cards)
        {
            if (Scoring.IsFourOfKind(cards))
            {
                return 100;
            }
            if (Scoring.IsThreeOfKind(cards))
            {
                return CardChance(1, cards);
            }
            return 0;
        }
        
        public static double StraightFlushChance(IEnumerable<Card> cards)
        {
            if(AlmostCalculator.IsAlmostStraightFlush(cards,out int possibleCount))
            {
                return CardChance(possibleCount,cards);
            }
            else
            {
                return 0;
            }
        }

        public static double RoyalFlushChance(IEnumerable<Card> cards)
        {
            if (AlmostCalculator.IsAlmostRoyalFlush(cards))
            {
                return CardChance(1, cards);
            }
            return 0;
        }
    }
}