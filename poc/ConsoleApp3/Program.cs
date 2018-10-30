using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime alarmTime = new DateTime(2018, 10, 29, 23, 30, 15);

            // c
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine($"Set alarm time to {alarmTime.ToString("s")}");

        }
    }
}
