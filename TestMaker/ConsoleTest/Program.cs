using System;
using Lib;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Test("kek", null);

            Console.WriteLine(test.Tasks.Count);
        }
    }
}
