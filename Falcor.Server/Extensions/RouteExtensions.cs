namespace Falcor.Server.Extensions
{
    public static class RouteExtensions
    {
        public static Route PathItem(this Route route, string name, IPathItem item)
        {
            route.Items.Add(new RoutePathItem(name, item));
            return route;
        }

        public static Route PathItem(this Route route, IPathItem item)
        {            
            return PathItem(route, null, item);
        }

        public static void To(this Route route, RouteHandler routeHandler)
        {
            route.RouteHandler = routeHandler;
        }
    }
}