# System.Concurrent.Collection
## What are the Concurrent collections and when to use them?
Concurrent collections (Namespace: System.Collections.Concurrent) are basically thread safe collections and are designed to be used in multithreading environment

These collections should be used when they are getting changed or data is added/updated/deleted by multiple threads. If the requirement is only read in multithreaded environment then generic collections can be used.

## What are the commonly used concurrent collections we have?
- ConcurrentDictionary -> Thread safe version of Dictionary
- ConcurrentBag -> New thread safe unordered collection
- ConcurrentQueue -> Thread safe version of generic queue (FIFO structure)
- ConcurrentStack -> Thread safe version of generic stack (LIFO structure)

Another classes included in Concurrent collection:

- BlockingCollection 
- OrderablePartitioner (TSource) 
- Partitioner 
- Partitioner (TSource)

### ConcurrentDictionary

Concurrent Dictionary is the general purpose collection and can be used in most of the cases. It has exposed several methods and properties and commonly used methods are as follows.

#### TryAdd
~~~
private static void DoTryAdd(string key, int value)  
        {  
            Console.WriteLine("Is {0} Successfully added? {1}", key, phoneOrders.TryAdd(key, value));  
        }  
~~~
Calling the method to output:
~~~
Console.WriteLine("********************TryAdd********************");  
//TryAdd returns true, if it successfully added.  
string key1 = "Prakash";  
int value1 = 5;  
DoTryAdd(key1, value1);  
string key2 = "Aradhana";  
int value2 = 7;  
DoTryAdd(key2, value2);  
  
string key3 = "DEF";  
int value3 = 6;   
DoTryAdd(key3, value3);  
  
////TryAdd returns false, if it failed to add due to duplicate key.   
DoTryAdd(key2, value2);  
PrintOrders();  
~~~
