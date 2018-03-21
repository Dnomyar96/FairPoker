using System;
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
        

        public MainPage()
        {
            this.InitializeComponent();

            /// Required Set After Initialisation
            CardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage3.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage4.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
            CardImage5.Source = new BitmapImage(new Uri("ms-appx:///Assets/Stenden.png"));
      
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
        }

        private async void TurnCard()
        {
            if (GameState == 1)
            {
                card1 = Cards.DrawCard();
                card2 = Cards.DrawCard();
                card3 = Cards.DrawCard();
                GameState++;
                CardImage1.Source = new BitmapImage(new Uri(card1.ImgUrl.ToString()));
                CardImage2.Source = new BitmapImage(new Uri(card2.ImgUrl.ToString()));
                CardImage3.Source = new BitmapImage(new Uri(card3.ImgUrl.ToString()));
            }
            else if (GameState == 2)
            {
                GameState++;
                card4 = Cards.DrawCard();
                CardImage4.Source = new BitmapImage(new Uri(card4.ImgUrl.ToString()));
            }

            else if (GameState == 3)
            {
                GameState++;
                card5 = Cards.DrawCard();
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
    }
}