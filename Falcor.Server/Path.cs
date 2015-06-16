using System.Collections.Generic;

namespace Falcor.Server
{
    public class Path: IPath
    {
        public Path(IEnumerable<IPathComponent> components)
        {
            Components = new List<IPathComponent>(components);
        }

        public Path(params IPathComponent[] components): this((IEnumerable<IPathComponent>)components)
        {            
        }

        public IList<IPathComponent> Components { get; }
    }
}