using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Falcor.Server.Routing
{
    public class Route
    {
        public Route()
        {
            Path = new List<IPathComponent>();
        }

        public Route(params IPathComponent[] pathComponents)
        {
            Path = pathComponents.ToList();
        }

        public IList<IPathComponent> Path { get; set; }
        public Func<Task> Handler { get; set; }
    }
}