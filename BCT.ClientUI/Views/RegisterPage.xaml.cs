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
            var client = App.Current.Properties["Client"] as Client.Client;
            
            Register reg = new Register();
            reg.Username = TextBoxUsername.Text;
            reg.Password = PasswordBoxPassword.Password;

            LoadingSpinner.Visibility = Visibility.Visible;
            TextBlockErrorMessage.Visibility = Visibility.Collapsed;

            var response = await client.SendAsync(reg, RequestType.Register);
            
            LoadingSpinner.Visibility = Visibility.Collapsed;

            if (response.ResponseType == ResponseType.Success)
            {
                App.Current.Properties["LoggedUser"] = reg.Username;
                this.NavigationService.Navigate(new HomePage());
            }
            else
            {
                TextBlockErrorMessage.Visibility = Visibility.Visible;
                TextBlockErrorMessage.Text = response.Message;
            }
        }
    }
}
