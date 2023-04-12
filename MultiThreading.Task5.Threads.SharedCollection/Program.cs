/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static readonly ConcurrentBag<int> SharedCollection = new();
        private static readonly object SyncLock = new();
        
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var addThread = new Thread(AddItems);
            var printThread = new Thread(PrintItems);

            addThread.Start();
            printThread.Start();

            addThread.Join();
            printThread.Join();

            Console.ReadLine();
        }

        private static void AddItems()
        {
            for (var i = 0; i < 10; i++)
            {
                lock (SyncLock)
                {
                    SharedCollection.Add(i);
                    Console.WriteLine($"Item {i} added.");
                    Monitor.Pulse(SyncLock);
                }
                Thread.Sleep(100);
            }
        }

        private static void PrintItems()
        {
            var itemCount = 0;

            while (itemCount < 10)
            {
                lock (SyncLock)
                {
                    while (SharedCollection.IsEmpty)
                    {
                        Monitor.Wait(SyncLock);
                    }

                    if (SharedCollection.TryTake(out var item))
                    {
                        Console.WriteLine($"Item {item} printed.");
                        itemCount++;
                    }
                }
            }
        }
    }
}
