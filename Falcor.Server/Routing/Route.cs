using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public class Route
    {
        public Route()
        {
            Path = new Path();
        }

        public Route(params IPathComponent[] pathComponents)
        {
            Path = new Path(pathComponents);
        }

        public IPath Path { get; set; }
        public Handler Handler { get; set; }
    }
}