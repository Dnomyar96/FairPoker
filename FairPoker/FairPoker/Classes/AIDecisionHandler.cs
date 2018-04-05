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
        private IDictionary<int, Plays> floors;
        
        public AIDecisionHandler(Player player)
        {
            this.player = player;
            baseWillingness = new Random().NextDouble() * 10 + 8;
        }

        public void MakeDecision()
        {
            var currentScore = player.GetScore();
            var willingness = (WillingnessOnCurrentScore(currentScore) * 3 + WillingnessOnChance(currentScore)) / 4;

            //TODO: Get play
        }
        
        private double WillingnessOnCurrentScore(Score currentScore)
        {
            return baseWillingness * ((int)currentScore * 3) / 3;
        }

        private double WillingnessOnChance(Score currentScore)
        {
            var chanceForHigherWillingness = 5.4; //TODO: implement chance thingy

            var willingness = 100 / baseWillingness * (int)chanceForHigherWillingness;
            return willingness < 100 ? willingness : 100;
        }
    }
}
