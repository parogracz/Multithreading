using System;
using System.Threading;

namespace Projekt3
{
    class BigReaderLock
    {
        Mutex[] mutex = new Mutex[8]; //Tablica zamków
        string[] tablica = { "Wypisanie" }; // Tablica do której zawartości ubiegać będą się czytacz i piszacz
        /// <summary>
        /// Konstruktor tworzy instancję mutexów do tablicy.
        /// </summary>
        public BigReaderLock()
        {
            for (int i = 0; i < mutex.Length; i++)
            {
                mutex[i] = new Mutex();
            }
        }
        /// <summary>
        /// Metoda dla czytaczy, każdy wylosuje sobie numer i zablokuje zamek 
        /// o tymże numerze po czym zacznie czytać i po skończeniu odblokuje swój mutex.
        /// </summary>
        /// <param name="num"> numer mutexu do zamknięcia </param>
        public void Access(int num)
        {
            mutex[num].WaitOne();
            foreach (string i in this.tablica) Console.WriteLine(i + " ");
            mutex[num].ReleaseMutex();
        }
        /// <summary>
        /// Metoda dla piszaczy, blokujemy wszystkie mutexy. 
        /// Piszacz zmienia "Wypisanie" na "*****" bądź odwrotnie w zależności od stanu tablicy.
        /// </summary>
        public void AccessAll()
        {
            for (int i = 0; i < 8; i++) mutex[i].WaitOne();
            if (tablica[0] == "Wypisanie") tablica[0] = "*****";
            else tablica[0] = "Wypisanie";
            for (int i = 0; i < 8; i++) mutex[i].ReleaseMutex();
        }
        /// <summary>
        /// Metoda losująca czytaczowi 100 razy mutex z zakresu 0-8 i odsyłająca do metody Access.
        /// </summary>
        public void Czytacz()
        {
            Random r = new Random();
            //while(true)
            for (int i = 0; i < 100; i++)
            {
                Access(r.Next() % 8);
            }
        }

        /// <summary>
        /// Metoda 100 razy uruchamiająca piszacza, odsyła do AccessAll.
        /// </summary>
        public void Pisarz()
        {
            //while(true) AccessAll();
            for (int i = 0; i < 100; i++) AccessAll();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            BigReaderLock brl = new BigReaderLock();
            Thread[] thread = new Thread[9];
            for (int i = 0; i < 8; i++) thread[i] = new Thread(brl.Czytacz);
            thread[8] = new Thread(brl.Pisarz);
            Console.WriteLine("===Start===");
            for (int i = 0; i < 9; i++) thread[i].Start();
        }
    }
}
