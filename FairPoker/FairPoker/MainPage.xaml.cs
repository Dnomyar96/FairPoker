﻿using System;
using System.Collections.Generic;
using System.Linq;
using FairPoker.Classes;
using FairPoker.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Playback;
using System.Threading;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FairPoker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        // GameState
        private RoundState gameState = RoundState.PreFlop;

        // Table Cards
        List<Card> tableCards;
        Deck Cards = new Deck();
        private Card card1;
        private Card card2;
        private Card card3;
        private Card card4;
        private Card card5;

        //Dealer and Players
        private Dealer dealer;
        private int playerCount = Settings.PlayerCount;
        private List<Player> players;

        //Sounds
        MediaPlayer Audio;

        public MainPage()
        {

            this.InitializeComponent();
            Audio = new MediaPlayer();
            /// Required Set After Initialisation
            SetDefaultValues();

            dealer = new Dealer();
            players = new List<Player>();

            for (var i = 0; i < playerCount; i++)
            {
                var player = new Player();

                if (i > 0)
                    player.UsesAI = true;

                players.Add(player);
            }
            DealCards();
            SetScores();
            SetChance();
            DoAIMoves();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Check Logic
            //Similar to a call but no money is bet. If there is no raise preflop, the big blind may check.
            players[0].Check();
        }
        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Fold Logic
            //Pay nothing to the pot and throw away their hand, waiting for the next deal to play again.
            players[0].Fold();
        }
        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Call Logic
            //Match the amount of the big blind.
            int amount = 0;
            foreach (var player in players)
            {
                if (amount < player.GetPlayerBet())
                {
                    amount = player.GetPlayerBet();
                }
            }

            if(amount <= 0)
            {
                //there is nothing to call....
            }
            else
            {
                players[0].Call(amount);
            }
            
        }
        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Raise Logic
            //Raise the bet by doubling the amount of the big blind. A player may raise more depending on the betting style being played.
            int amount = 0;
            players[0].Raise(amount);
            PlayAudio("chips.wav");
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewRound_Click(object sender, RoutedEventArgs e)
        {
            ResetChanceLabels();

            card1 = null;
            card2 = null;
            card3 = null;
            card4 = null;
            card5 = null;

            gameState = RoundState.PreFlop;

            SetDefaultValues();          
            dealer.NewRound();

            foreach (var player in players)
            {
                player.NewRound();
            }


            DealCards();
            SetScores();
            SetChance();
            DoAIMoves();
        }

        private void ResetChanceLabels()
        {
            PairChance.Text = "Pair chance not available";
            TwoPairChance.Text = "Two pair chance not available";
            ThreeOfAKindChance.Text = "Three of a kind chance not available";
            StraightChance.Text = "Straight chance not available";
            FlushChance.Text = "Flush chance not available";
            FullHouseChance.Text = "Full House chance not available";
            FourOfKindChance.Text = "Four of a kind chance not available";
            StraightFlushChance.Text = "Straight flush chance not available";
            RoyalFlushChance.Text = "Royal flush chance not available";
        }

        private void SetDefaultValues()
        {
            CardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage3.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage4.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage5.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));

            PlayerOneCardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerOneCardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerOneTextHand.Text = "";

            PlayerTwoCardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerTwoCardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerTwoTextHand.Text = "";

            PlayerThreeCardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerThreeCardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerThreeTextHand.Text = "";

            PlayerFourCardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerFourCardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerFourTextHand.Text = "";

            PlayerFiveCardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerFiveCardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerFiveTextHand.Text = "";

            PlayerSixCardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerSixCardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            PlayerSixTextHand.Text = "";

            if (!Settings.HideChances)
            {
                ChanceList.Visibility = Visibility.Visible;
            }
        }

        private void DealCards()
        {
            for (var i = 0; i < 2; i++)
            {
                foreach (var player in players)
                {
                    dealer.DealCard(player);
                }
            }

            var playerOneCards = players[0].GetCards().ToArray();
            PlayerOneCardImage1.Source = new BitmapImage(new Uri(playerOneCards[0].ImgUrl));
            PlayerOneCardImage2.Source = new BitmapImage(new Uri(playerOneCards[1].ImgUrl));

            if (playerCount > 1)
            {
                var playerTwoCards = players[1].GetCards().ToArray();
                GridP2.Visibility = Visibility.Visible;
                if (Settings.HideOtherPlayersCards == false)
                {
                    PlayerTwoCardImage1.Source = new BitmapImage(new Uri(playerTwoCards[0].ImgUrl));
                    PlayerTwoCardImage2.Source = new BitmapImage(new Uri(playerTwoCards[1].ImgUrl));
                }
            }

            if (playerCount > 2)
            {

                var playerThreeCards = players[2].GetCards().ToArray();
                GridP3.Visibility = Visibility.Visible;
                if (Settings.HideOtherPlayersCards == false)
                {
                    PlayerThreeCardImage1.Source = new BitmapImage(new Uri(playerThreeCards[0].ImgUrl));
                    PlayerThreeCardImage2.Source = new BitmapImage(new Uri(playerThreeCards[1].ImgUrl));
                }
            }

            if (playerCount > 3)
            {
                var playerFourCards = players[3].GetCards().ToArray();
                GridP4.Visibility = Visibility.Visible;
                if (Settings.HideOtherPlayersCards == false)
                {
                    PlayerFourCardImage1.Source = new BitmapImage(new Uri(playerFourCards[0].ImgUrl));
                    PlayerFourCardImage2.Source = new BitmapImage(new Uri(playerFourCards[1].ImgUrl));
                }
            }

            if (playerCount > 4)
            {
                var playerFiveCards = players[4].GetCards().ToArray();
                GridP5.Visibility = Visibility.Visible;
                if (Settings.HideOtherPlayersCards == false)
                {
                    {
                        PlayerFiveCardImage1.Source = new BitmapImage(new Uri(playerFiveCards[0].ImgUrl));
                        PlayerFiveCardImage2.Source = new BitmapImage(new Uri(playerFiveCards[1].ImgUrl));
                    }
                }
                if (playerCount > 5)
                {
                    var playerSixCards = players[5].GetCards().ToArray();
                    GridP6.Visibility = Visibility.Visible;
                    if (Settings.HideOtherPlayersCards == false)
                    {
                        PlayerSixCardImage1.Source = new BitmapImage(new Uri(playerSixCards[0].ImgUrl));
                        PlayerSixCardImage2.Source = new BitmapImage(new Uri(playerSixCards[1].ImgUrl));
                    }
                }
                SetScores();
                SetChance();
                PlayAsync();
                DoAIMoves();
            }
        }



        private void CardClick(object sender, RoutedEventArgs e)
        {
            TurnCard();
            SetScores();
            SetChance();
            PlayAsync();
            DoAIMoves();
        }

        private void SetScores()
        {

       

            tableCards = new List<Card>()
                {

                    card1,
                    card2,
                    card3,
                    card4,
                    card5
                }.Where(c => c != null).ToList();

            foreach (Player player in players)
            {
                Task t = new Task(() => player.CalculateScore(tableCards));
                t.Start();
                player.CalculateScore(tableCards);
            }

            PlayerOneTextHand.Text = players[0].GetScore().ToString();

            if ((playerCount > 1) && (Settings.HideOtherPlayersCards == false))
            {
                PlayerTwoTextHand.Text = players[1].GetScore().ToString();
            }
            if ((playerCount > 2) && (Settings.HideOtherPlayersCards == false))
            {
                PlayerThreeTextHand.Text = players[2].GetScore().ToString();
            }
            if ((playerCount > 3) && (Settings.HideOtherPlayersCards == false))
            {
                PlayerFourTextHand.Text = players[3].GetScore().ToString();
            }
            if ((playerCount > 4) && (Settings.HideOtherPlayersCards == false))
            {
                PlayerFiveTextHand.Text = players[4].GetScore().ToString();
            }
            if ((playerCount > 5) && (Settings.HideOtherPlayersCards == false))
            {
                PlayerSixTextHand.Text = players[5].GetScore().ToString();
            }
        }

        private void SetChance()
        {
            var player = players.FirstOrDefault();
            var cards = player.GetCards().Concat(new List<Card>()
                {
                    card1,
                    card2,
                    card3,
                    card4,
                    card5
                }.Where(c => c != null));
            if (cards.Count() < 7)
            {
                var straightChance = ChanceCalculator.StraightChance(cards);
                if (straightChance > 0)
                {
                    StraightChance.Text = straightChance.ToString("Straight: 0.##") + "%";
                }
                var pairChance = ChanceCalculator.PairChance(cards);
                if (pairChance > 0)
                {
                    PairChance.Text = pairChance.ToString("Pair: 0.##") + "%";
                }
                var twoPairChance = ChanceCalculator.TwoPairChance(cards);
                if (twoPairChance > 0)
                {
                    TwoPairChance.Text = twoPairChance.ToString("Two Pair: 0.##") + "%";
                }
                var threeOfAKindChance = ChanceCalculator.ThreeOfAKindChance(cards);
                if (threeOfAKindChance > 0)
                {
                    ThreeOfAKindChance.Text = threeOfAKindChance.ToString("Three of a kind: 0.##") + "%";
                }
                var flushChance = ChanceCalculator.FlushChance(cards);
                if (flushChance > 0)
                {
                    FlushChance.Text = flushChance.ToString("Flush: 0.##") + "%";
                }
                var fullHouseChance = ChanceCalculator.FullHouseChance(cards);
                if (fullHouseChance > 0)
                {
                    FullHouseChance.Text = fullHouseChance.ToString("Full House: 0.##") + "%";
                }

                var fourOfAKindChance = ChanceCalculator.FourOfAKindChance(cards);
                if (fourOfAKindChance > 0)
                {
                    FourOfKindChance.Text = fourOfAKindChance.ToString("Four of a kind: 0.##") + "%";
                }
                var straightFlushChance = ChanceCalculator.StraightFlushChance(cards);
                if (straightFlushChance > 0)
                {
                    StraightFlushChance.Text = straightFlushChance.ToString("Straight flush: 0.##") + "%";
                }
                var royalFlushChance = ChanceCalculator.RoyalFlushChance(cards);
                if (royalFlushChance > 0)
                {
                    RoyalFlushChance.Text = royalFlushChance.ToString("Royal flush: 0.##") + "%";
                }
            }
        }

        public async Task PlayAsync()
        {
            foreach(var player in players)
            {
                var t = new Task(() => player.GetScore());
                t.Start();
            }
            foreach (var player in players)
            {
                await player.Turn();
            }
        }

        private void DoAIMoves()
        {

            foreach(var player in players.Where(p => p.UsesAI))
            {
                var play = player.AIDecisionHandler.MakeDecision();
            }
        }

        private async void PlayAudio(string filename)
        {
            if (Settings.SoundEffects)
            {
                Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
                Windows.Storage.StorageFile file = await folder.GetFileAsync(@filename);
                Audio.AutoPlay = false;
                Audio.Source = Windows.Media.Core.MediaSource.CreateFromStorageFile(file);
                Audio.Volume = 0.3;
                Audio.Play();
            }
        }

        private async void TurnCard()
        {

            if (gameState == RoundState.PreFlop)
            {
                gameState = RoundState.Flop;
                var cards = dealer.DealFlop().ToArray();
                card1 = cards[0];
                card2 = cards[1];
                card3 = cards[2];
                CardImage1.Source = new BitmapImage(new Uri(card1.ImgUrl.ToString()));
                CardImage2.Source = new BitmapImage(new Uri(card2.ImgUrl.ToString()));
                CardImage3.Source = new BitmapImage(new Uri(card3.ImgUrl.ToString()));
                PlayAudio("card.wav");
            }
            else if (gameState == RoundState.Flop)
            {
                gameState = RoundState.Turn;
                card4 = dealer.DealTurn();
                CardImage4.Source = new BitmapImage(new Uri(card4.ImgUrl.ToString()));
                PlayAudio("card.wav");
            }
            else if (gameState == RoundState.Turn)
            {
                gameState = RoundState.River;
                card5 = dealer.DealRiver();
                CardImage5.Source = new BitmapImage(new Uri(card5.ImgUrl.ToString()));
                PlayAudio("card.wav");
            }
            else
            {
                ContentDialog tempDebugDialog = new ContentDialog
                {
                    Title = "Message",
                    Content = "All cards are on the table",
                    PrimaryButtonText = "Ok"
                };
                ContentDialogResult result = await tempDebugDialog.ShowAsync();
                ShowAllCards();
            }
        }

        private void ShowAllCards()
        {
            if (playerCount > 1)
            {
                var playerTwoCards = players[1].GetCards().ToArray();
                GridP2.Visibility = Visibility.Visible;
                PlayerTwoCardImage1.Source = new BitmapImage(new Uri(playerTwoCards[0].ImgUrl));
                PlayerTwoCardImage2.Source = new BitmapImage(new Uri(playerTwoCards[1].ImgUrl));
                PlayerTwoTextHand.Text = players[1].GetScore().ToString();
            }

            if (playerCount > 2)
            {

                var playerThreeCards = players[2].GetCards().ToArray();
                GridP3.Visibility = Visibility.Visible;
                PlayerThreeCardImage1.Source = new BitmapImage(new Uri(playerThreeCards[0].ImgUrl));
                PlayerThreeCardImage2.Source = new BitmapImage(new Uri(playerThreeCards[1].ImgUrl));
                PlayerThreeTextHand.Text = players[2].GetScore().ToString();
            }

            if (playerCount > 3)
            {
                var playerFourCards = players[3].GetCards().ToArray();
                GridP4.Visibility = Visibility.Visible;
                PlayerFourCardImage1.Source = new BitmapImage(new Uri(playerFourCards[0].ImgUrl));
                PlayerFourCardImage2.Source = new BitmapImage(new Uri(playerFourCards[1].ImgUrl));
                PlayerFourTextHand.Text = players[3].GetScore().ToString();
            }

            if (playerCount > 4)
            {
                var playerFiveCards = players[4].GetCards().ToArray();
                GridP2.Visibility = Visibility.Visible;
                PlayerFiveCardImage1.Source = new BitmapImage(new Uri(playerFiveCards[0].ImgUrl));
                PlayerFiveCardImage2.Source = new BitmapImage(new Uri(playerFiveCards[1].ImgUrl));
                PlayerFiveTextHand.Text = players[4].GetScore().ToString();
            }

            if (playerCount > 5)
            {
                var playerSixCards = players[5].GetCards().ToArray();
                GridP2.Visibility = Visibility.Visible;
                PlayerSixCardImage1.Source = new BitmapImage(new Uri(playerSixCards[0].ImgUrl));
                PlayerSixCardImage2.Source = new BitmapImage(new Uri(playerSixCards[1].ImgUrl));
                PlayerSixTextHand.Text = players[5].GetScore().ToString();
            }
        }


        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }

        // Handles the Click event on the Button on the page and opens the Popup. 
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OptionsPage));
        }
    }
}