using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcor.Server
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

        public IPath Path { get; private set; }
        public Handler Handler { get; set; }

        public Task<IEnumerable<PathValue>> Execute(IPath path)
        {
            return Handler(path);
        }
    }
}