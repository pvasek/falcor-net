using System.Collections.Generic;

namespace Falcor.Server.Extensions
{
    public static class SimpleRouteListExtensions
    {
        public static Route MapRoute(this IList<Route> list)
        {
            var result = new Route();
            list.Add(result);
            return result;
        }
    }
}
