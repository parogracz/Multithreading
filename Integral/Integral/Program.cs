using System;
using System.Threading;

namespace Integral
{
    class Całka
    {
        double wynik;
        double precyzja;

        public Całka(int precyzja)
        {
            this.precyzja = precyzja;
            this.wynik = 0;
        }

        public double Oblicz_1()
        {
            double wynik = 0;
            double przedział = (40-1) / this.precyzja;
            for(double i = 1; i<=40; i+=przedział)
            {
                wynik += (3 * Math.Pow(i, 3) + Math.Cos(7 * i) - Math.Log(2 * i));
            }
            wynik *= przedział;
            this.wynik = wynik;
            return wynik;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Całka c = new Całka(1000000);
            Console.WriteLine(c.Oblicz_1());
            Console.ReadKey();
        }
    }
}
