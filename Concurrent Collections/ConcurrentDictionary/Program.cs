using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
namespace ConcurrentDictionary
{
    public class Program
    {
        public static  Dictionary<string, int> _mydic = new Dictionary<string, int>();
        public static  ConcurrentDictionary<string, int> _mydictConcu = new ConcurrentDictionary<string, int>();
        static void Main()
        {
            Thread mythread1 = new Thread(new ThreadStart(InsertData));
            Thread mythread2 = new Thread(new ThreadStart(InsertData));
            mythread1.Start();
            mythread2.Start();
            mythread1.Join();
            mythread2.Join();
            Thread mythread11 = new Thread(new ThreadStart(InsertDataConcu));
            Thread mythread21 = new Thread(new ThreadStart(InsertDataConcu));
            mythread11.Start();
            mythread21.Start();
            mythread11.Join();
            mythread21.Join();
            Console.WriteLine($"Result in Dictionary : {_mydic.Values.Count}");
            Console.WriteLine("********************************************");
            Console.WriteLine($"Result in Concurrent Dictionary : {_mydictConcu.Values.Count}");
            Console.ReadKey();
        }
        static void InsertData()
        {
            for (int i = 0; i < 100; i++)
            {
                _mydic.Add(Guid.NewGuid().ToString(), i);
            }
        }
        static void InsertDataConcu()
        {
            for (int i = 0; i < 100; i++)
            {
                _mydictConcu.TryAdd(Guid.NewGuid().ToString(), i);
            }
        }
    }
}
