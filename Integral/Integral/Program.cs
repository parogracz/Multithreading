using System;
using System.Threading;
using System.Threading.Tasks;

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
            double przedział = (koniec - start) / this.precyzja;
            object nazwa = new object();
            for (double i = start; i <= koniec; i += przedział)
            {
                lock(nazwa)
                {
                    this.wynik += (3 * Math.Pow(i, 3) + Math.Cos(7 * i) - Math.Log(2 * i)) * przedział;
                }
            }
        }
        public void Oblicz_3(float start, float koniec)
        {
            double wynik = 0;
            double przedział = (koniec - start) / this.precyzja;
            object nazwa = new object();
            for (double i = start; i <= koniec; i += przedział)
            {
                wynik += (3 * Math.Pow(i, 3) + Math.Cos(7 * i) - Math.Log(2 * i)) * przedział;  
            }
            lock (nazwa)
            {
                this.wynik += wynik;
            }
        }        
        public void Oblicz_4(float start, float koniec)
        {
            double wynik = 0;
            double przedział = (koniec - start) / this.precyzja;
            object nazwa = new object();
            for (double i = start; i <= koniec; i += przedział)
            {
                wynik += (3 * Math.Pow(i, 3) + Math.Cos(7 * i) - Math.Log(2 * i)) * przedział;  
            }
            this.wynik += wynik;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var timer = new System.Diagnostics.Stopwatch();
            //Jeden Wątek
            Całka c1 = new Całka(5000000);
            Thread t = new Thread(() => c1.Oblicz_0());
            timer.Start();
            t.Start();
            t.Join();
            timer.Stop();
            Console.WriteLine("1Th : "+c1.wynik+" |"+timer.Elapsed+"|");
            timer.Reset();
            //Punkt 1
            Całka c2 = new Całka(5000000);
            Thread[] t1 = new Thread[40];
            timer.Start();
            for (int i = 1; i < 40; i++)
            {
                t1[i - 1] = new Thread(() => c2.Oblicz_1(i, i + 1));
                t1[i - 1].Start();
                t1[i - 1].Join();
            }
            timer.Stop();
            Console.WriteLine("(i): "+c2.wynik + " |" + timer.Elapsed + "|");
            timer.Reset();
            //Punkt 2
            Całka c3 = new Całka(5000000);
            Thread[] t2 = new Thread[40];
            timer.Start();
            for (int i = 1; i < 40; i++)
            {
                t2[i - 1] = new Thread(() => c3.Oblicz_2(i, i + 1));
                t2[i - 1].Start();
                t2[i - 1].Join();
            }
            timer.Stop();
            Console.WriteLine("(ii): "+c3.wynik + " |" + timer.Elapsed + "|");
            timer.Reset();
            //Punkt 3
            Całka c4 = new Całka(5000000);
            Thread[] t3 = new Thread[40];
            timer.Start();
            for (int i = 1; i < 40; i++)
            {
                t3[i - 1] = new Thread(() => c4.Oblicz_3(i, i + 1));
                t3[i - 1].Start();
                t3[i - 1].Join();
            }
            timer.Stop();
            Console.WriteLine("(iii): "+c4.wynik + " |" + timer.Elapsed + "|");
            timer.Reset();
            //Punkt 4
            Całka c5 = new Całka(5000000);
            Task[] task = new Task[40];
            timer.Start();
            Parallel.For(1, 40, (i) =>
            //for(int i=1; i<40; i++)
            {
                  task[i - 1] = new Task(() => c5.Oblicz_3(i, i + 1));
                  task[i - 1].Start();
                  task[i - 1].Wait();
            }
            );
            timer.Stop();
            Console.WriteLine("(iv): " + c5.wynik + " |" + timer.Elapsed + "|");
            Console.ReadKey();
        }
    }
}

