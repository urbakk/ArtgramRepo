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
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Artgram
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class v_Edycja : Page
    {
        string url, doWyszukaj;
        string[] list;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] lista = e.Parameter as string[]; //Pobieranie tablicy z parametrami wybranego obrazka
            list = lista; 
            url = lista[3];  //Sciezka dostępu
            var uri = new Uri(url, UriKind.Absolute);
            var img = new ImageBrush();
            img.ImageSource = new BitmapImage(uri);
            button.Background = img;
            textBox.Text = lista[0];  //Nazwa obrazu
            if (lista[1] == null)
            {
                textBox_Copy.Text = "Autor nie dodał opisu";
            }
            else
            {
                textBox_Copy.Text = lista[1];  //Opis obrazka
            }

            doWyszukaj = lista[4];   //Nazwa po której szukaliśmy. Potrzebna do powrotu

            if (list[6] == "2")
            {
                comboBox.SelectedItem = "Rzeźba";
            }
            else if (list[6] == "3")
            {
                comboBox.SelectedItem = "Malarstwo";
            }
            else if (list[6] == "4")
            {
                comboBox.SelectedItem = "Rysunek";
            }
            else if (list[6] == "5")
            {
                comboBox.SelectedItem = "Tatuaże";
            }
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(v_Szukaj), doWyszukaj);
        }

        public v_Edycja()
        {
            this.InitializeComponent();
            textBlock.Text = "";

            var opcje = new ObservableCollection<string>(); //utworzenie kolekcji z opcjami wyboru kategorii
            //var item = ""; //proces uzupełniania kolekcji
            //opcje.Add(item);
            opcje.Add("Rzeźba");
            opcje.Add("Malarstwo");
            opcje.Add("Rysunek");
            opcje.Add("Tatuaże");

            comboBox.ItemsSource = opcje;



        }

        private async void button_Accept_Click(object sender, RoutedEventArgs e)
        {
            string odpowiedz, link = "http://artgram.hostingpo.pl/edycja.php", dane_obrazu;

            if (textBox.Text == "" || textBox_Copy.Text == "")
            {
                textBlock.Text = "Pola nie mogą być puste.";
            }
            else
            {
                textBlock.Text = "Proszę czekać...";
                Edit_Obraz Edycja = new Edit_Obraz(textBox.Text, textBox_Copy.Text, list[5], "");

                if (comboBox.SelectedItem == "Rzeźba")
                {
                    Edycja.ID_Kategorii = "2";
                }
                else if (comboBox.SelectedItem == "Malarstwo")
                {
                    Edycja.ID_Kategorii = "3";
                }
                else if (comboBox.SelectedItem == "Rysunek")
                {
                    Edycja.ID_Kategorii = "4";
                }
                else if (comboBox.SelectedItem == "Tatuaże")
                {
                    Edycja.ID_Kategorii = "5";
                }
                else
                {
                    textBlock.Text = "Wybierz kategorię.";
                    return;
                }
                
                if (textBlock.Text != "Wybierz kategorię.")
                {

                        dane_obrazu = JsonConvert.SerializeObject(Edycja);
                        odpowiedz = await Wyslanie(link, dane_obrazu);

                        if (odpowiedz == "\tDodano")
                        {
                            textBlock.Text = "Edycja zkończona.";

                        }
                        else
                        {
                            textBlock.Text = "Edycja się nie powiodła";
                        }
                    
                }
                else
                {
                    //
                    textBlock.Text = "Błąd systemu.";
                }
            }
        }

        class Edit_Obraz
        {
            public string Nazwa_obrazu, Opis_obrazu, ID_Obrazu, ID_Kategorii;

            public Edit_Obraz (string Nazwa_obrazu, string Opis_obrazu, string ID_Obrazu, string ID_Kategorii)
            {
                this.Nazwa_obrazu = Nazwa_obrazu;
                this.Opis_obrazu = Opis_obrazu;
                this.ID_Obrazu = ID_Obrazu;
                this.ID_Kategorii = ID_Kategorii;
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

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //funkcja, zeby po kliknieciu w boxa zniknęła zawartość
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= textBox_GotFocus;
        }

        private void textBox_Copy_GotFocus(object sender, RoutedEventArgs e)
        {
            //funkcja, zeby po kliknieciu w boxa zniknęła zawartość
            TextBox tb1 = (TextBox)sender;
            tb1.Text = string.Empty;
            tb1.GotFocus -= textBox_Copy_GotFocus;
        }
    }
}
