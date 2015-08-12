using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Falcor.Server
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

        public Task<Response> Execute(params IPath[] paths)
        {
            return Execute((IEnumerable<IPath>)paths);
        }

        public async Task<Response> Execute(IEnumerable<IPath> paths)
        {
            var result = await GetPathValues(paths);
            var nextResults = result;
            
            while (true)
            {
                // select all which evaluted just partially 
                var nextPaths = nextResults
                    .Where(i => i.ForPath.Items.Count > i.Result.Path.Items.Count)
                    .Select(i => 
                        new Path(((Ref)i.Result.Value).Path
                            .Items
                            .Concat(i.ForPath.Items.Skip(i.Result.Path.Items.Count))
                            .ToArray()))
                    .ToList();

                if (nextPaths.Count == 0)
                {
                    break;
                }

                nextResults = await GetPathValues(nextPaths);
                result.AddRange(nextResults);
            }

            return _responseBuilder
                .CreateResponse(result
                    .Select(i => i.Result)
                    .ToList());
        }

        private async Task<List<PathEvaluationItem>> GetPathValues(IEnumerable<IPath> paths)
        {
            paths = _pathCollapser.Collapse(paths);

            var routeResultsWithPaths = paths
                .SelectMany(path =>
                    _routeResolver
                        .FindRoutes(path)
                        .Select(route => new
                        {
                            ExecutionTask = route.Execute(path),
                            Path = path
                        }))
                .ToList();

            await Task.WhenAll(routeResultsWithPaths.Select(i => i.ExecutionTask));

            var result = routeResultsWithPaths
                .Select(i => i.ExecutionTask
                    .Result
                    .Select(value => new PathEvaluationItem
                        {
                            Result = value,
                            ForPath = i.Path
                        }))
                .SelectMany(i => i.ToList())
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
