using System.Collections.Generic;

namespace Falcor.Server
{
    public class Path: IPath
    {
        public Path(IEnumerable<IPathItem> items)
        {
            Items = new List<IPathItem>(items);
        }

        public Path(params IPathItem[] items): this((IEnumerable<IPathItem>)items)
        {            
        }

        public IList<IPathItem> Items { get; }
    }
}