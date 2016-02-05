using BCT.ClientUI.Views;
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

            MainFrame.Navigate(new LoginPage());
            MainFrame.Navigated += FrameNavigated;
        }

        void FrameNavigated(object sender, NavigationEventArgs e)
        {
            // don't save and show history here
            if (e.Content is LoginPage || e.Content is RegisterPage)
            {
                MainFrame.NavigationService.RemoveBackEntry();
                MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            }
        }
    }
}
