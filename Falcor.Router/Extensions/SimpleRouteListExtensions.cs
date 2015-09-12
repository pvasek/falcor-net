using System.Collections.Generic;

namespace Falcor.Router.Extensions
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
