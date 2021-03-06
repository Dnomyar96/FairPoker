﻿using FairPoker.Enums;
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
        public int BettingCash { get; internal set; }

        private List<Card> hand;
        private int cash;
        private Score score;
        private PlayerState state;

        const int defaultBet = 100;

        public Player()
        {
            cash = 1000;
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
            BettingCash = 0;
            Bet(defaultBet);
        }

        /// <summary>
        /// Add a card to the hand of this player.
        /// </summary>
        /// <param name="card">Card to add to the hand</param>
        public void GiveCard(Card card)
        {
            hand.Add(card);
        }

        public void GiveCash(int cash)
        {
            this.cash += cash;
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
        public void Call(int requiredBet)
        {
            if (state == PlayerState.Folded)
                throw new PlayerFoldedException();

            var amount = requiredBet - BettingCash;

            if (cash < amount)
                throw new NotEnoughCashException();

            cash -= amount;
            BettingCash += amount;
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
            BettingCash += amount;
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
            BettingCash += amount;
            state = PlayerState.Raised;
        }

  

        /// <summary>
        /// Player raises. Returns amount that is bet this way.
        /// </summary>
        /// <exception cref="PlayerFoldedException"/>
        public void AllIn()
        {
            if (state == PlayerState.Folded)
                throw new PlayerFoldedException();

            BettingCash += cash;
            cash = 0;
            state = PlayerState.AllIn;
        }

        /// <summary>
        /// Player folds.
        /// </summary>
        public void Fold()
        {
            state = PlayerState.Folded;
        }
        
        public PlayerState GetStatus()
        {
            return state;
        }
        public int GetTotalCash()
        {
            return cash;
        }
        
        public async Task Turn(int amountRequiredToBet)
        {
            if (state == PlayerState.AllIn)
                return;

            AIDecisionHandler aIDecisionHandler = new AIDecisionHandler(this);
            Debug.WriteLine(AIDecisionHandler.MakeDecision());
            switch(AIDecisionHandler.MakeDecision())
            {
                case Plays.AllIn:
                    AllIn();
                    break;
                case Plays.Call:                  
                    int amount = amountRequiredToBet - BettingCash;

                    if (amount <= 0)
                    {
                        Check();
                        break;
                    }

                    Call(amount);
                    break;
                case Plays.CheckOrFold:
                    if(amountRequiredToBet > BettingCash)
                    {
                        Fold();
                        break;
                    }

                    Check();
                    break;
                case Plays.Raise:
                    if(cash <= 100)
                    {
                        AllIn();
                        break;
                    }

                    Raise(100);
                    break;
            }
        }
    }
}
