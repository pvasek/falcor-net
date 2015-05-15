using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public class ListRouteJourney<T> : PropertyRouteJourney<T>
    {
        public ListRouteJourney(Route route, IList<Route> routes)
            : base(route, routes)
        {
        }
    }
}