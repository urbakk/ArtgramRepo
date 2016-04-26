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
using System.Diagnostics;
using System.Threading;
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
        private string responseServer, url, zap, 
            link = "http://artgram.hostingpo.pl/login.php";
        private ImageBrush tlo = new ImageBrush();

        public MainPage()
        {
            this.InitializeComponent();
            //obekty do zapytan
            Zapytanie rzezba = new Zapytanie("Nazwa_obrazu", "Sciezka_dostepu", "Liczba_WOW", "2"); 
            Zapytanie malunek = new Zapytanie("Nazwa_obrazu", "Sciezka_dostepu", "Liczba_WOW", "3");
            Zapytanie rys = new Zapytanie("Nazwa_obrazu", "Sciezka_dostepu", "Liczba_WOW", "4");
            Zapytanie tatu = new Zapytanie("Nazwa_obrazu", "Sciezka_dostepu", "Liczba_WOW", "5");
            //dane do wyszukania w zapytaniu

            //wysłanie zapytania do serwera i przekonwertowanie danych na JSON
            zap = JsonConvert.SerializeObject(rzezba); //konwerter do JSONa
            //textBox2.Text = zap;
            
            responseServer = Task.Run(() => Wyslanie(link, zap).Result).Result; 
            List<Obraz> oRzezba = JsonConvert.DeserializeObject<List<Obraz>>(responseServer);

            zap = JsonConvert.SerializeObject(malunek); //konwerter do JSONa
            responseServer = Task.Run(() => Wyslanie(link, zap).Result).Result;
            List<Obraz> oMalunek = JsonConvert.DeserializeObject<List<Obraz>>(responseServer);

<<<<<<< HEAD
            //List<Obraz> Obrazy = new List<Obraz>();
            
            List<Obraz> Obrazy = JsonConvert.DeserializeObject<List<Obraz>>(responseServer);
            url = Obrazy[0].Sciezka_dostepu;
            //textBox2.Text = url;
=======
            zap = JsonConvert.SerializeObject(rys); //konwerter do JSONa
            responseServer = Task.Run(() => Wyslanie(link, zap).Result).Result;
            List<Obraz> oRys = JsonConvert.DeserializeObject<List<Obraz>>(responseServer);
>>>>>>> refs/remotes/origin/master

            zap = JsonConvert.SerializeObject(tatu); //konwerter do JSONa
            responseServer = Task.Run(() => Wyslanie(link, zap).Result).Result;
            List<Obraz> oTatu = JsonConvert.DeserializeObject<List<Obraz>>(responseServer);

            
            url = oRzezba[1].Sciezka_dostepu; //wyciągnięcie ścieżki do obrazu
            //tlo = Task.Run(() => Zmiana_tla(url).Result).Result; ///zmiana tła
            tlo = Zmiana_tla(url);
            button_Copy3.Background = tlo;
            url = oMalunek[1].Sciezka_dostepu;
            //tlo = Task.Run(() => Zmiana_tla(url).Result).Result;
            tlo = Zmiana_tla(url);
            button_Copy4.Background = tlo;
            url = oRys[1].Sciezka_dostepu;
            //tlo = Task.Run(() => Zmiana_tla(url).Result).Result;
            tlo = Zmiana_tla(url);
            button_Copy5.Background = tlo;
            url = oTatu[1].Sciezka_dostepu;
            //tlo = Task.Run(() => Zmiana_tla(url).Result).Result;
            tlo = Zmiana_tla(url);
            button_Copy6.Background = tlo;
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private async Task<string> Wyslanie(string rzezb, string zap)
        {
            try
            {
                string responseServ;

                var request = (HttpWebRequest)WebRequest.Create(rzezb);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
                {
                    streamWriter.Write(zap);
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

        //metoda asynchroniczna nie działa...
        private ImageBrush Zmiana_tla(string http)
        {

            var uri = new Uri(http, UriKind.Absolute);
            var img = new ImageBrush();
            img.ImageSource = new BitmapImage(uri);
            return img;
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

        private class Zapytanie : Obraz
        {
            public string ID_Kategorii;

            public Zapytanie(string Nazwa_obrazu, string Sciezka_dostepu, string Liczba_WOW, string ID_Kategorii)
                : base(Nazwa_obrazu, Sciezka_dostepu, Liczba_WOW)
            {
                this.ID_Kategorii = ID_Kategorii;
            }
        }

        private class Obraz
        {
            public string Nazwa_obrazu, Sciezka_dostepu, Liczba_WOW;

            public Obraz(string Nazwa_obrazu, string Sciezka_dostepu, string Liczba_WOW)
            {
                this.Nazwa_obrazu = Nazwa_obrazu;
                this.Sciezka_dostepu = Sciezka_dostepu;
                this.Liczba_WOW = Liczba_WOW;
            }
        }
    }
}
