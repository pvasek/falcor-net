using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public class PropertyRouteJourney<T> : RouteJourney
    {
        public PropertyRouteJourney(Route route, IList<Route> routes) : base(route, routes)
        {
        }
    }
}