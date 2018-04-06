using FairPoker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public class AIDecisionHandler
    {
        private Player player;
        private double baseWillingness;
        private IDictionary<double, Plays> floors;
        
        public AIDecisionHandler(Player player)
        {
            this.player = player;
            baseWillingness = new Random().NextDouble() * 10 + 8;
            floors = new Dictionary<double, Plays>();
            floors.Add(baseWillingness - 5, Plays.CheckOrFold);
            floors.Add(baseWillingness, Plays.Call);
            floors.Add(baseWillingness + 5, Plays.Raise);
            floors.Add(baseWillingness + 15, Plays.AllIn);
        }

        /// <summary>
        /// Makes a decision based on the current score and chance for a better score.
        /// Returns the play to make. Raising always with the minimum amount.
        /// </summary>
        public Plays MakeDecision()
        {
            var currentScore = player.GetScore();
            var willingness = (WillingnessOnCurrentScore(currentScore) * 3 + WillingnessOnChance(currentScore)) / 4;

            var play = floors.Where(c => c.Key <= willingness).LastOrDefault().Value;
            return play;
        }
        
        private double WillingnessOnCurrentScore(Score currentScore)
        {
            return baseWillingness * ((int)currentScore * 3) / 3;
        }

        private double WillingnessOnChance(Score currentScore)
        {
            var cards = player.GetCards();

            var count = 0;
            double totalWillingness = 0;

            foreach(Score score in Enum.GetValues(typeof(Score)))
            {
                if (score < currentScore)
                    continue;

                totalWillingness += GetChanceForScore(score, cards) * (int)score + baseWillingness;
                count++;
            }

            var willingness = totalWillingness / count;
            
            return willingness < 100 ? willingness : 100;
        }

        private double GetChanceForScore(Score score, IEnumerable<Card> cards)
        {
            switch (score)
            {
                case Score.Pair:
                    return ChanceCalculator.PairChance(cards);
                case Score.TwoPair:
                    return ChanceCalculator.TwoPairChance(cards);
                case Score.ThreeOfKind:
                    return ChanceCalculator.ThreeOfAKindChance(cards);
                case Score.Straight:
                    return ChanceCalculator.StraightChance(cards);
                case Score.Flush:
                    return ChanceCalculator.FlushChance(cards);
                case Score.FullHouse:
                    return ChanceCalculator.FullHouseChance(cards);
                case Score.FourOfKind:
                    return ChanceCalculator.FourOfAKindChance(cards);
                case Score.StraightFlush:
                    return ChanceCalculator.StraightFlushChance(cards);
                case Score.RoyalFlush:
                    return ChanceCalculator.RoyalFlushChance(cards);
            }

            return 0;
        }
    }
}
