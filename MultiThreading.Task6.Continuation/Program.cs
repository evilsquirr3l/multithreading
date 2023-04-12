/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            CriteriaA();
            CriteriaB();
            CriteriaC();
            CriteriaD();

            Console.ReadLine();
        }

        private static void CriteriaA()
        {
            var parentTask = Task.Factory.StartNew(() =>
            {
                return 2 + 2;
            });

            var continuationTask = parentTask.ContinueWith(parent =>
            {
                Console.WriteLine($"{nameof(CriteriaA)}: Continuation task should be executed regardless of the result of the parent task.");
                Console.WriteLine($"{nameof(CriteriaA)}: Parent task result: {parent.Result}");
            });
        }

        private static void CriteriaB()
        {
            var parentTask = Task.Factory.StartNew(() =>
            {
                throw new Exception("Something bad happened.");
            });

            var continuationTask = parentTask.ContinueWith(parent =>
            {
                Console.WriteLine($"{nameof(CriteriaB)}: Continuation task should be executed when the parent task finished without success.");
                Console.WriteLine($"{nameof(CriteriaB)}: Parent task exception: {parent.Exception.InnerException.Message}");
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
        
        private static void CriteriaC()
        {
            var parentTask = Task.Factory.StartNew(() =>
            {
                throw new Exception("Something bad happened.");
            });

            var continuationTask = parentTask.ContinueWith(parent =>
            {
                Console.WriteLine($"{nameof(CriteriaC)}: Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation");
                Console.WriteLine($"{nameof(CriteriaC)}: Parent task exception: {parent.Exception.InnerException.Message}");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }
        
        private static void CriteriaD()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            
            var parentTask = Task.Factory.StartNew(() =>
            {
                cts.Cancel();
                cts.Token.ThrowIfCancellationRequested();
            }, token);

            var continuationTask = parentTask.ContinueWith(parent =>
            {
                Console.WriteLine($"{nameof(CriteriaD)}: Continuation task should be executed outside of the thread pool when the parent task would be cancelled");
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);
        }
    }
}
