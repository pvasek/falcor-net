using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Falcor.Server
{
    public class RouteResolver: IRouteResolver
    {
        private readonly List<Route> _routes;

        public RouteResolver(IEnumerable<Route> routes)
        {
            _routes = routes.ToList();
        }

        public IEnumerable<Route> FindRoutes(IPath path)
        {
            return _routes.Where(i => Match(path.Components, i.Path.Components));
        }

        private static bool Match(IList<IPathComponent> input, IList<IPathComponent> definition)
        {
            if (input.Count < definition.Count)
            {
                return false;
            }

            var result = definition.Zip(input, MatchComponent).All(i => i);
            return result;
        }

        private static bool MatchComponent(IPathComponent definition, IPathComponent input)
        {
            var keysInput = input as KeysPathComponent;
            var keysDefinition = definition as KeysPathComponent;

            if (keysDefinition != null)
            {
                if (keysInput == null)
                {
                    return false;
                }

                return keysDefinition.Keys.Count == 0 
                    || keysDefinition.Keys.Any(i => keysInput.Keys.Any(j => string.Compare(j, i, false, CultureInfo.InvariantCulture) == 0));
            }            

            var rangeInput = input as RangePathComponent;
            var rangeDefinition = definition as RangePathComponent;
            if (rangeDefinition != null)
            {
                return rangeInput != null;
            }

            var indexInput = input as IntegersPathComponent;
            var indexDefinition = definition as IntegersPathComponent;
            return indexInput != null && indexDefinition != null;
        }
    }
}
