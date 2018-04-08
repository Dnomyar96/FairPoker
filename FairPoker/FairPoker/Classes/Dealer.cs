using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public class Dealer
    {
        private Deck deck;

        /// <summary>
        /// Start a new round. The deck is reset.
        /// </summary>
        public void NewRound()
        {
            deck = new Deck();
        }

        /// <summary>
        /// Deal a card to a player
        /// </summary>
        /// <param name="player">The player to deal the card to</param>
        public void DealCard(Player player)
        {
            player.GiveCard(deck.DrawCard());
        }

        /// <summary>
        /// Deal the first three community cards
        /// </summary>
        public IEnumerable<Card> DealFlop()
        {
            var cards = new Card[3];

            for(var i = 0; i < 3; i++)
            {
                cards[i] = deck.DrawCard();
            }

            return cards;
        }

        /// <summary>
        /// Deal the fourth community card
        /// </summary>
        public Card DealTurn()
        {
            return deck.DrawCard();
        }

        /// <summary>
        /// Deal the fifth community card
        /// </summary>
        public Card DealRiver()
        {
            return deck.DrawCard();
        }
    }
}
