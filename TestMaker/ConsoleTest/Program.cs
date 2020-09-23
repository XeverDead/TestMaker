using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Lib;
using Lib.TaskTypes;
using UI;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.Date + DateTime.Now.TimeOfDay);
        }
    }
}
