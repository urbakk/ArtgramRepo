using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Artgram
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Add : Page
    {
        private StorageFile plik;
        private ulong maxFile = 2 * 1024 * 1024;

        public Add()
        {
            this.InitializeComponent();
            textBlock.Text = "";

            var opcje = new ObservableCollection<string>(); //utworzenie kolekcji z opcjami wyboru kategorii
            var item = ""; //proces uzupełniania kolekcji
            opcje.Add(item);
            opcje.Add("Rzeźba");
            opcje.Add("Malarstwo");
            opcje.Add("Rysunek");
            opcje.Add("Tatuaże");
            
            comboBox.ItemsSource = opcje; //przypisanie kolekcji do comboBoxa
        }


        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void button_Accept_Click(object sender, RoutedEventArgs e)
        {
            string link = "http://artgram.hostingpo.pl/dodawanie.php", odpowiedz, dane_obrazu;

            if(textBox.Text == "" || textBox_Copy.Text == "")
            {
                textBlock.Text = "Pola nie mogą być puste.";
            }
            else
            {
                textBlock.Text = "Proszę czekać...";
                Obraz_dod Obraz = new Obraz_dod(textBox.Text, textBox_Copy.Text, "", "");
                

                if ( comboBox.SelectedItem == "Rzeźba")
                {
                    Obraz.ID_Kategorii = "2";
                }
                else if ( comboBox.SelectedItem == "Malarstwo")
                {
                    Obraz.ID_Kategorii = "3";
                }
                else if ( comboBox.SelectedItem == "Rysunek")
                {
                    Obraz.ID_Kategorii = "4";
                }
                else if ( comboBox.SelectedItem == "Tatuaże")
                {
                    Obraz.ID_Kategorii = "5";
                }
                else
                {
                    textBlock.Text = "Wybierz kategorię.";
                }

                if (textBlock.Text != "Wybierz kategorię.")
                {
                    dane_obrazu = JsonConvert.SerializeObject(Obraz);
                    odpowiedz = await Wyslanie(link, dane_obrazu);

                    //ta petla dziala jakos na odwrot, nie wiem dlaczego... 
                    //źle porównuje stringi
                    if (odpowiedz == "\tDodano\t")
                    {
                        textBlock.Text = "Obraz dodany.";
                        textBox.Text = "Dodaj nazwę";
                        textBox_Copy.Text = "Dodaj opis";
                        comboBox.SelectedItem = "";

                        var obrazek = new ImageBrush();
                        var plik = new FileInfo("../Assets/Square44x44Logo.png");
                        var uri = new Uri(plik.FullName);
                        obrazek.ImageSource = new BitmapImage(uri);
                        button.Background = obrazek;
                    }
                    else
                    {
                        textBlock.Text = "Błąd dodania obrazu do bazy.";
                    }
                }
            }
        }

        private async void button_Add_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker Otwieracz = new FileOpenPicker();
            Otwieracz.ViewMode = PickerViewMode.Thumbnail;
            //ustawiamy domyslną lokalizację, w której otworzy się nowe okno dialogowe
            Otwieracz.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //określamy typy plików, jakie można dodawać
            Otwieracz.FileTypeFilter.Add(".jpg");
            Otwieracz.FileTypeFilter.Add(".jpeg");
            Otwieracz.FileTypeFilter.Add(".png");
            Otwieracz.FileTypeFilter.Add(".bmp");

            StorageFile File = await Otwieracz.PickSingleFileAsync();

            if (File != null)
            {
                BasicProperties wlasciwosci = await File.GetBasicPropertiesAsync(); //pobranie danych o pliku (rozmiar)
                //błąd, jeśli rozmiar pliku przekracza 2 MB
                if (wlasciwosci.Size > maxFile)
                {
                    textBlock.Text = "Rozmiar pliku przekroczył 2 MB.";
                    return;
                }

                plik = File;
                var stream = await File.OpenAsync(FileAccessMode.Read);
                var image = new ImageBrush();
                var img = new BitmapImage();

                img.SetSource(stream);
                image.ImageSource = img;
                button.Background = image;
            }
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

        private class Obraz_dod
        {
            public string Nazwa_obrazu, Opis_obrazu, Sciezka_dostepu, ID_Kategorii;

            public Obraz_dod (string Nazwa_obrazu, string Opis_obrazu, string Sciezka_dostepu, 
                string ID_Kategorii)
            {
                this.Nazwa_obrazu = Nazwa_obrazu;
                this.Opis_obrazu = Opis_obrazu;
                this.Sciezka_dostepu = Sciezka_dostepu;
                this.ID_Kategorii = ID_Kategorii;
            }
        }

    }
}
