/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */

using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var task1 = Task.Run(Task1);
            var task2 = task1.ContinueWith(task => Task2(task.Result));
            var task3 = task2.ContinueWith(task => Task3(task.Result));
            var task4 = task3.ContinueWith(task => Task4(task.Result));

            task4.Wait();

            Console.ReadLine();
        }

        private static int[] Task1()
        {
            var randomNumbers = new int[10];
            var random = new Random();

            for (var i = 0; i < randomNumbers.Length; i++)
            {
                randomNumbers[i] = random.Next(1, 100);
            }

            Console.WriteLine($"Task1 - Array of random integers: {string.Join(", ", randomNumbers)}");
            return randomNumbers;
        }

        private static int[] Task2(int[] randomNumbers)
        {
            var random = new Random();
            var multiplier = random.Next(1, 10);

            for (var i = 0; i < randomNumbers.Length; i++)
            {
                randomNumbers[i] *= multiplier;
            }

            Console.WriteLine($"Task2 - Multiplied array: {string.Join(", ", randomNumbers)}");
            return randomNumbers;
        }

        private static int[] Task3(int[] multipliedRandomNumbers)
        {
            Array.Sort(multipliedRandomNumbers);

            Console.WriteLine($"Task3 - Sorted array: {string.Join(", ", multipliedRandomNumbers)}");
            return multipliedRandomNumbers;
        }

        private static double Task4(int[] sortedNumbers)
        {
            var numbers = sortedNumbers;
            var average = numbers.Average();

            Console.WriteLine($"Task4 - Average value: {average}");
            return average;
        }
    }
}