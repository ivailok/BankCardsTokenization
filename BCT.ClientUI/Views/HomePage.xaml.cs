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
            await client.SendAsync(null, RequestType.Logout);

            client.Dispose();
            App.Current.Properties.Remove("LoggedUser");
            App.Current.Properties.Remove("Client");

            this.NavigationService.Navigate(new LoginPage());
        }
    }
}
