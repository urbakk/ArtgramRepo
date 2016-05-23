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
        int gornyPrzedzial, licznik = 0, obraz0, obraz1, obraz2, obraz3;
        string Nazwa_obrazu, Pomocnicza_nazwa_obrazu;
        AppBar ap1 = new AppBar();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Nazwa_obrazu = e.Parameter as string;
            Pomocnicza_nazwa_obrazu = Nazwa_obrazu;
            if (Nazwa_obrazu.Equals("Moje obrazy"))
            {
                string ID_Uzytkownicy = ap1.Wyslij_ID_Uz();
                textBlock_Uwaga.Visibility = Visibility.Visible;
                textBlock_Uwaga.Text = ID_Uzytkownicy;
                Szukaj_moje szukaj_moje = new Szukaj_moje("1");
                ListaObrazow = Task.Run(() => Pobierz_moje_obrazy(szukaj_moje).Result).Result;
                gornyPrzedzial = ListaObrazow.Count();
            }
            else
            {
                Pomocnicza_nazwa_obrazu = Pomocnicza_nazwa_obrazu.Remove(0, 6);
                textBlock_Szukaj.Text = textBlock_Szukaj.Text + Pomocnicza_nazwa_obrazu;

                Szukaj szukaj = new Szukaj(Pomocnicza_nazwa_obrazu);
                ListaObrazow = Task.Run(() => Pobierz_obrazy(szukaj).Result).Result;
                gornyPrzedzial = ListaObrazow.Count();
            }


            if (gornyPrzedzial == 0)
            {
                textBlock_Uwaga.Visibility = Visibility.Visible;
                textBlock_Uwaga.Text = "Nie znaleziono wyników";
            }
            else
            {
                obraz0 = licznik;                    //obraz0, obraz1.. potrzebne do przeslania numeru zdjecia do wyswietlenia jeg widoku
                UstawObraz(licznik, 0);              //Ustawi obraz na pierwszej pozycji
                licznik++;
                obraz1 = licznik;
                UstawObraz(licznik, 1);              //Ustawia obraz na drugiej pozycji itd.
                licznik++;
                obraz2 = licznik;
                UstawObraz(licznik, 2);
                licznik++;
                obraz3 = licznik;
                UstawObraz(licznik, 3);
                licznik++;

                if (licznik < gornyPrzedzial)
                {
                    button_Dalej.Visibility = Visibility.Visible;
                }
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

        private async Task<List<Obraz>> Pobierz_obrazy(Szukaj nazwa)
        {
            string responseServer, zap, link = "http://artgram.hostingpo.pl/szukaj.php"; ;
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

        private ImageBrush PobierzObraz(Obraz obraz)
        {
            string url;

            url = obraz.Sciezka_dostepu;
            var uri = new Uri(url, UriKind.Absolute);
            var img = new ImageBrush();
            img.ImageSource = new BitmapImage(uri);
            return img;
        }

        private void UstawObraz(int licznik, int miejsce)
        {
            if (miejsce == 0)
            {
                button0.Visibility = Visibility.Visible;
                textBlock0.Visibility = Visibility.Visible;
                button0.Background = PobierzObraz(ListaObrazow[licznik]);
                textBlock0.Text = ListaObrazow[licznik].Nazwa_obrazu;
            }

            if (licznik < gornyPrzedzial && miejsce == 1)
            {
                button1.Visibility = Visibility.Visible;
                textBlock1.Visibility = Visibility.Visible;
                button1.Background = PobierzObraz(ListaObrazow[licznik]);
                textBlock1.Text = ListaObrazow[licznik].Nazwa_obrazu;
            }

            if (licznik < gornyPrzedzial && miejsce == 2)
            {
                button2.Visibility = Visibility.Visible;
                textBlock2.Visibility = Visibility.Visible;
                button2.Background = PobierzObraz(ListaObrazow[licznik]);
                textBlock2.Text = ListaObrazow[licznik].Nazwa_obrazu;
            }

            if (licznik < gornyPrzedzial && miejsce == 3)
            {
                button3.Visibility = Visibility.Visible;
                textBlock3.Visibility = Visibility.Visible;
                button3.Background = PobierzObraz(ListaObrazow[licznik]);
                textBlock3.Text = ListaObrazow[licznik].Nazwa_obrazu;
            }


        }

        private void button_Dalej_Click(object sender, RoutedEventArgs e)
        {
            button_Cofnij.Visibility = Visibility.Visible;
            button_Dalej.Visibility = Visibility.Collapsed;
            button0.Visibility = Visibility.Collapsed;
            textBlock0.Visibility = Visibility.Collapsed;
            button1.Visibility = Visibility.Collapsed;
            textBlock1.Visibility = Visibility.Collapsed;
            button2.Visibility = Visibility.Collapsed;
            textBlock2.Visibility = Visibility.Collapsed;
            button3.Visibility = Visibility.Collapsed;
            textBlock3.Visibility = Visibility.Collapsed;

            obraz0 = licznik;
            UstawObraz(licznik, 0);
            licznik++;
            obraz1 = licznik;
            UstawObraz(licznik, 1);
            licznik++;
            obraz2 = licznik;
            UstawObraz(licznik, 2);
            licznik++;
            obraz3 = licznik;
            UstawObraz(licznik, 3);
            licznik++;

            if (licznik < gornyPrzedzial)
            {
                button_Dalej.Visibility = Visibility.Visible;
            }
        }

        private void button_Cofnij_Click(object sender, RoutedEventArgs e)
        {
            licznik = licznik - 8;
            if (licznik == 0)
            {
                button_Cofnij.Visibility = Visibility.Collapsed;
            }
            obraz0 = licznik;
            UstawObraz(licznik, 0);
            licznik++;
            obraz1 = licznik;
            UstawObraz(licznik, 1);
            licznik++;
            obraz2 = licznik;
            UstawObraz(licznik, 2);
            licznik++;
            obraz3 = licznik;
            UstawObraz(licznik, 3);
            licznik++;
            button_Dalej.Visibility = Visibility.Visible;
        }

        private void button0_Click(object sender, RoutedEventArgs e)
        {
            if (Nazwa_obrazu.Equals("Moje obrazy"))
            {
                string[] lista = { ListaObrazow[obraz0].Nazwa_obrazu, ListaObrazow[obraz0].Opis_obrazu, ListaObrazow[obraz0].Liczba_WOW, ListaObrazow[obraz0].Sciezka_dostepu, Nazwa_obrazu };
                this.Frame.Navigate(typeof(v_Edycja), lista);
            }
            else
            {
                string[] lista = { ListaObrazow[obraz0].Nazwa_obrazu, ListaObrazow[obraz0].Opis_obrazu, ListaObrazow[obraz0].Liczba_WOW, ListaObrazow[obraz0].Sciezka_dostepu, Nazwa_obrazu };
                this.Frame.Navigate(typeof(v_View_Szukaj), lista);
            }                
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (Nazwa_obrazu.Equals("Moje obrazy"))
            {
                string[] lista = { ListaObrazow[obraz1].Nazwa_obrazu, ListaObrazow[obraz1].Opis_obrazu, ListaObrazow[obraz1].Liczba_WOW, ListaObrazow[obraz1].Sciezka_dostepu, Nazwa_obrazu };
                this.Frame.Navigate(typeof(v_Edycja), lista);
            }
            else
            {
                string[] lista = { ListaObrazow[obraz1].Nazwa_obrazu, ListaObrazow[obraz1].Opis_obrazu, ListaObrazow[obraz1].Liczba_WOW, ListaObrazow[obraz1].Sciezka_dostepu, Nazwa_obrazu };
                this.Frame.Navigate(typeof(v_View_Szukaj), lista);
            }            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (Nazwa_obrazu.Equals("Moje obrazy"))
            {
                string[] lista = { ListaObrazow[obraz2].Nazwa_obrazu, ListaObrazow[obraz2].Opis_obrazu, ListaObrazow[obraz2].Liczba_WOW, ListaObrazow[obraz2].Sciezka_dostepu, Nazwa_obrazu };
                this.Frame.Navigate(typeof(v_Edycja), lista);
            }
            else
            {
                string[] lista = { ListaObrazow[obraz2].Nazwa_obrazu, ListaObrazow[obraz2].Opis_obrazu, ListaObrazow[obraz2].Liczba_WOW, ListaObrazow[obraz2].Sciezka_dostepu, Nazwa_obrazu };
                this.Frame.Navigate(typeof(v_View_Szukaj), lista);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (Nazwa_obrazu.Equals("Moje obrazy"))
            {
                string[] lista = { ListaObrazow[obraz3].Nazwa_obrazu, ListaObrazow[obraz3].Opis_obrazu, ListaObrazow[obraz3].Liczba_WOW, ListaObrazow[obraz3].Sciezka_dostepu, Nazwa_obrazu };
                this.Frame.Navigate(typeof(v_Edycja), lista);
            }
            else
            {
                string[] lista = { ListaObrazow[obraz3].Nazwa_obrazu, ListaObrazow[obraz3].Opis_obrazu, ListaObrazow[obraz3].Liczba_WOW, ListaObrazow[obraz3].Sciezka_dostepu, Nazwa_obrazu };
                this.Frame.Navigate(typeof(v_View_Szukaj), lista);
            }
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
        class Szukaj_moje
        {
            public string ID_Uzytkownicy;

            public Szukaj_moje(string ID_Uzytkownicy)
            {
                this.ID_Uzytkownicy = ID_Uzytkownicy;
            }

        }
    }
}
