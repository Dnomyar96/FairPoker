using System;
using System.Collections.Generic;
using System.Linq;
using FairPoker.Classes;
using FairPoker.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Playback;
using System.Threading;


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
        Deck Cards = new Deck();
        List<Card> tableCards;
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
                players.Add(new Player());
            }           
            DealCards();
            SetScores();       
        }



        private void Quit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Exit();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Check Logic
        }
        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Fold Logic
        }
        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Call Logic
        }
        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Raise Logic
            if (Settings.SoundEffects)
            {
                CoinAudio();
            }
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

            SetDefaultValues();
            dealer.NewRound();

            foreach(var player in players)
            {
              player.NewRound();
            }
         
            DealCards();
            SetScores();
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
                }
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
              tableCards = new List<Card>()
                {
                    card1,
                    card2,
                    card3,
                    card4,
                    card5
                }.Where(c => c != null).ToList();

                Thread t = new Thread(() => player.CalculateScore(tableCards));
                t.Start();

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

       private async void CardAudio()
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync(@"card.wav");
            Audio.AutoPlay = false;
            Audio.Source = Windows.Media.Core.MediaSource.CreateFromStorageFile(file);
            Audio.Volume = 0.3;
            Audio.Play();
        }
        private async void CoinAudio()
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync(@"chips.wav");
            Audio.AutoPlay = false;
            Audio.Source = Windows.Media.Core.MediaSource.CreateFromStorageFile(file);
            Audio.Volume = 0.3;
            Audio.Play();
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
                if (Settings.SoundEffects)
                {
                    CardAudio();
                }
            }
            else if (gameState == RoundState.Flop)
            {
                gameState = RoundState.Turn;
                card4 = dealer.DealTurn();
                CardImage4.Source = new BitmapImage(new Uri(card4.ImgUrl.ToString()));
                if (Settings.SoundEffects)
                {
                    CardAudio();
                }
            }
            else if (gameState == RoundState.Turn)
            {
                gameState = RoundState.River;
                card5 = dealer.DealRiver();
                CardImage5.Source = new BitmapImage(new Uri(card5.ImgUrl.ToString()));
                if (Settings.SoundEffects)
                {
                    CardAudio();
                }
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