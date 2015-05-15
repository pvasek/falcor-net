using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcor.Server.Routing
{
    public class Route
    {
        public IList<PathFragment> Path { get; set; }
        public Func<Task> Handler { get; set; }
    }
}