
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artgram
{
    class Obraz
    {
        public string Nazwa_obrazu, Sciezka_dostepu, Liczba_WOW, Opis_obrazu, ID_Obrazu, ID_Kategorii;

        public Obraz(string Nazwa_obrazu, string Sciezka_dostepu, string Liczba_WOW, string Opis_obrazu, string ID_Obrazu, string ID_Kategorii)
        {
            this.Nazwa_obrazu = Nazwa_obrazu;
            this.Sciezka_dostepu = Sciezka_dostepu;
            this.Liczba_WOW = Liczba_WOW;
            this.Opis_obrazu = Opis_obrazu;
            this.ID_Obrazu = ID_Obrazu;
            this.ID_Kategorii = ID_Kategorii;
        }
    }
}