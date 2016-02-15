using BCT.ClientCore;
using BCT.ClientUI.Views;
using BCT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BCT.ClientUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Closed += MainWindow_Closed;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Client client = new Client();
            App.Current.Properties["Client"] = client;
            MainFrame.Navigate(new LoginPage());
        }

        private async void MainWindow_Closed(object sender, EventArgs e)
        {
            var client = App.Current.Properties["Client"] as Client;
            var response = await client.SendAsync(null, RequestType.Terminate) as Response;
            if (response.ResponseType == ResponseType.Success)
            {
                client.Dispose();
            }
        }
    }
}
