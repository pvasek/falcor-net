using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public class Path: IPath
    {
        public Path(params IPathComponent[] components)
        {
            Components = new List<IPathComponent>(components);
        }

        public IList<IPathComponent> Components { get; private set; }
    }
}