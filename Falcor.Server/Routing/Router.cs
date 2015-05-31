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
        private readonly IResponseBuilder _responseBuilder;

        public Router(IRouteResolver routeResolver, IPathCollapser pathCollapser, IResponseBuilder responseBuilder)
        {
            _routeResolver = routeResolver;
            _pathCollapser = pathCollapser;
            _responseBuilder = responseBuilder;
        }

        public Response Execute(params IPath[] paths)
        {
            return Execute((IEnumerable<IPath>)paths);
        }

        public Response Execute(IEnumerable<IPath> paths)
        {
            var result = GetPathValues(paths);
            var nextResults = result;
            
            while (true)
            {
                //TODO: the route can match only half of the path we need to evalutate the rest

                // select all which evaluted just partially 
                var nextPaths = nextResults
                    .Where(i => i.ForPath.Components.Count > i.Result.Path.Components.Count)
                    .Select(i => 
                        new Path(((Ref)i.Result.Value).Path
                            .Components
                            .Concat(i.ForPath.Components.Skip(i.Result.Path.Components.Count))
                            .ToArray()))
                    .ToList();

                if (nextPaths.Count == 0)
                {
                    break;
                }

                nextResults = GetPathValues(nextPaths);
                result.AddRange(nextResults);
            }

            return _responseBuilder
                .CreateResponse(result
                    .Select(i => i.Result)
                    .ToList());
        }

        private List<PathEvaluationItem> GetPathValues(IEnumerable<IPath> paths)
        {
            paths = _pathCollapser.Collapse(paths);

            var routeWithPaths = paths.SelectMany(path =>
                _routeResolver
                    .FindRoutes(path)
                    .Select(route => new
                    {
                        Route = route,
                        Path = path
                    }));

            // first try it synchronously, we can solve RX things later
            var result = routeWithPaths
                .Select(i => i.Route
                    .Handler(i.Path)
                    .Zip(Observable.Repeat(i.Path), 
                        (value, path) => new PathEvaluationItem
                        {
                            Result = value,
                            ForPath = path
                        }))
                .Concat()
                .ToEnumerable()
                .ToList();

            return result;
        }

        private class PathEvaluationItem
        {
            public PathValue Result { get; set; }
            public IPath ForPath { get; set; }
        }
    }
}
