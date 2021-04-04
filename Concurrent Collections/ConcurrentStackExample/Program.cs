using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ConcurrentStackExample
{
    class Program
    {
        public static Stack<int> genericStack = new Stack<int>();
        public static ConcurrentStack<int> concurrentStack = new ConcurrentStack<int>();

        public static void PushConcurrent()
        {
            for (int i = 0; i < 100; i++)
            {
                concurrentStack.Push(i);
                // Thread.Sleep(100);
            }
        }

        public static void PushGeneric()
        {
            for (int i = 0; i < 10; i++)
            {
                genericStack.Push(i);
                // Thread.Sleep(100);
            }
        }

        public static void Verify()
        {
            Thread.Sleep(200);
            var resp = concurrentStack.TryPeek(out int result);
            if (resp)
            {
                Console.WriteLine("RESULT= " + result);
                Console.WriteLine($"TryPeek() saw {result} on top of the stack.");
            }
            else
            {
                Console.WriteLine("RESULT= " + result + " - " + resp);
                Console.WriteLine("Could not peek most recently added number.");
            }


        }

        static void Main(string[] args)
        {
            Thread threadGenericStack = new Thread(new ThreadStart(PushGeneric));
            threadGenericStack.Start();

            Thread threadConcurrentStack = new Thread(new ThreadStart(PushConcurrent));
            threadConcurrentStack.Start();

            // Console.WriteLine("Generic => " + genericStack.Peek());

            Thread verifyThread = new Thread(new ThreadStart(Verify));
            verifyThread.Start();
        }


    }
}
