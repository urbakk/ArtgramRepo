
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Artgram
{
    class Zapytanie
    {
        public string ID_Kategorii;

        public Zapytanie(string ID_Kategorii)
        {
            this.ID_Kategorii = ID_Kategorii;
        }
        
    }
}
