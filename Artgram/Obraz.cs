
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artgram
{
    class Obraz
    {
        public string Nazwa_obrazu, Sciezka_dostepu, Liczba_WOW, Opis_obrazu;

        public Obraz(string Nazwa_obrazu, string Sciezka_dostepu, string Liczba_WOW, string Opis_obrazu)
        {
            this.Nazwa_obrazu = Nazwa_obrazu;
            this.Sciezka_dostepu = Sciezka_dostepu;
            this.Liczba_WOW = Liczba_WOW;
            this.Opis_obrazu = Opis_obrazu;
        }
    }
}