﻿using System;
using System.Collections.Generic;
using System.Linq;
using FairPoker.Classes;
using FairPoker.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FairPoker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        private RoundState gameState = RoundState.PreFlop;
        Deck Cards = new Deck();
        private Card card1;
        private Card card2;
        private Card card3;
        private Card card4;
        private Card card5;

        private Dealer dealer;
        private int playerCount = Settings.PlayerCount;
        private List<Player> players;

        public MainPage()
        {
            this.InitializeComponent();

            /// Required Set After Initialisation
            SetDefaultImages();

            dealer = new Dealer();
            players = new List<Player>();
          

            for (var i = 0; i < playerCount; i++)
            {
                players.Add(new Player());
            }
            DealCards();
            SetScores();

        }       
        
        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewRound_Click(object sender, RoutedEventArgs e)
        {
            card1 = null;
            card2 = null;
            card3 = null;
            card4 = null;
            card5 = null;

            gameState = RoundState.PreFlop;

            SetDefaultImages();
            dealer.NewRound();

            foreach(var player in players)
            {
                player.NewRound();
            }

            DealCards();
            SetScores();
        }

        private void SetDefaultImages()
        {
            CardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage3.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage4.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage5.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
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
            

            var playerTwoCards = players[1].GetCards().ToArray();
            PlayerTwoCardImage1.Source = new BitmapImage(new Uri(playerTwoCards[0].ImgUrl));
            PlayerTwoCardImage2.Source = new BitmapImage(new Uri(playerTwoCards[1].ImgUrl));

            if (playerCount > 2)
            {
                var playerThreeCards = players[2].GetCards().ToArray();
                PlayerThreeCardImage1.Source = new BitmapImage(new Uri(playerThreeCards[0].ImgUrl));
                PlayerThreeCardImage2.Source = new BitmapImage(new Uri(playerThreeCards[1].ImgUrl));
            }
            if (playerCount > 3)
            {

                var playerFourCards = players[3].GetCards().ToArray();
                PlayerFourCardImage1.Source = new BitmapImage(new Uri(playerFourCards[0].ImgUrl));
                PlayerFourCardImage2.Source = new BitmapImage(new Uri(playerFourCards[1].ImgUrl));
            }
            if (playerCount > 4)
            {
                var playerFiveCards = players[4].GetCards().ToArray();
                PlayerFiveCardImage1.Source = new BitmapImage(new Uri(playerFiveCards[0].ImgUrl));
                PlayerFiveCardImage2.Source = new BitmapImage(new Uri(playerFiveCards[1].ImgUrl));
            }
            if (playerCount > 5)
            {
                var playerSixCards = players[5].GetCards().ToArray();
                PlayerSixCardImage1.Source = new BitmapImage(new Uri(playerSixCards[0].ImgUrl));
                PlayerSixCardImage2.Source = new BitmapImage(new Uri(playerSixCards[1].ImgUrl));
            }
            SetScores();
        }       

        private void CardClick(object sender, RoutedEventArgs e)
        {
            TurnCard();
            SetScores();
        }

        private void SetScores()
        {
            foreach (var player in players)
            {
                player.CalculateScore(new List<Card>()
                {
                    card1,
                    card2,
                    card3,
                    card4,
                    card5
                }.Where(c => c != null).ToList());
            }

          PlayerOneTextHand.Text = players[0].GetScore().ToString();
        
            if (playerCount > 1)
            {
                PlayerTwoTextHand.Text = players[1].GetScore().ToString();
            }
            if (playerCount > 2)
            {
                PlayerThreeTextHand.Text = players[2].GetScore().ToString();
            }
            if (playerCount > 3)
            {
                PlayerFourTextHand.Text = players[3].GetScore().ToString();
            }
            if (playerCount > 4)
            {
                PlayerFiveTextHand.Text = players[4].GetScore().ToString();
            }
            if (playerCount > 5)
            {
                PlayerSixTextHand.Text = players[5].GetScore().ToString();
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
            }
            else if (gameState == RoundState.Flop)
            {
                gameState = RoundState.Turn;
                card4 = dealer.DealTurn();
                CardImage4.Source = new BitmapImage(new Uri(card4.ImgUrl.ToString()));
            }
            else if (gameState == RoundState.Turn)
            {
                gameState = RoundState.River;
                card5 = dealer.DealRiver();
                CardImage5.Source = new BitmapImage(new Uri(card5.ImgUrl.ToString()));
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