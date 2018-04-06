using FairPoker.Enums;
using FairPoker.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public class Player
    {
        public bool UsesAI { get; set; }
        public AIDecisionHandler AIDecisionHandler { get; set; }

        private List<Card> hand;
        private int cash;
        private Score score;
        private PlayerState state;
        private int bet;

        public Player()
        {
            hand = new List<Card>();
            cash = 1000;
            score = Score.HighCard;
            state = PlayerState.NotPlayed;
            bet = 100;
            AIDecisionHandler = new AIDecisionHandler(this);
        }

        /// <summary>
        /// Reset the state for a player to start a new round.
        /// </summary>
        /// <param name="playerState">The PlayerState this player needs to get. Used to assign blinds. 
        /// Defaults to NotPlayed</param>
        public void NewRound(PlayerState playerState = PlayerState.NotPlayed)
        {
            hand = new List<Card>();
            score = Score.HighCard;
            state = playerState;
        }

        /// <summary>
        /// Add a card to the hand of this player.
        /// </summary>
        /// <param name="card">Card to add to the hand</param>
        public void GiveCard(Card card)
        {
            hand.Add(card);
        }

        /// <summary>
        /// Returns the current hand of this player.
        /// </summary>
        public IEnumerable<Card> GetCards()
        {
            return hand;
        }

        /// <summary>
        /// Calculates the score for this player
        /// </summary>
        /// <param name="cards">The community cards</param>
        public void CalculateScore(List<Card> cards)
        {
            score = Scoring.GetScore(hand.Concat(cards));
        }

        /// <summary>
        /// Returns the current score for this player.
        /// </summary>
        public Score GetScore()
        {
            return score;
        }

        public PlayerState GetPlayerState()
        {
            return state;
        }

        /// <summary>
        /// Player checks
        /// </summary>
        /// <exception cref="PlayerFoldedException"/>
        public void Check()
        {
            if (state == PlayerState.Folded)
                throw new PlayerFoldedException();

            state = PlayerState.Checked;
        }

        /// <summary>
        /// Player calls.
        /// </summary>
        /// <param name="amount">The amount the player adds to the pot</param>
        /// <exception cref="NotEnoughCashException"/>
        /// <exception cref="PlayerFoldedException"/>
        public void Call(int amount)
        {
            if (state == PlayerState.Folded)
                throw new PlayerFoldedException();

            if (cash < amount)
                throw new NotEnoughCashException();

            cash -= amount;
            bet += amount;
            state = PlayerState.Called;
        }

        /// <summary>
        /// Player bets.
        /// </summary>
        /// <param name="amount">The amount the player adds to the pot</param>
        /// <exception cref="NotEnoughCashException"/>
        /// <exception cref="PlayerFoldedException"/>
        public void Bet(int amount)
        {
            if (state == PlayerState.Folded)
                throw new PlayerFoldedException();

            if (cash < amount)
                throw new NotEnoughCashException();

            cash -= amount;
            state = PlayerState.Bet;
        }

        /// <summary>
        /// Player raises.
        /// </summary>
        /// <param name="amount">The amount the player adds to the pot</param>
        /// <exception cref="NotEnoughCashException"/>
        /// <exception cref="PlayerFoldedException"/>
        public void Raise(int amount)
        {
            if (state == PlayerState.Folded)
                throw new PlayerFoldedException();

            if (cash < amount)
                throw new NotEnoughCashException();

            cash -= amount;
            bet += amount;
            state = PlayerState.Raised;
        }

        /// <summary>
        /// Player raises. Returns amount that is bet this way.
        /// </summary>
        /// <exception cref="PlayerFoldedException"/>
        public int AllIn()
        {
            if (state == PlayerState.Folded)
                throw new PlayerFoldedException();

            var amountBet = cash;
            cash = 0;
            state = PlayerState.Raised;
            return amountBet;
        }

        /// <summary>
        /// Player folds.
        /// </summary>
        public void Fold()
        {
            state = PlayerState.Folded;
        }
        
        public int GetPlayerBet()
        {
            return bet;
        }

        public async Task Turn()
        {
            AIDecisionHandler aIDecisionHandler = new AIDecisionHandler(this);
            Debug.WriteLine(AIDecisionHandler.MakeDecision());
        }
    }
}
