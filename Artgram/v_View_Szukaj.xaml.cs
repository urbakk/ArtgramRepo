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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Artgram
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class v_View_Szukaj : Page
    {
        int licz;
        string linkWOW = "http://artgram.hostingpo.pl/zwieksz_wow.php",
            linkWOW_2 = "http://artgram.hostingpo.pl/zwieksz_wow_2.php",
            linkBlokada = "http://artgram.hostingpo.pl/blokada_wow.php",
            linkUsun = "http://artgram.hostingpo.pl/usun.php",
            linkZmniejsz = "http://artgram.hostingpo.pl/zmniejsz_wow.php",
            linkKontakt = "http://artgram.hostingpo.pl/kontakt.php";

        string url, doWyszukaj, ID_Obrazu;
        bool stan_ulubionego;
        string[] list;
        AppBar ap1 = new AppBar();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] lista = e.Parameter as string[];  //Pobieranie tablicy z parametrami wybranego obrazka
            list = lista;
            url = lista[3];  //Sciezka dostępu
            var uri = new Uri(url, UriKind.Absolute);
            var img = new ImageBrush();
            img.ImageSource = new BitmapImage(uri);
            button.Background = img;   
            textBlock_nazwa.Text = lista[0];  //Nazwa obrazu
            textBlock_WOW.Text = lista[2];  //Liczba WoW
            ID_Obrazu = lista[5];
            //textBlock_ID_Obrazu1.Text = ID_Obrazu;
            List<Ulubione> ListaUlubionych = new List<Ulubione>();

            //******************
            Ulubione ulub = new Ulubione(ap1.Wyslij_ID_Uz(), ID_Obrazu);
            ListaUlubionych = await Pobierz_ulubione(linkBlokada, ulub);

            if (ListaUlubionych.Count != 0) 
            {
                button_Wow.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///assets/wow-no.png")) };
                stan_ulubionego = true;
            }

            else
            {
                button_Wow.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///assets/wow.png")) };
                stan_ulubionego = false; 
            }

            //******************

            if (lista[1] == null)
            {
                textBlock_opis.Text = "Autor nie dodał opisu";
            }
            else
            {
                textBlock_opis.Text = lista[1];  //Opis obrazka
            }

            doWyszukaj = lista[4];   //Nazwa po której szukaliśmy. Potrzebna do powrotu
        }

        private async void Kontakt1_Click(object sender, RoutedEventArgs e)
        {
            string http = "http://www.facebook.com/", user, adres,
                msg = "{ \"ID_Obrazu\" : \"" + list[5] + "\"}";

            user = await Wyslanie(linkKontakt, msg);
            user = user.Replace("\t", "");
            List<Kontakt> Użytkownik = JsonConvert.DeserializeObject<List<Kontakt>>(user);

            if (Użytkownik[0].ID_Uzytkownicy == "1")
            {
                textBlock_nazwa_Copy.Text = "Obrazek systemowy. Nie można przekierować.";
            }
            else if (Użytkownik[0].ID_Uzytkownicy == "\tError in selecting")
            {
                textBlock_nazwa_Copy.Text = "Problem połączenia.";
            }
            else
            {
                adres = http + Użytkownik[0].ID_Uzytkownicy;
                var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(adres));
                if (success)
                {
                    textBlock_nazwa_Copy.Text = "Wyświetlanie.";
                }
                else
                    textBlock_nazwa_Copy.Text = "Otwarcie strony się nie powiodło.";
            }
        }

        public v_View_Szukaj()
        {
            this.InitializeComponent();
        }

        private async void button_Wow_Click(object sender, RoutedEventArgs e)
        {
            //ID_Obrazu = textBlock_ID_Obrazu1.Text;
            int liczba_wow = 0;
            string dane_polubienia, odpowiedz;
            //string ID_Obrazu = textBlock_ID_Obrazu1.Text;
            string ID_Uzytkownicy = ap1.Wyslij_ID_Uz();

            if (ID_Uzytkownicy == null)
            {
                button_Wow.Content = "Najpierw się zaloguj!";
            }

            else
            {

                Dane_WOW obiekt_wow = new Dane_WOW(ID_Uzytkownicy, ID_Obrazu);
                dane_polubienia = JsonConvert.SerializeObject(obiekt_wow);


                if (stan_ulubionego == false)
                {
                    Int32.TryParse(textBlock_WOW.Text, out liczba_wow); //potrzebna konwersja
                    liczba_wow += 1;
                    textBlock_WOW.Text = liczba_wow.ToString();     //aktualizacja liczby wow
                    button_Wow.Content = "";

                    odpowiedz = await Wyslanie(linkWOW, dane_polubienia);
                    odpowiedz = await Wyslanie(linkWOW_2, dane_polubienia);

                    button_Wow.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///assets/wow-no.png")) };
                    stan_ulubionego = true;
                }

                else
                {
                    Int32.TryParse(textBlock_WOW.Text, out liczba_wow); //potrzebna konwersja
                    liczba_wow -= 1;
                    textBlock_WOW.Text = liczba_wow.ToString();     //aktualizacja liczby wow
                    button_Wow.Content = "";

                    odpowiedz = await Wyslanie(linkUsun, dane_polubienia);
                    odpowiedz = await Wyslanie(linkZmniejsz, dane_polubienia);

                    button_Wow.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///assets/wow.png")) };
                    stan_ulubionego = false;
                }
            }
        }

        private void button_Powrot_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(v_Szukaj), doWyszukaj);
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

        private async Task<List<Ulubione>> Pobierz_ulubione(string link, Ulubione ulub)
        {
            string responseServer, ulubiona_praca;
            try
            {
                ulubiona_praca = JsonConvert.SerializeObject(ulub); //konwerter do JSONa
                responseServer = await Wyslanie(link, ulubiona_praca); //wysłanie danych do zapytania
                List<Ulubione> ListaUlubionych = JsonConvert.DeserializeObject<List<Ulubione>>(responseServer); //konwersja wyniku zapytania z JSONa do listy
                return ListaUlubionych;
            }
            catch
            {
                return null;
            }
        }

        private class Dane_WOW
        {
            public string ID_Uzytkownicy, ID_Obrazu;

            public Dane_WOW(string ID_Uzytkownicy, string ID_Obrazu)
            {
                this.ID_Uzytkownicy = ID_Uzytkownicy;
                this.ID_Obrazu = ID_Obrazu;
            }
        }

        public class Kontakt
        {
            public string ID_Uzytkownicy;

            public Kontakt(string ID_Uzytkownicy)
            {
                this.ID_Uzytkownicy = ID_Uzytkownicy;
            }
        }
    }
}
