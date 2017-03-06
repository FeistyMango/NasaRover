using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaApp
{
    public class MovableFactory: IMovableFactory
    {
        private IEnvironment Environment { get; set; }
        private IParser Parser { get; set; }
        private ILogger Logger { get; set; }

        public MovableFactory(IEnvironment environment, IParser parser, ILogger logger)
        {
            Environment = environment;
            Parser = parser;
            Logger = logger;
        }

        public IMovable Rover()
        {
            return new Rover(Environment, Parser, Logger);
        }
    }
}
