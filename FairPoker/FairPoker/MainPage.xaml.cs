using System;
using System.Collections.Generic;
using System.Linq;
using FairPoker.Classes;
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
        
        private int GameState = 1;
        Deck Cards = new Deck();
        private Card card1;
        private Card card2;
        private Card card3;
        private Card card4;
        private Card card5;

        private Dealer dealer;
        private List<Player> players;

        public MainPage()
        {
            this.InitializeComponent();

            /// Required Set After Initialisation
            CardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage3.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage4.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage5.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));

            dealer = new Dealer();
            players = new List<Player>()
            {
                new Player(),
                new Player()
            };

            for (var i = 0; i < 2; i++)
            {
                foreach (var player in players)
                {
                    dealer.DealCard(player);
                }
            }

            var playerOneCards = players[0].GetCards().ToArray();
            var playerTwoCards = players[1].GetCards().ToArray();
            PlayerOneCardImage1.Source = new BitmapImage(new Uri(playerOneCards[0].ImgUrl));
            PlayerOneCardImage2.Source = new BitmapImage(new Uri(playerOneCards[1].ImgUrl));
            PlayerTwoCardImage1.Source = new BitmapImage(new Uri(playerTwoCards[0].ImgUrl));
            PlayerTwoCardImage2.Source = new BitmapImage(new Uri(playerTwoCards[1].ImgUrl));

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
            this.Frame.Navigate(typeof(MainPage));
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

            PlayerOneScore.Text = players[0].GetScore().ToString();
            PlayerTwoScore.Text = players[1].GetScore().ToString();
        }

        private async void TurnCard()
        {
            if (GameState == 1)
            {
                GameState++;
                var cards = dealer.DealFlop().ToArray();
                card1 = cards[0];
                card2 = cards[1];
                card3 = cards[2];
                CardImage1.Source = new BitmapImage(new Uri(card1.ImgUrl.ToString()));
                CardImage2.Source = new BitmapImage(new Uri(card2.ImgUrl.ToString()));
                CardImage3.Source = new BitmapImage(new Uri(card3.ImgUrl.ToString()));
            }
            else if (GameState == 2)
            {
                GameState++;
                card4 = dealer.DealTurn();
                CardImage4.Source = new BitmapImage(new Uri(card4.ImgUrl.ToString()));
            }
            else if (GameState == 3)
            {
                GameState++;
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