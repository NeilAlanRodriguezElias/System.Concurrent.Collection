# System.Concurrent.Collection
## What are the Concurrent collections and when to use them?
Concurrent collections (Namespace: System.Collections.Concurrent) are basically thread safe collections and are designed to be used in multithreading environment

These collections should be used when they are getting changed or data is added/updated/deleted by multiple threads. If the requirement is only read in multithreaded environment then generic collections can be used.

## What are the commonly used concurrent collections we have?
- [ConcurrentDictionary](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-5.0) -> Thread safe version of Dictionary
- [ConcurrentQueue](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1?view=net-5.0) -> Thread safe version of generic queue (FIFO structure)
- [ConcurrentBag](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=net-5.0) -> New thread safe unordered collection
- [ConcurrentStack](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=net-5.0) -> Thread safe version of generic stack (LIFO structure)

Another classes included in Concurrent collection:

- [BlockingCollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.blockingcollection-1?view=net-5.0)
- [OrderablePartitioner](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.orderablepartitioner-1?view=net-5.0) 
- [Partitioner](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.partitioner?view=net-5.0) 
- [Partitioner (TSource)](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.partitioner-1?view=net-5.0)

### ConcurrentDictionary

Concurrent Dictionary is the general purpose collection and can be used in most of the cases. It has exposed several methods and properties and commonly used methods are as follows.

- [TryAdd](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.tryadd?view=net-5.0https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.tryadd?view=net-5.0) (Adds the specified key/value pair, if the key doesn't currently exist in the dictionary)
- [TryUpdate](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.tryupdate?view=net-5.0) (Checks whether the key has a specified value, and if it does, updates the key with a new value.)
- [AddOrUpdate](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.addorupdate?view=net-5.0#System_Collections_Concurrent_ConcurrentDictionary_2_AddOrUpdate__0_System_Func__0__1__System_Func__0__1__1__) (Accepts the key, a value to add, and the update delegate.)
- [GetOrAdd](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.getoradd?view=net-5.0#System_Collections_Concurrent_ConcurrentDictionary_2_GetOrAdd__0__1_) (Overloads provide lazy initialization for a key/value pair in the dictionary, adding the value only if it's not there.)

~~~
public class Program {  
    static Dictionary < string, int > _mydic = new Dictionary < string, int > ();  
    static ConcurrentDictionary < string, int > _mydictConcu = new ConcurrentDictionary < string, int > ();  
    static void Main() {  
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
        Console.WriteLine($ "Result in Dictionary : {_mydic.Values.Count}");  
        Console.WriteLine("********************************************");  
        Console.WriteLine($ "Result in Concurrent Dictionary : {_mydictConcu.Values.Count}");  
        Console.ReadKey();  
    }  
    static void InsertData() {  
        for (int i = 0; i < 100; i++) {  
            _mydic.Add(Guid.NewGuid().ToString(), i);  
        }  
    }  
    static void InsertDataConcu() {  
        for (int i = 0; i < 100; i++) {  
            _mydictConcu.TryAdd(Guid.NewGuid().ToString(), i);  
        }  
    }  
}  
// Output    
Result in Dictionary: 189 ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** Result in Concurrent Dictionary: 200
~~~
### ConcurrentQueue

ConcurrentQueue is a wrapper around generic Queue class. Queue class also provides FIFO data structure but it is not safe to use with multi-threading environment. To provide thread-safety, we have to implement locking around Queue methods which is always error prone.

Concurrent Queue has exposed several other methods. Let's look at some of the commonly used ones.
- [Clear](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.clear?view=net-5.0#System_Collections_Concurrent_ConcurrentQueue_1_Clear) (Removes all objects from the ConcurrentQueue.)
- [CopyTo](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.copyto?view=net-5.0#System_Collections_Concurrent_ConcurrentQueue_1_CopyTo__0___System_Int32_) (Copies the ConcurrentQueue elements to an existing one-dimensional Array)
- [Enqueue](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.enqueue?view=net-5.0#System_Collections_Concurrent_ConcurrentQueue_1_Enqueue__0_) (Adds an object to the end of the ConcurrentQueue.)
- Equals
- [GetEnumerator](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.getenumerator?view=net-5.0#System_Collections_Concurrent_ConcurrentQueue_1_GetEnumerator)
- GetHashCode
- GetType
- MemberwiseClone
- [ToArray](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.toarray?view=net-5.0#System_Collections_Concurrent_ConcurrentQueue_1_ToArray) (Copies the elements stored in the ConcurrentQueue<T> to a new array.)
- [TryDequeue](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.trydequeue?view=net-5.0#System_Collections_Concurrent_ConcurrentQueue_1_TryDequeue__0__) (Tries to remove and return the object at the beginning of the concurrent queue.)
- [TryPeek](https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.trypeek?view=net-5.0)
(Tries to return an object from the beginning of the ConcurrentQueue without removing it.)


~~~
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
~~~
