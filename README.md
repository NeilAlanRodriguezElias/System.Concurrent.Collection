# System.Concurrent.Collection
## What are the Concurrent collections and when to use them?
Concurrent collections (Namespace: System.Collections.Concurrent) are basically thread safe collections and are designed to be used in multithreading environment

These collections should be used when they are getting changed or data is added/updated/deleted by multiple threads. If the requirement is only read in multithreaded environment then generic collections can be used.

## What are the commonly used concurrent collections we have?
- [ConcurrentDictionary](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-5.0) -> Thread safe version of Dictionary
- [ConcurrentBag](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=net-5.0) -> New thread safe unordered collection
- [ConcurrentStack](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=net-5.0) -> Thread safe version of generic stack (LIFO structure)
- [ConcurrentQueue](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1?view=net-5.0) -> Thread safe version of generic queue (FIFO structure)

Another classes included in Concurrent collection:

- [BlockingCollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.blockingcollection-1?view=net-5.0)
- [OrderablePartitioner](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.orderablepartitioner-1?view=net-5.0) 
- [Partitioner](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.partitioner?view=net-5.0) 
- [Partitioner (TSource)](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.partitioner-1?view=net-5.0)

### ConcurrentDictionary

Concurrent Dictionary is the general purpose collection and can be used in most of the cases. It has exposed several methods and properties and commonly used methods are as follows:

- [TryAdd](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.tryadd?view=net-5.0https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.tryadd?view=net-5.0) (Adds the specified key/value pair, if the key doesn't currently exist in the dictionary)
- [TryUpdate](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.tryupdate?view=net-5.0) (Checks whether the key has a specified value, and if it does, updates the key with a new value.)
- [AddOrUpdate](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.addorupdate?view=net-5.0#System_Collections_Concurrent_ConcurrentDictionary_2_AddOrUpdate__0_System_Func__0__1__System_Func__0__1__1__) (Accepts the key, a value to add, and the update delegate.)
- [GetOrAdd](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.getoradd?view=net-5.0#System_Collections_Concurrent_ConcurrentDictionary_2_GetOrAdd__0__1_) (Overloads provide lazy initialization for a key/value pair in the dictionary, adding the value only if it's not there.)

Example:
~~~
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
~~~

### ConcurrentBag

ConcurrentBag is a collection class that allows generic data to be stored in unordered from. It is a thread-safe class and allows multiple threads to use it.

Here's the list of the important methods of the ConcurrentBag class:
- [Add](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1.add?view=net-5.0#System_Collections_Concurrent_ConcurrentBag_1_Add__0_)(T element) (This method is used to add an element to the ConcurrentBag)
- [TryPeek](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1.trypeek?view=net-5.0#System_Collections_Concurrent_ConcurrentBag_1_TryPeek__0__)(out T) This method is used to retrieve an element from ConcurrentBag without removing it.
- [TryTake](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1.trytake?view=net-5.0#System_Collections_Concurrent_ConcurrentBag_1_TryTake__0__)(out T) This method is used to retrieve an element from ConcurrentBag. Note that this method removes the item from the collection.

Example:
~~~
class Program
    {
        static void Main(string[] args)
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            for (int i = 1; i <= 50; ++i)
            {
                bag.Add(i);
            }
            var task1 = Task.Factory.StartNew(() => {
                while (bag.IsEmpty == false)
                {
                    int item;
                    if (bag.TryTake(out item))
                    {
                        Console.WriteLine($"{item} was picked by {Task.CurrentId}");
                    }
                }
            });
            var task2 = Task.Factory.StartNew(() => {
                while (bag.IsEmpty == false)
                {
                    int item;
                    if (bag.TryTake(out item))
                    {
                        Console.WriteLine($"{item} was picked by {Task.CurrentId}");
                    }
                }
            });
            Task.WaitAll(task1, task2);
            Console.WriteLine("DONE");
        }
    }
~~~

### ConcurrentStack
The stack is one of the most basic data structures and also one of the most widely used. ConcurrentStack is like wrapper around Stack class. Stack class is not thread-safe. ConcurrentStack provides thread-safety. It internally uses locking to synchronize different threads.

Its most important methods are:
- [Push](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentstack-1.push?view=net-5.0#System_Collections_Concurrent_ConcurrentStack_1_Push__0_) (T element) (This method is used to add data of type T.)
- [PushRange](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentstack-1.pushrange?view=net-5.0#System_Collections_Concurrent_ConcurrentStack_1_PushRange__0___) (This method can be used to add an array of items of type T.)
- [TryPop](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentstack-1.trypop?view=net-5.0#System_Collections_Concurrent_ConcurrentStack_1_TryPop__0__) (out T) (This method is used to retrieve the first element from the stack. It returns true on success, false otherwise.)
- [TryPeek](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentstack-1.trypeek?view=net-5.0#System_Collections_Concurrent_ConcurrentStack_1_TryPeek__0__) (out T) (This method is used to retrieve the next element from the stack but it doesn't remove the element from the stack. Note that similar to the TryPop(out T) method, it returns true on success and false otherwise.)
- [TryPopRange](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentstack-1.trypoprange?view=net-5.0#System_Collections_Concurrent_ConcurrentStack_1_TryPopRange__0___) (This method is overloaded and works similar to the TryPop but is used for retriving arrays from the stack)

Example:
~~~
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
~~~

### ConcurrentQueue

ConcurrentQueue is a wrapper around generic Queue class. Queue class also provides FIFO data structure but it is not safe to use with multi-threading environment. To provide thread-safety, we have to implement locking around Queue methods which is always error prone.

Concurrent Queue has exposed several other methods. Let's look at some of the commonly used ones:
- [Enqueue](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.enqueue?view=net-5.0#System_Collections_Concurrent_ConcurrentQueue_1_Enqueue__0_) (Adds an object to the end of the ConcurrentQueue.)
- [TryDequeue](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.trydequeue?view=net-5.0) (Tries to remove and return the object at the beginning of the concurrent queue.)
- [TryPeek](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.trypeek?view=net-5.0)
(Tries to return an object from the beginning of the ConcurrentQueue without removing it.)

Example:
~~~
class Program
    {
        public static Queue<int> genericQueue = new Queue<int>();
        public static ConcurrentQueue<int> concurrentQueue = new ConcurrentQueue<int>();

        public static void EnqueueNotConcurrent(){
            for (int i = 0; i < 100; i++)
            {
                genericQueue.Enqueue(i);
                Thread.Sleep(100);
            }
        }

        public static void EnqueueConcurrent()
        {
            for (int i = 0; i < 100; i++)
            {
                concurrentQueue.Enqueue(i);
                Thread.Sleep(100);
            }
        }

        static void Main(string[] args)
        {
            Thread threadNotConcurrent = new Thread(new ThreadStart(EnqueueNotConcurrent));
            threadNotConcurrent.Start();
            // Take out comment below to try genericQueuePeek
            // Console.WriteLine("Generic Peek first " + genericQueue.Peek());

            Thread threadConcurrent = new Thread(new ThreadStart(EnqueueConcurrent));
            threadConcurrent.Start();
            int result;
            if (concurrentQueue.TryPeek(out result))
            {
                Console.WriteLine("Concurrent Peek first " + result);
            }else
            {
                Console.WriteLine("Concurrent Not peeked ");
            }
        }
    }
~~~
