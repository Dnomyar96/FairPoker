using System;
using System.Collections.Generic;
using System.Linq;
using FairPoker.Classes;
using FairPoker.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Playback;
using System.Threading.Tasks;
using FairPoker.Exceptions;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FairPoker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GameState gameState;
        private int playerCount = Settings.PlayerCount;

        //Sounds
        MediaPlayer audio;

        /// <inheritdoc />
        /// <summary>
        /// Constructor for MainPage
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            gameState = new GameState();
            audio = new MediaPlayer();
            // Required Set After Initialisation
            SetDefaultValues();

            DealCards();
            SetScores();
            SetChance();
        }

        /// <summary>
        /// Click function to quit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        /// <summary>
        /// Click function for the check button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            gameState.Players[0].Check();
            Next();
        }

        /// <summary>
        /// Click function for fold button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            gameState.Players[0].Fold();
            Next();
        }

        /// <summary>
        /// Click function for call button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            int amount = gameState.RequiredBetPerPlayer;

            if (amount > 0)
            {
                try
                {
                    gameState.Players[0].Call(amount);
                }
                catch(NotEnoughCashException)
                {
                    gameState.Players[0].AllIn();
                }
            }
            Next();
        }

        /// <summary>
        /// Click function for raise button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Raise Logic
            //Raise the bet by doubling the amount of the big blind. A player may raise more depending on the betting style being played.
            int amount = 50;

            try
            {
                gameState.Players[0].Raise(amount);
            }
            catch (NotEnoughCashException)
            {
                gameState.Players[0].AllIn();
            }

            gameState.RequiredBetPerPlayer += amount;

            PlayAudio("chips.wav");
            Next();
        }

        /// <summary>
        /// Click function for new round
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRound_Click(object sender, RoutedEventArgs e)
        {
            DealCards();
            SetScores();
            SetChance();
        }

        /// <summary>
        /// Reset the labels which display the chances
        /// </summary>
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

        /// <summary>
        /// Set default values
        /// </summary>
        private void SetDefaultValues()
        {
            var defaultCardUrl = "ms-appx:///Assets/Stenden.png";
            SetImage(CardImage1, defaultCardUrl);
            SetImage(CardImage2, defaultCardUrl);
            SetImage(CardImage3, defaultCardUrl);
            SetImage(CardImage4, defaultCardUrl);
            SetImage(CardImage5, defaultCardUrl);

            SetImage(PlayerOneCardImage1, defaultCardUrl);
            SetImage(PlayerOneCardImage2, defaultCardUrl);
            PlayerOneTextHand.Text = "";

            SetImage(PlayerTwoCardImage1, defaultCardUrl);
            SetImage(PlayerTwoCardImage2, defaultCardUrl);
            PlayerTwoTextHand.Text = "";

            SetImage(PlayerThreeCardImage1, defaultCardUrl);
            SetImage(PlayerThreeCardImage2, defaultCardUrl);
            PlayerThreeTextHand.Text = "";

            SetImage(PlayerFourCardImage1, defaultCardUrl);
            SetImage(PlayerFourCardImage2, defaultCardUrl);
            PlayerFourTextHand.Text = "";

            SetImage(PlayerFiveCardImage1, defaultCardUrl);
            SetImage(PlayerFiveCardImage2, defaultCardUrl);
            PlayerFiveTextHand.Text = "";

            SetImage(PlayerSixCardImage1, defaultCardUrl);
            SetImage(PlayerSixCardImage2, defaultCardUrl);
            PlayerSixTextHand.Text = "";

            if (!Settings.HideChances)
            {
                ChanceList.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Method to deal the cards
        /// </summary>
        private void DealCards()
        {
            for (var i = 0; i < 2; i++)
            {
                foreach (var player in gameState.Players)
                {
                    gameState.Dealer.DealCard(player);
                }
            }

            var playerOneCards = gameState.Players[0].GetCards().ToArray();
            SetImage(PlayerOneCardImage1, playerOneCards[0].ImgUrl);
            SetImage(PlayerOneCardImage2, playerOneCards[1].ImgUrl);

            if (playerCount > 1)
                SetPlayerCards(gameState.Players[1], PlayerTwoCardImage1, PlayerTwoCardImage2, PlayerTwoTextCash, PlayerTwoTextStatus, GridP2);

            if (playerCount > 2)
                SetPlayerCards(gameState.Players[2], PlayerThreeCardImage1, PlayerThreeCardImage2, PlayerThreeTextCash, PlayerThreeTextStatus, GridP3);

            if (playerCount > 3)
                SetPlayerCards(gameState.Players[3], PlayerFourCardImage1, PlayerFourCardImage2, PlayerFourTextCash, PlayerFourTextStatus, GridP4);

            if (playerCount > 4)
                SetPlayerCards(gameState.Players[4], PlayerFiveCardImage1, PlayerFiveCardImage2, PlayerFiveTextCash, PlayerFiveTextStatus, GridP5);

            if (playerCount > 5)
                SetPlayerCards(gameState.Players[5], PlayerSixCardImage1, PlayerSixCardImage2, PlayerSixTextCash, PlayerSixTextStatus, GridP6);

            SetScores();
            SetChance();
            Play();
        }

        private void SetPlayerCards(Player player, Image image, Image imageTwo, TextBlock cashText, TextBlock statusText, Grid grid)
        {
            var betting = player.BettingCash.ToString();
            var status = player.GetPlayerState().ToString();

            grid.Visibility = Visibility.Visible;
            if (Settings.HideOtherPlayersCards == false)
            {
                var cards = player.GetCards().ToArray();
                SetImage(image, cards[0].ImgUrl);
                SetImage(image, cards[1].ImgUrl);
            }
            cashText.Text = "\u20AC" + betting;
            statusText.Text = status;
        }

        /// <summary>
        /// Click function for cards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardClick(object sender, RoutedEventArgs e)
        {
            Next();
        }

        /// <summary>
        /// Method to set scores
        /// </summary>
        private void SetScores()
        {
            var tableCards = gameState.TableCards.Where(c => c != null).ToList();
            foreach (Player player in gameState.Players)
            {
                Task t = new Task(() => player.CalculateScore(tableCards));
                t.Start();
                player.CalculateScore(tableCards);
            }

            PlayerOneTextHand.Text = gameState.Players[0].GetScore().ToString();
            PlayerOneTotalMoneyText.Text = gameState.Players[0].GetTotalCash().ToString();

            if (playerCount > 1)
                SetPlayerTexts(new TextBlock[] { PlayerTwoTextHand, PlayerTwoTextCash, PlayerTwoTextStatus }, gameState.Players[1]);

            if (playerCount > 2)
                SetPlayerTexts(new TextBlock[] { PlayerThreeTextHand, PlayerThreeTextCash, PlayerThreeTextStatus }, gameState.Players[2]);

            if (playerCount > 3)
                SetPlayerTexts(new TextBlock[] { PlayerFourTextHand, PlayerFourTextCash, PlayerFourTextStatus }, gameState.Players[3]);

            if (playerCount > 4)
                SetPlayerTexts(new TextBlock[] { PlayerFiveTextHand, PlayerFiveTextCash, PlayerFiveTextStatus }, gameState.Players[4]);

            if (playerCount > 5)
                SetPlayerTexts(new TextBlock[] { PlayerSixTextHand, PlayerSixTextCash, PlayerSixTextStatus }, gameState.Players[5]);
        }

        private void SetPlayerTexts(TextBlock[] textBlocks, Player player)
        {
            if (Settings.HideOtherPlayersCards == false)
                textBlocks[0].Text = player.GetScore().ToString();

            textBlocks[1].Text = player.GetBet().ToString();
            textBlocks[2].Text = player.GetStatus().ToString();
        }

        /// <summary>
        /// Method to set the chance
        /// </summary>
        private void SetChance()
        {
            var player = gameState.Players.FirstOrDefault();
            var cards = player.GetCards().Concat(gameState.TableCards);
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

        /// <summary>
        /// Method to play when cards are turned. Triggers tasks for players
        /// </summary>
        public async void Play()
        {
            foreach (var player in gameState.Players)
            {
                var t = new Task(() => player.GetScore());
                t.Start();
            }

            foreach (var player in gameState.Players)
            {
                if (!player.Equals(gameState.Players[0]))
                {
                    await player.Turn(0); // TODO
                }
            }
        }

        /// <summary>
        /// Method to play audio file
        /// </summary>
        /// <param name="filename"></param>
        private async void PlayAudio(string filename)
        {
            if (Settings.SoundEffects)
            {
                Windows.Storage.StorageFolder folder =
                    await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
                Windows.Storage.StorageFile file = await folder.GetFileAsync(@filename);
                audio.AutoPlay = false;
                audio.Source = Windows.Media.Core.MediaSource.CreateFromStorageFile(file);
                audio.Volume = 0.3;
                audio.Play();
            }
        }

        /// <summary>
        /// Method for turning a card
        /// </summary>
        private async void TurnCard()
        {
            if (gameState.RoundState == RoundState.PreFlop)
            {
                gameState.RoundState = RoundState.Flop;
                var cards = gameState.Dealer.DealFlop().ToArray();
                gameState.TableCards.AddRange(cards);

                SetImage(CardImage1, gameState.TableCards[0].ImgUrl.ToString());
                SetImage(CardImage2, gameState.TableCards[1].ImgUrl.ToString());
                SetImage(CardImage3, gameState.TableCards[2].ImgUrl.ToString());
                PlayAudio("card.wav");
            }
            else if (gameState.RoundState == RoundState.Flop)
            {
                gameState.RoundState = RoundState.Turn;
                gameState.TableCards.Add(gameState.Dealer.DealTurn());
                SetImage(CardImage4, gameState.TableCards[3].ImgUrl.ToString());
                PlayAudio("card.wav");
            }
            else if (gameState.RoundState == RoundState.Turn)
            {
                gameState.RoundState = RoundState.River;
                gameState.TableCards.Add(gameState.Dealer.DealTurn());
                SetImage(CardImage5, gameState.TableCards[4].ImgUrl.ToString());
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

        private void SetImage(Image image, string imgUrl)
        {
            image.Source = new BitmapImage(new Uri(imgUrl));
        }

        /// <summary>
        /// Method to show all cards
        /// </summary>
        private void ShowAllCards()
        {
            if (playerCount > 1)
                ShowPlayerCards(gameState.Players[1], PlayerTwoCardImage1, PlayerTwoCardImage2, PlayerTwoTextHand);

            if (playerCount > 2)
                ShowPlayerCards(gameState.Players[2], PlayerThreeCardImage1, PlayerThreeCardImage2, PlayerThreeTextHand);

            if (playerCount > 3)
                ShowPlayerCards(gameState.Players[3], PlayerFourCardImage1, PlayerFourCardImage2, PlayerFourTextHand);

            if (playerCount > 4)
                ShowPlayerCards(gameState.Players[4], PlayerFiveCardImage1, PlayerFiveCardImage2, PlayerFiveTextHand);

            if (playerCount > 5)
                ShowPlayerCards(gameState.Players[5], PlayerSixCardImage1, PlayerSixCardImage2, PlayerSixTextHand);
        }

        private void ShowPlayerCards(Player player, Image image, Image imageTwo, TextBlock handText)
        {
            var playerCards = player.GetCards().ToArray();
            GridP2.Visibility = Visibility.Visible;
            SetImage(image, playerCards[0].ImgUrl);
            SetImage(imageTwo, playerCards[1].ImgUrl);
            handText.Text = player.GetScore().ToString();
        }

        /// <summary>
        /// Method to show popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OptionsPage));
        }

        /// <summary>
        /// Click function for button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Does this need to do anything? If not, delete it.
        }

        private void Next()
        {
            TurnCard();
            SetScores();
            SetChance();
            Play();
        }
    }
}