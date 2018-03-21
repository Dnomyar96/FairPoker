using FairPoker.Enums;
using FairPoker.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public class Deck
    {
        private List<Card> cards;

        /// <summary>
        /// Creates a new instance of <see cref="Deck"/> with a list of cards based on the 
        /// <see cref="CardColor"/> and <see cref="CardValue"/> enums.
        /// </summary>
        public Deck()
        {
            var cards = new List<Card>();

            foreach(CardColor color in Enum.GetValues(typeof(CardColor)))
            {
                foreach(CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    cards.Add(new Card
                    {
                        Color = color,
                        Value = value,
                        ImgUrl = ""
                    });
                }
            }

            this.cards = Shuffle(cards);
        }

        /// <summary>
        /// Shuffles a list of cards.
        /// Code from StackOverflow: https://stackoverflow.com/a/1262619/7894052
        /// </summary>
        /// <param name="deck">The list to shuffle</param>
        /// <returns>The shuffled list</returns>
        private List<Card> Shuffle(List<Card> deck)
        {
            var n = deck.Count;
            var random = new Random();

            while(n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                var card = deck[k];
                deck[k] = deck[n];
                deck[n] = card;
            }

            return deck;
        }

        /// <summary>
        /// Draw the first card from the deck. The card gets removed from the deck.
        /// </summary>
        /// <returns>The first card from the deck.</returns>
        /// <exception cref="DeckEmptyException">Throws when the deck is empty</exception>
        public Card DrawCard()
        {
            if(cards.Count() < 1 || cards == null)
                throw new DeckEmptyException();

            var card = cards.Take(1).First();
            cards.Remove(card);

            return card;
        }
    }
}
