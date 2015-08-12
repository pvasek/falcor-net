namespace Falcor.Server
{
    public class RoutePathItem: IRoutePathItem
    {
        public RoutePathItem(string name, IPathItem item)
        {
            Name = name;
            Item = item;
        }

        public RoutePathItem(IPathItem item): this(null, item)
        {
        }

        public string Name { get; }
        public IPathItem Item { get; }
    }
}