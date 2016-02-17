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
        private SHA256Hasher hasher;
        private Validator validator;

        public LoginPage()
        {
            InitializeComponent();

            Loaded += LoginPage_Loaded;
        }

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.hasher = new SHA256Hasher();
            this.validator = new Validator();
        }
        
        private async void Login(object sender, RoutedEventArgs e)
        {
            LockScreen();

            var client = App.Current.Properties["Client"] as Client;

            if (Validate())
            {
                Login login = new Login();
                login.Username = TextBoxUsername.Text;
                login.Password = this.hasher.ComputeHash(PasswordBoxPassword.Password);

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
            else
            {
                UnlockScreen();
                TextBlockErrorMessage.Visibility = Visibility.Visible;
                TextBlockErrorMessage.Text = "Invalid format of username or pass";
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

        private bool Validate()
        {
            return this.validator.ValidateUsername(TextBoxUsername.Text) &&
                   this.validator.ValidatePassword(PasswordBoxPassword.Password);
        }
    }
}
