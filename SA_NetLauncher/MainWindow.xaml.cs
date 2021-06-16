using launcher_template.Core;
using launcher_template.UI.News;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

namespace launcher_template
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!ServerInfo.isGameInstalled)
                bPlay.Content = "Обновить";
            
            for (int i = 0; i < 5; i++)
                newsPanel.Children.Add(new Additional());

        }

        private void bClose_Click(object sender, RoutedEventArgs e) =>
            Application.Current.Shutdown();

        private void bHide_Click(object sender, RoutedEventArgs e) =>
            this.WindowState = WindowState.Minimized;


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            this.DragMove();

        private void bMainSite_Click(object sender, RoutedEventArgs e) =>
            Process.Start("https://vk.com/id1");

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {
            if(ServerInfo.isGameInstalled == true)
            {
                Process.Start("multiplayer_browser_cr.exe");
            }
            else
            {
                WebClient downloader = new WebClient();
                downloader.DownloadProgressChanged += (o, x) =>
                {
                    downloadProgress.Value = x.ProgressPercentage;

                };
                downloader.DownloadFileCompleted += (o, z) =>
                {
                    downloadProgress.Value = 100;
                    bPlay.Content = "Играть";
                };
                downloader.DownloadFileAsync(new Uri("здесь_ссылка_на_файл"), "здесь_название_файла . расширение");
            }
        }
    }
}
