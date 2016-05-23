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
using Windows.UI.Core;
using Windows.Security.Authentication.Web;
using winsdkfb;
using winsdkfb.Graph;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
namespace Artgram
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>


    public sealed partial class MainPage : Page
    {
        //int LoginStatus;    //Do sprawdzania stanu logowania (ma być w tym miejscu?) :O
        private string UrlRzezba, UrlMalarstwo, UrlRysunek, UrlTatuaze, UrlNajpopularniejsze, UrlNowe, ID_uzytkownika, UrlMoje,
            link = "http://artgram.hostingpo.pl/login.php",
            linkNajpopularniejsze = "http://artgram.hostingpo.pl/najpopularniejsze.php",
            linkNowe = "http://artgram.hostingpo.pl/nowe.php";
        List<Obraz> ListaObrazow = new List<Obraz>();
        int losowo, Przedzial;

        AppBar ap1 = new AppBar();

        public MainPage()
        {
            this.InitializeComponent();

            Zapytanie rzezba = new Zapytanie("2");
            UrlRzezba = Task.Run(() => Pobierz_url(link, rzezba).Result).Result;
            if(UrlRzezba == null)
            {
                Obsluga_bledu();
            }
            else
            {
                button_Copy3.Background = Zmiana_tla(UrlRzezba);
            }           

            Zapytanie malarstwo = new Zapytanie("3");
            UrlMalarstwo = Task.Run(() => Pobierz_url(link, malarstwo).Result).Result;
            button_Copy4.Background = Zmiana_tla(UrlMalarstwo);

            Zapytanie rysunek = new Zapytanie("4");
            UrlRysunek = Task.Run(() => Pobierz_url(link, rysunek).Result).Result;
            button_Copy5.Background = Zmiana_tla(UrlRysunek);

            Zapytanie tatuaze = new Zapytanie("5");
            UrlTatuaze = Task.Run(() => Pobierz_url(link, tatuaze).Result).Result;
            button_Copy6.Background = Zmiana_tla(UrlTatuaze);

            UrlNajpopularniejsze = Task.Run(() => Pobierz_url(linkNajpopularniejsze, tatuaze).Result).Result;  //Pobierz_url(linkNajpopularniejsze, tatuaze)- tatuaze nie są wykorzystywane, ale coś musialam przeslac zeby zadzialalo
            button.Background = Zmiana_tla(UrlNajpopularniejsze);

            UrlNowe = Task.Run(() => Pobierz_url(linkNowe, tatuaze).Result).Result;  // Pobierz_url(linkNowe, tatuaze)- tatuaze nie są wykorzystywane, ale coś musialam przeslac zeby zadzialalo
            button_Copy.Background = Zmiana_tla(UrlNowe);

            ID_uzytkownika = ap1.Wyslij_ID_Uz();
            if (ID_uzytkownika != null)
            {
                button_Copy1.IsEnabled = true;
                button_Copy2.IsEnabled = true;

                //string ID_Uzytkownicy = ap1.Wyslij_ID_Uz();
                Szukaj_moje szukaj_moje = new Szukaj_moje("1");
                ListaObrazow = Task.Run(() => Pobierz_moje_obrazy(szukaj_moje).Result).Result;
                Przedzial = ListaObrazow.Count;  //pobieranie wielkosci List<Obraz>
                Random random = new Random();
                losowo = random.Next(0, Przedzial);
                UrlMoje = ListaObrazow[losowo].Sciezka_dostepu;
                button_Copy2.Background = Zmiana_tla(UrlMoje);
            }
            else
            {
                button_Copy1.IsEnabled = false;
                button_Copy2.IsEnabled = false;
            }
        }

        private async Task<string> Wyslanie(string link, string zap)
        {

            try
            {
                string responseServ;

                var request = (HttpWebRequest)WebRequest.Create(link);
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

        private async Task<string> Pobierz_url(string link, Zapytanie kat)
        {
            string url, responseServer, zap;
            int gornyPrzedzial = 0, losowa = 0;

            try
            {
                zap = JsonConvert.SerializeObject(kat); //konwerter do JSONa
                responseServer = await Wyslanie(link, zap); //wysłanie danych do zapytania
                List<Obraz> ListaObrazow = JsonConvert.DeserializeObject<List<Obraz>>(responseServer); //konwersja wyniku zapytania z JSONa do listy
                gornyPrzedzial = ListaObrazow.Count;  //pobieranie wielkosci List<Obraz>
                Random random = new Random();
                losowa = random.Next(0, gornyPrzedzial);

                url = ListaObrazow[losowa].Sciezka_dostepu; //wyciągnięcie losowej ścieżki do obrazu
                return url;
            }
            catch
            {
                return null;
            }
        }

        private ImageBrush Zmiana_tla(string url)
        {
            if(url == null)
            {
                return null;
            }
            else
            {
                var uri = new Uri(url, UriKind.Absolute);
                var img = new ImageBrush();
                img.ImageSource = new BitmapImage(uri);

                return img;
            }           
        }

        private void Obsluga_bledu()
        {
            button.Visibility = Visibility.Collapsed;
            button_Copy.Visibility = Visibility.Collapsed;
            button_Copy1.Visibility = Visibility.Collapsed;
            button_Copy2.Visibility = Visibility.Collapsed;
            button_Copy3.Visibility = Visibility.Collapsed;
            button_Copy4.Visibility = Visibility.Collapsed;
            button_Copy5.Visibility = Visibility.Collapsed;
            button_Copy6.Visibility = Visibility.Collapsed;

            textBlock_Copy.Visibility = Visibility.Collapsed;
            textBlock_Copy1.Visibility = Visibility.Collapsed;
            textBlock_Copy2.Visibility = Visibility.Collapsed;
            textBlock_Copy3.Visibility = Visibility.Collapsed;
            textBlock_Copy4.Visibility = Visibility.Collapsed;
            textBlock_Copy5.Visibility = Visibility.Collapsed;
            textBlock_Copy6.Visibility = Visibility.Collapsed;
            textBlock_Copy7.Visibility = Visibility.Collapsed;

            textBlock_Blad.Visibility = Visibility.Visible;
        }

        private void button_Click(object sender, RoutedEventArgs e) //Najpopularniejsze
        {
            string[] Lista = { UrlNajpopularniejsze, "najpopularniejsze" };
            this.Frame.Navigate(typeof(View), Lista);
        }

        private void button_Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Add));
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e) //Nowe
        {
            string[] Lista = { UrlNowe, "nowe" };
            this.Frame.Navigate(typeof(View), Lista);
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View));
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(v_Szukaj), "Moje obrazy");
        }

        private void button_Copy3_Click(object sender, RoutedEventArgs e)
        {
            string[] Lista = { UrlRzezba, "2" };
            this.Frame.Navigate(typeof(View), Lista);
        }

        private void button_Copy4_Click(object sender, RoutedEventArgs e)
        {
            string[] Lista = { UrlMalarstwo, "3" };
            this.Frame.Navigate(typeof(View), Lista);
        }

        private void button_Copy5_Click(object sender, RoutedEventArgs e)
        {
            string[] Lista = { UrlRysunek, "4" };
            this.Frame.Navigate(typeof(View), Lista);
        }

        private void button_Copy6_Click(object sender, RoutedEventArgs e)
        {
            string[] Lista = { UrlTatuaze, "5" };
            this.Frame.Navigate(typeof(View), Lista);
        }

        private void button_Logo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async Task<List<Obraz>> Pobierz_moje_obrazy(Szukaj_moje nazwa)
        {
            string responseServer, zap, link = "http://artgram.hostingpo.pl/moje_obrazy.php";
            try
            {
                zap = JsonConvert.SerializeObject(nazwa); //konwerter do JSONa
                responseServer = await Wyslanie(link, zap); //wysłanie danych do zapytania
                List<Obraz> ListaObrazow = JsonConvert.DeserializeObject<List<Obraz>>(responseServer); //konwersja wyniku zapytania z JSONa do listy
                return ListaObrazow;
            }
            catch
            {
                return null;
            }
        }

    }
}

