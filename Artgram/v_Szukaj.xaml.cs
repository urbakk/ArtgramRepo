using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Artgram
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class v_Szukaj : Page
    {
        List<Obraz> ListaObrazow = new List<Obraz>();
        int gornyPrzedzial, licznik = 0;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string Nazwa_obrazu = e.Parameter as string;
            textBlock_Szukaj.Text = textBlock_Szukaj.Text + Nazwa_obrazu;

            Szukaj szukaj = new Szukaj(Nazwa_obrazu);
            ListaObrazow = Task.Run(() => Pobierz_obrazy(szukaj).Result).Result;
            gornyPrzedzial = ListaObrazow.Count();
                      
            if(gornyPrzedzial == 0)
            {
                textBlock_Uwaga.Visibility = Visibility.Visible;
               textBlock_Uwaga.Text = "Nie znaleziono wyników";
            }
            else 
            {
                UstawObraz(licznik);
            }        
            
        }

        private async Task<string> Wyslanie(string zap)
        {
            string link = "http://artgram.hostingpo.pl/szukaj.php";

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

        private async Task<List<Obraz>> Pobierz_obrazy(Szukaj nazwa)
        {
            string responseServer, zap;
            try
            {
                zap = JsonConvert.SerializeObject(nazwa); //konwerter do JSONa
                responseServer = await Wyslanie(zap); //wysłanie danych do zapytania
                List<Obraz> ListaObrazow = JsonConvert.DeserializeObject<List<Obraz>>(responseServer); //konwersja wyniku zapytania z JSONa do listy
                return ListaObrazow;
            }
            catch
            {
                return null;
            }
        }

        private ImageBrush PobierzObraz(Obraz obraz)
        {
            string url;

            url = obraz.Sciezka_dostepu;
            var uri = new Uri(url, UriKind.Absolute);
            var img = new ImageBrush();
            img.ImageSource = new BitmapImage(uri);
            return img;                            
        }

        private void UstawObraz(int licznik)
        {
            button0.Visibility = Visibility.Visible;
            textBlock0.Visibility = Visibility.Visible;
            button0.Background = PobierzObraz(ListaObrazow[licznik]);
            textBlock0.Text = ListaObrazow[licznik].Nazwa_obrazu;
            licznik++;

            if (licznik < gornyPrzedzial)
            {
                button1.Visibility = Visibility.Visible;
                textBlock1.Visibility = Visibility.Visible;
                button1.Background = PobierzObraz(ListaObrazow[licznik]);
                textBlock1.Text = ListaObrazow[licznik].Nazwa_obrazu;
                licznik++;
            }

            if (licznik < gornyPrzedzial)
            {
                button2.Visibility = Visibility.Visible;
                textBlock2.Visibility = Visibility.Visible;
                button2.Background = PobierzObraz(ListaObrazow[licznik]);
                textBlock2.Text = ListaObrazow[licznik].Nazwa_obrazu;
                licznik++;
            }

            if (licznik < gornyPrzedzial)
            {
                button3.Visibility = Visibility.Visible;
                textBlock3.Visibility = Visibility.Visible;
                button3.Background = PobierzObraz(ListaObrazow[licznik]);
                textBlock3.Text = ListaObrazow[licznik].Nazwa_obrazu;
                licznik++;
            }

            if (licznik < gornyPrzedzial)
            {
                button_Dalej.Visibility = Visibility.Visible;
            }
        }

        private void button_Dalej_Click(object sender, RoutedEventArgs e)
        {
            button_Dalej.Visibility = Visibility.Collapsed;
            button0.Visibility = Visibility.Collapsed;
            textBlock0.Visibility = Visibility.Collapsed;
            button1.Visibility = Visibility.Collapsed;
            textBlock1.Visibility = Visibility.Collapsed;
            button2.Visibility = Visibility.Collapsed;
            textBlock2.Visibility = Visibility.Collapsed;
            button3.Visibility = Visibility.Collapsed;
            textBlock3.Visibility = Visibility.Collapsed;

            licznik = licznik + 4;
            UstawObraz(licznik);
        }

        public v_Szukaj()
        {
            this.InitializeComponent();
        }        

        class Szukaj
        {
            public string Nazwa_obrazu;

            public Szukaj(string Nazwa_obrazu)
            {
                this.Nazwa_obrazu = Nazwa_obrazu;
            }
        }
    }
}
