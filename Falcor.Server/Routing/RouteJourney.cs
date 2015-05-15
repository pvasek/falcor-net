using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public class RouteJourney
    {
        public RouteJourney(Route route, IList<Route> routes)
        {
            Route = route;
            Routes = routes;
        }

        internal Route Route {get; set; }
        internal IList<Route> Routes { get; set; }        
    }
}