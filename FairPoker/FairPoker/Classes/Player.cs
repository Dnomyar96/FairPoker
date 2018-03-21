using FairPoker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public class Player
    {
        private List<Card> hand;
        private int cash;
        private Score score;

        public Player()
        {
            hand = new List<Card>();
            cash = 1000;
            score = Score.HighCard;
        }

        public void GiveCard(Card card)
        {
            hand.Add(card);
        }

        public IEnumerable<Card> GetCards()
        {
            return hand;
        }

        public void CalculateScore(List<Card> cards)
        {
            score = Scoring.GetScore(hand.Concat(cards));
        }

        public Score GetScore()
        {
            return score;
        }

        public void Check()
        {
            throw new NotImplementedException();
        }

        public void Bet()
        {
            throw new NotImplementedException();
        }

        public void Raise()
        {
            throw new NotImplementedException();
        }

        public void Fold()
        {
            throw new NotImplementedException();
        }
    }
}
