using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Security.Authentication.Web;
using winsdkfb;
using winsdkfb.Graph;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Artgram
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>


    public sealed partial class MainPage : Page
    {
        //int LoginStatus;    //Do sprawdzania stanu logowania (ma być w tym miejscu?) :O
        public string responseServer = "", url = ""; //odpowiedz z serwera
        //string costam = "ms-appx:///Assets/Square150x150Logo.png";

        public MainPage()
        {
            this.InitializeComponent();

            responseServer = Task.Run(() => Wyslanie().Result).Result; 
            //funkcja, która wysyła i odbiera dane z serwera

            //List<Obraz> Obrazy = new List<Obraz>();
            
            List<Obraz> Obrazy = JsonConvert.DeserializeObject<List<Obraz>>(responseServer);
            url = Obrazy[0].Sciezka_dostepu;
            //textBox2.Text = url;

            var zmienna = new Uri(url, UriKind.Absolute);
            var img = new ImageBrush();
            //BitmapImage img1 = ;
            img.ImageSource = new BitmapImage(zmienna);
            button.Background = img;
            

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        public async Task<string> Wyslanie()
        {
            try
            {
                string plik = "{\"tabela\":\"Obrazy\"}";
                string responseServ;

                var request = (HttpWebRequest)WebRequest.Create("http://artgram.hostingpo.pl/login.php");
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
                {
                    streamWriter.Write(plik);
                    streamWriter.Flush();
                    streamWriter.Dispose();
                }
                var response = await request.GetResponseAsync();

                using (var streamreader = new StreamReader(response.GetResponseStream()))
                {
                    responseServ = streamreader.ReadToEnd();
                }
                return responseServ;
            }
            catch
            {
                string responseServ = "Cos nie tak...";
                return responseServ;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View));
        }

        private void button_Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Add));
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View));
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View));
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View));
        }

        private void button_Copy3_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View));
        }

        private void button_Copy4_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View));
        }

        private void button_Copy5_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View));
        }

        private void button_Copy6_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View));
        }

        private void button_Logo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        class Obraz
        {
            public string Nazwa_Obrazu, Sciezka_dostepu, Liczba_WOW;

            public Obraz(string Nazwa_Obrazu, string Sciezka_dostepu, string Liczba_WOW)
            {
                this.Nazwa_Obrazu = Nazwa_Obrazu;
                this.Sciezka_dostepu = Sciezka_dostepu;
                this.Liczba_WOW = Liczba_WOW;
            }
        }
    }
}
