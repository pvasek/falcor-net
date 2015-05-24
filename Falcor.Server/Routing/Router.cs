using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Falcor.Server.Routing
{
    public class Router
    {
        private readonly IRouteResolver _routeResolver;
        private readonly IPathCollapser _pathCollapser;

        public Router(IRouteResolver routeResolver, IPathCollapser pathCollapser)
        {
            _routeResolver = routeResolver;
            _pathCollapser = pathCollapser;
        }

        public Response Execute(IEnumerable<IPath> paths)
        {
            var result = GetPathValues(paths);
            var possibleReferences = result;
            
            while (true)
            {
                var references = possibleReferences
                    .Where(i => i.Value is Ref)
                    .Select(i => ((Ref)i.Value).Path)
                    .ToList();

                if (possibleReferences.Count == 0)
                {
                    break;
                }

                possibleReferences = GetPathValues(references);
                result.AddRange(possibleReferences);
            }

            // TODO:
            // 5 - put everything together and transform it to the response object

            return new Response();
        }

        private List<PathValue> GetPathValues(IEnumerable<IPath> paths)
        {
            paths = _pathCollapser.Collapse(paths);

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
            return result;
        }
    }
}
