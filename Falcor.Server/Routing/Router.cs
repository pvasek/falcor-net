using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Falcor.Server.Routing
{
    public class Router
    {
        private readonly IRouteResolver _routeResolver;
        
        public Router(IRouteResolver routeResolver)
        {
            _routeResolver = routeResolver;
        }

        public Response Execute(IList<IPath> paths)
        {
            //TODO: compact paths first
            var routeWithPaths = paths.SelectMany(path => 
                _routeResolver
                    .FindRoutes(path)
                    .Select(route => new 
                        {
                            Route = route,
                            Path = path
                        }
                    ));

            // first try it synchronously, we can solve RX things later
            var result = routeWithPaths
                .Select(i => i.Route.Handler(i.Path))
                .Concat()
                .ToEnumerable()
                .ToList();

            // TODO:
            // 1 - create paths
            // 2 - compact paths
            // 3 - resolve references
            // 4 - after refactoring 3/4 will be calling the same/similar method as we have above
            // 5 - put everything together and transform it to the response object

            return new Response();
        }
    }
}
