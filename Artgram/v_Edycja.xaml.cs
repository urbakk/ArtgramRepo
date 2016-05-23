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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Artgram
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class v_Edycja : Page
    {
        string url, doWyszukaj;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] lista = e.Parameter as string[];  //Pobieranie tablicy z parametrami wybranego obrazka
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
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(v_Szukaj), doWyszukaj);
        }

        public v_Edycja()
        {
            this.InitializeComponent();
        }
    }
}
