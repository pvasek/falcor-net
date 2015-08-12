using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Falcor.Server
{
    public class Route
    {
        public Route()
        {            
            Items = new List<IRoutePathItem>();
        }

        public Route(params IRoutePathItem[] items)
        {
            Items = items.ToList();         
        }

        public Route(params IPathItem[] items)
        {
            Items = items
                .Select(i => (IRoutePathItem)new RoutePathItem(i))
                .ToList();
        }

        public IList<IRoutePathItem> Items { get; }

        public RouteHandler RouteHandler { get; set; }

        public Task<IEnumerable<PathValue>> Execute(IPath path)
        {
            return RouteHandler(new RouteHandlerContext(path, Items));
        }
    }
}