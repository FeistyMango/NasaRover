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

        public MovableFactory(IEnvironment environment, IParser parser)
        {
            Environment = environment;
            Parser = parser;
        }

        public IMovable Rover()
        {
            return new Rover(Environment, Parser);
        }
    }
}
