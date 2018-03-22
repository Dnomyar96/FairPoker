using FairPoker.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FairPoker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OptionsPage : Page
    {
        public OptionsPage()
        {
            this.InitializeComponent();
            PlayerCount.SelectedIndex = Settings.PlayerCount - 2;
            HideCards.IsOn = Settings.HideOtherPlayersCards;
        }

        // Handles the Click event on the Button on the page and opens the Popup. 
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(OptionsPage));

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void NewRound_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            var playercountValue = (ComboBoxItem)PlayerCount.SelectedValue;

            if (playercountValue != null)
                Settings.PlayerCount = int.Parse(playercountValue.Content.ToString());
            else
                Settings.PlayerCount = 2;

            Settings.HideOtherPlayersCards = HideCards.IsOn;

            this.Frame.Navigate(typeof(MainPage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
