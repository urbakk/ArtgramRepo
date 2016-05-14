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
        private string url, ID_kat, responseServer, zap,
           link = "http://artgram.hostingpo.pl/view.php";
        int gornyPrzedzial = 0, licznik = 0, licznikPoprzednie, licznikNastepne;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            licznik--;
            Zapytanie zapytanie = new Zapytanie(ID_kat);
            Zmiana_tla(zapytanie);
        }

        private void button1_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            licznik++;
            Zapytanie zapytanie = new Zapytanie(ID_kat);
            Zmiana_tla(zapytanie);
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ID_kat = e.Parameter as string;
            licznik = 0;
            Zapytanie zapytanie = new Zapytanie(ID_kat);
            Zmiana_tla(zapytanie);

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

        private async void Zmiana_tla(Zapytanie kat)
        {            
            string kategoria = kat.ID_Kategorii;
            zap = JsonConvert.SerializeObject(kat); //konwerter do JSONa
            responseServer = await Wyslanie(link, zap); //wysłanie danych do zapytania
            List<Obraz> oKat = JsonConvert.DeserializeObject<List<Obraz>>(responseServer); //konwersja wyniku zapytania z JSONa do listy
            gornyPrzedzial = oKat.Count;  //pobieranie wielkosci List<Obraz>

            licznikNastepne = licznik;
            licznikPoprzednie = licznik;
            url = oKat[licznik].Sciezka_dostepu; //wyciągnięcie ścieżki do obrazu glownego
            var uri1 = new Uri(url, UriKind.Absolute);
            var img1 = new ImageBrush();
            img1.ImageSource = new BitmapImage(uri1);
            button.Background = img1;
            textBlock_nazwa.Text = oKat[licznik].Nazwa_obrazu;
            textBlock_WOW.Text = oKat[licznik].Liczba_WOW;
            if (oKat[licznik].Opis_obrazu == null)
            {
                textBlock_opis.Text = "Autor nie dodał opisu";
            }
            else
            {
                textBlock_opis.Text = oKat[licznik].Opis_obrazu;
            }

            licznikNastepne++;
            licznikPoprzednie--;

            if (licznikNastepne == gornyPrzedzial)  //Warunek obsługujący następne zdjęcia
            {
                button1_Copy.Visibility = Visibility.Collapsed;
                textBlock_Copy.Visibility = Visibility.Collapsed;
            }
            else
            {
                button1_Copy.Visibility = Visibility.Visible;
                textBlock_Copy.Visibility = Visibility.Visible;

                url = oKat[licznikNastepne].Sciezka_dostepu; //wyciągnięcie ścieżki do obrazu glownego
                var uri2 = new Uri(url, UriKind.Absolute);
                var img2 = new ImageBrush();
                img2.ImageSource = new BitmapImage(uri2);
                button1_Copy.Background = img2;
            }


            if (licznikPoprzednie < 0)  //Warunek obsługujący poprzednie zdjęcia
            {
                button1.Visibility = Visibility.Collapsed;
                textBlock.Visibility = Visibility.Collapsed;                
            }
            else
            {
                button1.Visibility = Visibility.Visible;
                textBlock.Visibility = Visibility.Visible;

                url = oKat[licznikPoprzednie].Sciezka_dostepu; //wyciągnięcie ścieżki do obrazu glownego
                var uri2 = new Uri(url, UriKind.Absolute);
                var img2 = new ImageBrush();
                img2.ImageSource = new BitmapImage(uri2);
                button1.Background = img2;
            }
        }
        
        public View()
        {
            this.InitializeComponent();            
        }
    }
}
