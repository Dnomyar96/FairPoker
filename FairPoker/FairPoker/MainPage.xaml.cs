using System;
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

        int GameState = 1;

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
                GameState++;
                CardImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png"));
                CardImage2.Source = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png"));
                CardImage3.Source = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png"));
            }
            else if (GameState == 2)
            {
                GameState++;
                CardImage4.Source = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png"));
            }
            else if (GameState == 3)
            {
                GameState++;
                CardImage5.Source = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png"));
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