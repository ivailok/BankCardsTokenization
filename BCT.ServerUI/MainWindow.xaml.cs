using BCT.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace BCT.ServerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TokenizationService tokenizationService;
        private Server.Server server;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.server = new Server.Server();
            this.tokenizationService = new TokenizationService();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            this.server.Dispose();
        }

        private void ExportTokens(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.AddExtension = true;
            save.OverwritePrompt = true;
            save.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            save.DefaultExt = ".txt";
            save.FileOk += SaveTokens_FileOk;
            save.ShowDialog();
        }

        private async void SaveTokens_FileOk(object sender, CancelEventArgs e)
        {
            LockScreen();

            var cardsList = await Task.Run(() => { return this.tokenizationService.GetEntries().OrderBy(x => x.Token); });
            var filename = (sender as SaveFileDialog).FileName;
            using (StreamWriter sw = new StreamWriter(filename))
            {
                await sw.WriteLineAsync(string.Format("{0,-16} - {1,-16}", "Token", "Card number"));
                foreach (var item in cardsList)
                {
                    await sw.WriteLineAsync(string.Format("{0} - {1}", item.Token, item.CardNumber));
                }
            }

            UnlockScreen();
        }

        private void ExportCards(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.AddExtension = true;
            save.OverwritePrompt = true;
            save.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            save.DefaultExt = ".txt";
            save.FileOk += SaveCards_FileOk;
            save.ShowDialog();
        }

        private async void SaveCards_FileOk(object sender, CancelEventArgs e)
        {
            LockScreen();

            var cardsList = await Task.Run(() => { return this.tokenizationService.GetEntries().GroupBy(x => x.CardNumber).OrderBy(x => x.Key); });
            var filename = (sender as SaveFileDialog).FileName;
            bool hasPrevious;

            using (StreamWriter sw = new StreamWriter(filename))
            {
                await sw.WriteLineAsync(string.Format("{0,-16} - Tokens", "Card number", "Card number"));
                foreach (var group in cardsList)
                {
                    hasPrevious = false;
                    await sw.WriteAsync(string.Format("{0} - ", group.Key));
                    foreach (var item in group)
                    {
                        if (hasPrevious)
                        {
                            await sw.WriteAsync(string.Format(", {0}", item.Token));
                        }
                        else
                        {
                            await sw.WriteAsync(item.Token);
                            hasPrevious = true;
                        }
                    }
                    await sw.WriteLineAsync();
                }
            }

            UnlockScreen();
        }

        private void LockScreen()
        {
            BtnExportTokens.IsEnabled = false;
            BtnExportCards.IsEnabled = false;
            LoadingSpinner.Visibility = Visibility.Visible;
        }

        private void UnlockScreen()
        {
            BtnExportTokens.IsEnabled = true;
            BtnExportCards.IsEnabled = true;
            LoadingSpinner.Visibility = Visibility.Collapsed;
        }
    }
}
