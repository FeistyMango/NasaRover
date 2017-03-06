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
            var logger = Log.Logger;
            var parser = new Parser(logger);
            var plateau = new Plateau(parser);
            var factory = new MovableFactory(plateau, parser, logger);
            

            var mars = new MarsSimulator(plateau, parser, factory, logger).Init(input);
            Console.WriteLine(mars.Simulate());
            Console.Read();
        }
    }
}