using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class RouteHandlerContext
    {
        public RouteHandlerContext(IPath path, IList<IRoutePathItem> routePathItems)
        {
            Path = path;
            _items = routePathItems
                .Zip(Path.Items, (routePathItem, item) => 
                    new
                    {
                        RoutePathItem = routePathItem,
                        Item = item
                    }
                )
               .Where(i => i.RoutePathItem.Name != null)
               .ToDictionary(i => i.RoutePathItem.Name, i => i.Item);
        }

        private readonly Dictionary<string, IPathItem> _items;
    
        public IPath Path { get; }

        public T Item<T>(string name) where T : IPathItem
        {
            return (T) _items[name];
        }
    }
}