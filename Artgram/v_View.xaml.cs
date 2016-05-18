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
            linkNowe = "http://artgram.hostingpo.pl/nowe.php";
        int licznik = 0, gornyPrzedzial, licznikNastepne, licznikPoprzednie;
        List<Obraz> ListaObrazow = new List<Obraz>();

        private void button1_Click(object sender, RoutedEventArgs e) //Poprzedni obraz
        {
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

            if (!IdKategorii.Equals("najpopularniejsze") && !IdKategorii.Equals("nowe"))
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
                
        private void UstawObraz(Obraz obraz, string ktore)
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

        public View()
        {
            this.InitializeComponent();            
        }
    }
}
