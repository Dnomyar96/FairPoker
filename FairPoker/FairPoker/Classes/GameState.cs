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
        public int Pot { get; set; }

        public int RequiredBetPerPlayer { get; set; }

        public List<Card> TableCards { get; set; }

        public Dealer Dealer { get; set; }

        public List<Player> Players { get; set; }

        public RoundState RoundState { get; set; }

        public GameState()
        {
            Dealer = new Dealer();
            Players = new List<Player>();
            RoundState = RoundState.PreFlop;
            Dealer.NewRound();
            
            for (var i = 0; i < Settings.PlayerCount; i++)
            {
                var player = new Player();

                if (i > 0)
                    player.UsesAI = true;

                player.NewRound();

                Players.Add(player);
            }
        }
    }
}
