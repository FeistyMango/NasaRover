using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NasaApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var input = @"
            5 5
            1 2 N
            LMLMLMLMM
            3 3 E
            MMRMMRMRRM";

            var mars = new MarsSimulator(input);
            mars.Simulate();
            Console.Read();
        }
    }
}