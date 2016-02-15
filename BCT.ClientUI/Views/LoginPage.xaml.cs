using BCT.ClientCore;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            LockScreen();

            var client = App.Current.Properties["Client"] as Client;

            Login login = new Login();
            login.Username = TextBoxUsername.Text;
            login.Password = PasswordBoxPassword.Password;
            
            var response = await client.SendAsync(login, RequestType.Login);

            if (response.ResponseType == ResponseType.Success)
            {
                App.Current.Properties["LoggedUser"] = login.Username;
                this.NavigationService.Navigate(new HomePage());
            }
            else
            {
                UnlockScreen();
                TextBlockErrorMessage.Visibility = Visibility.Visible;
                TextBlockErrorMessage.Text = response.Message;
            }
        }

        private void LockScreen()
        {
            LoadingSpinner.Visibility = Visibility.Visible;
            TextBlockErrorMessage.Visibility = Visibility.Collapsed;
            BtnLogin.IsEnabled = false;
            TextBoxUsername.IsEnabled = false;
            PasswordBoxPassword.IsEnabled = false;
        }

        private void UnlockScreen()
        {
            LoadingSpinner.Visibility = Visibility.Collapsed;
            BtnLogin.IsEnabled = true;
            TextBoxUsername.IsEnabled = true;
            PasswordBoxPassword.IsEnabled = true;
        }
    }
}
