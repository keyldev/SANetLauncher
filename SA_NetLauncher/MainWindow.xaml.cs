using launcher_template.Core;
using launcher_template.UI.News;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

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
        }

        private void bClose_Click(object sender, RoutedEventArgs e) =>
            System.Windows.Application.Current.Shutdown();

        private void bHide_Click(object sender, RoutedEventArgs e) =>
            this.WindowState = WindowState.Minimized;


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            this.DragMove();

        private void bMainSite_Click(object sender, RoutedEventArgs e) =>
            Process.Start("https://vk.com/id1");

        private void bPlay_Click(object sender, RoutedEventArgs e)
        {

            if (ServerInfo.isGameInstalled == true)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var api = new VkApi();
            api.Authorize(new ApiAuthParams { AccessToken = "vk_token_here" });

            var get = api.Wall.Get(new WallGetParams
            {
                OwnerId = -1, // id group with minus
                Domain = "@group_url_here",
                Offset = 0,
                Count = 5,
                Extended = true
            });
            tbHeaderText.Text = get.WallPosts[0].Text;
            var uriImageSource = new Uri(getPhotoURL(get.WallPosts[0].Attachment.Instance as Photo));

            imgTest.Source = new BitmapImage(uriImageSource);
            for (int i = 0; i < 5; i++)
                newsPanel.Children.Add(new Additional(get.WallPosts[i].Text, get.WallPosts[i].Text));
        }
        string getPhotoURL(VkNet.Model.Attachments.Photo photo)
        {
            if (photo == null)
            {
                return "https://sun2.43222.userapi.com/3_cayJRQoas6-ymflVhL8b5goDA_zNfB4lnzDg/-pv5ESBPKX8.jpg";
            }
            if (photo.Sizes?.Count > 0)
            {
                var bigSize = photo.Sizes[0];
                for (int i = 0; i < photo.Sizes.Count; i++)
                {
                    var photoSize = photo.Sizes[i];
                    if (photoSize.Height > bigSize.Height && photoSize.Width > bigSize.Width)
                        bigSize = photoSize;
                }

                return bigSize.Url.ToString();
            }
            return "https://sun2.43222.userapi.com/3_cayJRQoas6-ymflVhL8b5goDA_zNfB4lnzDg/-pv5ESBPKX8.jpg";
        }
    }
}
