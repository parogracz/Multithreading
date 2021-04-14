using System;
using System.Threading;

namespace Integral
{
    class Całka
    {
        public double wynik;
        protected double precyzja;

        public Całka(int precyzja)
        {
            this.precyzja = precyzja;
            this.wynik = 0;
        }
        public void Oblicz_0()
        {
            double wynik = 0;
            double przedział = (40-1) / this.precyzja;
            for(double i = 1; i<=40; i+=przedział)
            {
                wynik += (3 * Math.Pow(i, 3) + Math.Cos(7 * i) - Math.Log(2 * i));
            }
            wynik *= przedział;
            this.wynik = wynik;
        }
        public void Oblicz_1(float start, float koniec)
        {
            double wynik = 0;
            double przedział = (koniec-start) / this.precyzja;
            //object nazwa = new object();
            for(double i = start; i<=koniec; i+=przedział)
            {
                    wynik += (3 * Math.Pow(i, 3) + Math.Cos(7 * i) - Math.Log(2 * i)); 
            }
            wynik *= przedział;
            this.wynik = wynik;
        }
        public void Oblicz_2(float start, float koniec)
        {
            double wynik = 0;
            double przedział = (koniec - start) / this.precyzja;
            object nazwa = new object();
            for (double i = start; i <= koniec; i += przedział)
            {
                lock(nazwa)
                {
                    Monitor.Enter(nazwa);
                    wynik += (3 * Math.Pow(i, 3) + Math.Cos(7 * i) - Math.Log(2 * i));
                }
            }
            wynik *= przedział;
            this.wynik = wynik;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //Jeden Wątek
            Całka c1 = new Całka(100000);
            Thread t = new Thread(() => c1.Oblicz_0());
            t.Start();
            t.Join();
            Console.WriteLine(c1.wynik);
            //Punkt 1
            Całka c2 = new Całka(100000);
            Thread[] t1 = new Thread[40];
            for(int i = 1; i<40; i++)
            {
                t1[i-1] = new Thread(() => c2.Oblicz_1(i,i+1));
                t1[i-1].Start();
                t1[i-1].Join();
            }
            Console.WriteLine(c2.wynik);
            //Punkt 2
            Całka c3 = new Całka(100000);
            Thread[] t2 = new Thread[40];
            for(int i = 1; i<40; i++)
            {
                t1[i-1] = new Thread(() => c3.Oblicz_2(i,i+1));
                t1[i-1].Start();
                t1[i-1].Join();
            }
            Console.WriteLine(c3.wynik);
            //Punkt 3
            Całka c4 = new Całka(100000);
            Thread[] t3 = new Thread[40];
            for (int i = 1; i < 40; i++)
            {
                t1[i - 1] = new Thread(() => c3.Oblicz_2(i, i + 1));
                t1[i - 1].Start();
                t1[i - 1].Join();
            }
            Console.WriteLine(c3.wynik);
            Console.ReadKey();
        }
    }
}

