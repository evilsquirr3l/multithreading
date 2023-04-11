﻿/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static readonly Semaphore Semaphore = new(1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            var startingNumber = 10;
            var initialThread = new Thread(ThreadFunction);
            initialThread.Start(startingNumber);
            initialThread.Join();

            Console.WriteLine("a) Done. Press any key to do the same using ThreadPool and semaphores.");
            Console.ReadLine();

            ThreadPoolFunction(startingNumber);

            Console.WriteLine("b) Done.");

            Console.ReadLine();
        }

        private static void ThreadFunction(object state)
        {
            var number = (int)state;

            if (number > 0)
            {
                Console.WriteLine(number);
                number--;

                var newThread = new Thread(ThreadFunction);
                newThread.Start(number);
                newThread.Join();
            }
        }

        private static void ThreadPoolFunction(object state)
        {
            Semaphore.WaitOne();
            
            var number = (int)state;

            if (number > 0)
            {
                Console.WriteLine(number);
                number--;

                ThreadPool.QueueUserWorkItem(ThreadPoolFunction, number);
            }

            Semaphore.Release();
        }
    }
}
