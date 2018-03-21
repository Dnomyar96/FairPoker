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

        public Dealer()
        {
            deck = new Deck();
        }

        public void DealCard(Player player)
        {
            player.GiveCard(deck.DrawCard());
        }

        public IEnumerable<Card> DealFlop()
        {
            var cards = new Card[3];

            for(var i = 0; i < 3; i++)
            {
                cards[i] = deck.DrawCard();
            }

            return cards;
        }

        public Card DealTurn()
        {
            return deck.DrawCard();
        }

        public Card DealRiver()
        {
            return deck.DrawCard();
        }
    }
}
