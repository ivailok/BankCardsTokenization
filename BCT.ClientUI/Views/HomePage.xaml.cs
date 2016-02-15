using BCT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BCT.ClientUI.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            TextBlockUsername.Text = App.Current.Properties["LoggedUser"] as string;
        }

        private async void Logout(object sender, RoutedEventArgs e)
        {
            var client = App.Current.Properties["Client"] as Client.Client;

            var response = await client.SendAsync(null, RequestType.Logout);

            if (response.ResponseType == ResponseType.Success)
            {
                App.Current.Properties.Remove("LoggedUser");
                this.NavigationService.Navigate(new LoginPage());
            }
        }

        private async void GetTokenByCardNumber(object sender, RoutedEventArgs e)
        {
            LockScreen();

            var client = App.Current.Properties["Client"] as Client.Client;

            var response = await client.SendAsync(TextBoxCardNumber.Text, RequestType.RegisterToken);
            
            UnlockScreen();

            if (response.ResponseType == ResponseType.Success)
            {
                TextBoxToken.Text = response.Data as string;
            }
            else
            {
                TextBlockError.Visibility = Visibility.Visible;
                TextBlockError.Text = response.Message;
            }
        }

        private async void GetCardNumberByToken(object sender, RoutedEventArgs e)
        {
            LockScreen();

            var client = App.Current.Properties["Client"] as Client.Client;

            var response = await client.SendAsync(TextBoxToken.Text, RequestType.GetCardNumber);

            UnlockScreen();

            if (response.ResponseType == ResponseType.Success)
            {
                TextBoxCardNumber.Text = response.Data as string;
            }
            else
            {
                TextBlockError.Visibility = Visibility.Visible;
                TextBlockError.Text = response.Message;
            }
        }

        private void LockScreen()
        {
            LoadingSpinner.Visibility = Visibility.Visible;
            TextBlockError.Visibility = Visibility.Collapsed;
            BtnConvertCard.IsEnabled = false;
            BtnConvertToken.IsEnabled = false;
            TextBoxCardNumber.IsEnabled = false;
            TextBoxToken.IsEnabled = false;
        }

        private void UnlockScreen()
        {
            LoadingSpinner.Visibility = Visibility.Collapsed;
            BtnConvertCard.IsEnabled = true;
            BtnConvertToken.IsEnabled = true;
            TextBoxCardNumber.IsEnabled = true;
            TextBoxToken.IsEnabled = true;
        }
    }
}
