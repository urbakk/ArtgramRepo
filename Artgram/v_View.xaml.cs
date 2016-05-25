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
    public sealed partial class View : Page
    {
        string UrlObrazka, IdKategorii,
            link = "http://artgram.hostingpo.pl/login.php",
            linkNajpopularniejsze = "http://artgram.hostingpo.pl/najpopularniejsze.php",
            linkNowe = "http://artgram.hostingpo.pl/nowe.php",
            linkWOW = "http://artgram.hostingpo.pl/zwieksz_wow.php",
            linkWOW_2 = "http://artgram.hostingpo.pl/zwieksz_wow_2.php",
            linkBlokada = "http://artgram.hostingpo.pl/blokada_wow.php",
            linkUsun = "http://artgram.hostingpo.pl/usun.php",
            linkZmniejsz = "http://artgram.hostingpo.pl/zmniejsz_wow.php",
            linkUlubione = "http://artgram.hostingpo.pl/ulubione.php",
            linkZglos = "http://artgram.hostingpo.pl/zglos.php";
        int licznik = 0, gornyPrzedzial, licznikNastepne, licznikPoprzednie;
        bool stan_ulubionego; 
        List<Obraz> ListaObrazow = new List<Obraz>();
        List<Ulubione> ListaUlubionych = new List<Ulubione>();

        AppBar ap1 = new AppBar(); //potrzebne do uzyskania ID_Uzytkownika

        private async void button_Report_Click(object sender, RoutedEventArgs e)
        {
            string odpowiedz, dane;
            Zglos report = new Zglos(ListaObrazow[licznik].ID_Obrazu, ListaObrazow[licznik].Nazwa_obrazu);
            dane = JsonConvert.SerializeObject(report);

            odpowiedz = await Wyslanie(linkZglos, dane);

            if (odpowiedz == "Wyslano")
            {
                textBlock1.Text = "Obraz został zgłoszony.";
                button_Report.IsEnabled = false;
            }
            else
            {
                textBlock1.Text = "Napotkano problem ze zgłoszniem.";
            }
        }

        private async void button_Wow_Click(object sender, RoutedEventArgs e)
        {

            int liczba_wow = 0;
            string dane_polubienia, odpowiedz;
            string ID_Obrazu = textBlock_ID_Obrazu.Text;
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

        private void button1_Click(object sender, RoutedEventArgs e) //Poprzedni obraz
        {
            textBlock1.Text = ""; //resetowanie okienka powiadomień

            UstawObraz(ListaObrazow[licznikPoprzednie], "glowne"); //Ustawianie glownego obrazu
            button1_Copy.Visibility = Visibility.Visible;
            textBlock_Copy.Visibility = Visibility.Visible;
            
            if (licznikPoprzednie == gornyPrzedzial - 1 && licznik == gornyPrzedzial - 1)
            {
                licznikNastepne = 0;
                UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
                button1.Visibility = Visibility.Collapsed;
                textBlock.Visibility = Visibility.Collapsed;
            }
            else if (licznikPoprzednie ==  licznik)
            {
                licznikNastepne = licznikPoprzednie + 1;
                UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
                button1.Visibility = Visibility.Collapsed;
                textBlock.Visibility = Visibility.Collapsed;
            }            
            else if(licznikPoprzednie == 0)
            {
                licznikNastepne = licznikPoprzednie + 1;
                UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
                licznikPoprzednie = gornyPrzedzial - 1;
                UstawObraz(ListaObrazow[licznikPoprzednie], "poprzednie"); //Ustawianie poprzedniego obrazu
            }
            else if(licznikPoprzednie == gornyPrzedzial - 1)
            {
                licznikNastepne = 0;
                UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
                licznikPoprzednie--;
                UstawObraz(ListaObrazow[licznikPoprzednie], "poprzednie"); //Ustawianie poprzedniego obrazu
            }
            else
            {
                licznikNastepne = licznikPoprzednie + 1;
                UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
                licznikPoprzednie--;
                UstawObraz(ListaObrazow[licznikPoprzednie], "poprzednie"); //Ustawianie poprzedniego obrazu
            }
            
        }

        private void button1_Copy_Click_1(object sender, RoutedEventArgs e) //Następny obraz
        {
            textBlock1.Text = ""; // resetowanie okienka powiadomień

            UstawObraz(ListaObrazow[licznikNastepne], "glowne"); //Ustawianie glownego obrazu
            button1.Visibility = Visibility.Visible;
            textBlock.Visibility = Visibility.Visible;

            if (licznikNastepne + 1 == licznik && licznikNastepne==0)
            {                
                licznikPoprzednie = gornyPrzedzial - 1;
                UstawObraz(ListaObrazow[licznikPoprzednie], "poprzednie"); //Ustawianie następnego obrazu
                button1_Copy.Visibility = Visibility.Collapsed;
                textBlock_Copy.Visibility = Visibility.Collapsed;
            }
            else if(licznikNastepne + 1 == licznik)
            {
                licznikPoprzednie = licznikNastepne - 1;
                UstawObraz(ListaObrazow[licznikPoprzednie], "poprzednie"); //Ustawianie następnego obrazu
                button1_Copy.Visibility = Visibility.Collapsed;
                textBlock_Copy.Visibility = Visibility.Collapsed;
            }             
            else if (licznikNastepne == 0)
            {
                licznikPoprzednie = gornyPrzedzial - 1;
                UstawObraz(ListaObrazow[licznikPoprzednie], "poprzednie"); //Ustawianie poprzedniego obrazu
                licznikNastepne++;
                UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu                
            }
            else if (licznikNastepne == gornyPrzedzial - 1 && licznik == 0)
            {
                licznikPoprzednie = licznikNastepne - 1;
                UstawObraz(ListaObrazow[licznikPoprzednie], "poprzednie"); //Ustawianie następnego obrazu
                button1_Copy.Visibility = Visibility.Collapsed;
                textBlock_Copy.Visibility = Visibility.Collapsed;
            }
            else if(licznikNastepne == gornyPrzedzial - 1)
            {
                licznikPoprzednie = licznikNastepne - 1;
                UstawObraz(ListaObrazow[licznikPoprzednie], "poprzednie"); //Ustawianie następnego obrazu
                licznikNastepne = 0;
                UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
            }
            else
            {
                licznikPoprzednie = licznikNastepne - 1;
                UstawObraz(ListaObrazow[licznikPoprzednie], "poprzednie"); //Ustawianie następnego obrazu
                licznikNastepne++;
                UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu                
            }            
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] Parametry = e.Parameter as string[];
            UrlObrazka = Parametry[0];
            IdKategorii = Parametry[1];

            if (!IdKategorii.Equals("najpopularniejsze") && !IdKategorii.Equals("nowe") && !IdKategorii.Equals("Ulubione"))
            {
                Zapytanie zapytanie = new Zapytanie(IdKategorii);
                ListaObrazow = Task.Run(() => Pobierz_obrazy(link, zapytanie).Result).Result; //Pobieranie listy obrazów z danej kategorii
                
                    gornyPrzedzial = ListaObrazow.Count();

                    while (licznik < gornyPrzedzial)
                    {
                        if (ListaObrazow[licznik].Sciezka_dostepu == UrlObrazka)
                        {
                            licznikNastepne = licznik + 1;
                            licznikPoprzednie = licznik;
                            if (licznikNastepne == gornyPrzedzial)
                            {
                                licznikNastepne = 0;
                            }
                            UstawObraz(ListaObrazow[licznik], "glowne"); //Ustawianie głównego obrazu
                            UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
                            break;
                        }

                        licznik++;
                    }
                
            }
            else if (IdKategorii.Equals("najpopularniejsze"))
            {
                Zapytanie zapytanie = new Zapytanie("1");  //liczba w tym przypadku nie ma znaczenia, ale musi być
                ListaObrazow = Task.Run(() => Pobierz_obrazy(linkNajpopularniejsze, zapytanie).Result).Result; //Pobieranie listy obrazów z danej kategorii

                gornyPrzedzial = ListaObrazow.Count();

                    while (licznik < gornyPrzedzial)
                    {
                        if (ListaObrazow[licznik].Sciezka_dostepu == UrlObrazka)
                        {
                            licznikNastepne = licznik + 1;
                            licznikPoprzednie = licznik;
                            if (licznikNastepne == gornyPrzedzial)
                            {
                                licznikNastepne = 0;
                            }
                            UstawObraz(ListaObrazow[licznik], "glowne"); //Ustawianie głównego obrazu
                            UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
                            break;
                        }

                        licznik++;
                    }
                
            }
            else if (IdKategorii.Equals("Ulubione"))
            {
                Szukaj_moje szukaj_ulubione = new Szukaj_moje(ap1.Wyslij_ID_Uz());
                ListaObrazow = Task.Run(() => Pobierz_obrazy_ulubione(linkUlubione, szukaj_ulubione).Result).Result;

                if (ListaObrazow != null)
                {
                    gornyPrzedzial = ListaObrazow.Count();
                    while (licznik < gornyPrzedzial)
                    {
                        if (ListaObrazow[licznik].Sciezka_dostepu == UrlObrazka)
                        {
                            licznikNastepne = licznik + 1;
                            licznikPoprzednie = licznik;
                            if (licznikNastepne == gornyPrzedzial)
                            {
                                licznikNastepne = 0;
                            }
                            UstawObraz(ListaObrazow[licznik], "glowne"); //Ustawianie głównego obrazu
                            UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
                            break;
                        }

                        licznik++;
                    }
                }
                else
                {
                    button.Visibility = Visibility.Collapsed;
                    textBlock_nazwa.Text = "Niestety nie posiadasz ulubionych zdjęć. Kliknij opcję WOW przy obrazku, który Ci się podoba, aby dodać go do ulubionych.";
                    textBlock_WOW.Visibility = Visibility.Collapsed;
                    textBlock_ID_Obrazu.Visibility = Visibility.Collapsed;
                    textBlock_opis.Visibility = Visibility.Collapsed;
                    button1_Copy.Visibility = Visibility.Collapsed;
                    textBlock_Copy.Visibility = Visibility.Collapsed;
                    button_Report.Visibility = Visibility.Collapsed;
                    button_Contact.Visibility = Visibility.Collapsed;
                    button_Wow.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                Zapytanie zapytanie = new Zapytanie("1");  //liczba w tym przypadku nie ma znaczenia, ale musi być
                ListaObrazow = Task.Run(() => Pobierz_obrazy(linkNowe, zapytanie).Result).Result; //Pobieranie listy obrazów z danej kategorii                              
                gornyPrzedzial = ListaObrazow.Count();

                    while (licznik < gornyPrzedzial)
                    {
                        if (ListaObrazow[licznik].Sciezka_dostepu == UrlObrazka)
                        {
                            licznikNastepne = licznik + 1;
                            licznikPoprzednie = licznik;
                            if (licznikNastepne == gornyPrzedzial)
                            {
                                licznikNastepne = 0;
                            }
                            UstawObraz(ListaObrazow[licznik], "glowne"); //Ustawianie głównego obrazu
                            UstawObraz(ListaObrazow[licznikNastepne], "nastepne"); //Ustawianie następnego obrazu
                            break;
                        }

                        licznik++;
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

        private async Task<List<Obraz>> Pobierz_obrazy(string link, Zapytanie kat)
        {
            string responseServer, zap;
            try
            {
                zap = JsonConvert.SerializeObject(kat); //konwerter do JSONa
                responseServer = await Wyslanie(link, zap); //wysłanie danych do zapytania
                List<Obraz> ListaObrazow = JsonConvert.DeserializeObject<List<Obraz>>(responseServer); //konwersja wyniku zapytania z JSONa do listy
                return ListaObrazow;
            }
            catch
            {
                return null;
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

        private async void UstawObraz(Obraz obraz, string ktore)
        {
            string url;

            if (ktore.Equals("glowne"))
            {
                //Ustawianie glownego obrazu
                url = obraz.Sciezka_dostepu;
                var uri = new Uri(url, UriKind.Absolute);
                var img = new ImageBrush();
                img.ImageSource = new BitmapImage(uri);
                button.Background = img;
                textBlock_nazwa.Text = obraz.Nazwa_obrazu;
                textBlock_WOW.Text = obraz.Liczba_WOW;
                textBlock_ID_Obrazu.Text = obraz.ID_Obrazu.ToString();

                //******************
                Ulubione ulub = new Ulubione(ap1.Wyslij_ID_Uz(), obraz.ID_Obrazu.ToString());
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

                if (obraz.Opis_obrazu == null)
                {
                    textBlock_opis.Text = "Autor nie dodał opisu";

                }
                else
                {
                    textBlock_opis.Text = obraz.Opis_obrazu;
                }
            }
            else if (ktore.Equals("nastepne"))
            {              
                url = obraz.Sciezka_dostepu;
                var uri = new Uri(url, UriKind.Absolute);
                var img = new ImageBrush();
                img.ImageSource = new BitmapImage(uri);
                button1_Copy.Background = img;
            }
            else
            {               
                url = obraz.Sciezka_dostepu;
                var uri = new Uri(url, UriKind.Absolute);
                var img = new ImageBrush();
                img.ImageSource = new BitmapImage(uri);
                button1.Background = img;
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

        private async Task<List<Obraz>> Pobierz_obrazy_ulubione(string link, Szukaj_moje nazwa)
        {
            string responseServer, zap;
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

    public class Zglos
        {
            string ID_Obrazu, Nazwa_obrazu;

            public Zglos (string ID_Obrazu, string Nazwa_obrazu)
            {
                this.ID_Obrazu = ID_Obrazu;
                this.Nazwa_obrazu = Nazwa_obrazu;
            }
        }

    public View()
        {
            this.InitializeComponent();
            textBlock1.Text = "";       
        }
    }
}
