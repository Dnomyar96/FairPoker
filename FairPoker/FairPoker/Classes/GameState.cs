using FairPoker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPoker.Classes
{
    public class GameState
    {
        public int RequiredBetPerPlayer { get; set; }

        public List<Card> TableCards { get; set; }

        public Dealer Dealer { get; set; }

        public List<Player> Players { get; set; }

        public RoundState RoundState { get; set; }

        public GameState(List<Player> players = null)
        {
            TableCards = new List<Card>();
            Dealer = new Dealer();
            Players = new List<Player>();
            RoundState = RoundState.PreFlop;
            Dealer.NewRound();
            
            for (var i = 0; i < (players == null ? Settings.PlayerCount : players.Count()); i++)
            {
                Player player;

                if (players == null)
                    player = new Player();
                else
                    player = players[i];

                if (i > 0)
                    player.UsesAI = true;
                
                if (player.GetTotalCash() > 0)
                {
                    player.NewRound();
                    Players.Add(player);
                }
            }
        }
    }
}
