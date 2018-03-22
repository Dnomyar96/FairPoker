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
        private PlayerState state;

        public Player()
        {
            hand = new List<Card>();
            cash = 1000;
            score = Score.HighCard;
            state = PlayerState.NotPlayed;
        }

        public void NewRound(PlayerState playerState = PlayerState.NotPlayed)
        {
            hand = new List<Card>();
            score = Score.HighCard;
            state = playerState;
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

        public void Bet(int amount)
        {
            throw new NotImplementedException();
        }

        public void Raise(int amount)
        {
            throw new NotImplementedException();
        }

        public void Fold()
        {
            throw new NotImplementedException();
        }
    }
}
