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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void GoToLoginPage(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage());
        }

        private async void Register(object sender, RoutedEventArgs e)
        {
            LockScreen();

            var client = App.Current.Properties["Client"] as Client.Client;
            
            Register reg = new Register();
            reg.Username = TextBoxUsername.Text;
            reg.Password = PasswordBoxPassword.Password;

            int rights = (CheckBoxRegisterToken.IsChecked ?? false) ? (int)Rights.CanRegisterToken : 0;
            rights |= ((CheckBoxGetCard.IsChecked ?? false) ? (int)Rights.CanGetCardNumber : 0);
            reg.Rights = rights;

            var response = await client.SendAsync(reg, RequestType.Register);

            if (response.ResponseType == ResponseType.Success)
            {
                App.Current.Properties["LoggedUser"] = reg.Username;
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
            PasswordBoxRepeatPassword.IsEnabled = false;
        }

        private void UnlockScreen()
        {
            LoadingSpinner.Visibility = Visibility.Collapsed;
            BtnLogin.IsEnabled = true;
            TextBoxUsername.IsEnabled = true;
            PasswordBoxPassword.IsEnabled = true;
            PasswordBoxRepeatPassword.IsEnabled = true;
        }
    }
}
