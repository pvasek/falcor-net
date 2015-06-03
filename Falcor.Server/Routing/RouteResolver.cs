using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
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
            var propertiesIntput = input as PropertiesPathComponent;
            var propertiesDefinition = definition as PropertiesPathComponent;

            if (propertiesDefinition != null)
            {
                if (propertiesIntput == null)
                {
                    return false;
                }

                var result = propertiesIntput.Properties.Any(i => propertiesDefinition.Properties.Any(j => j == i));
                return result;
            }

            var keyInput = input as KeysPathComponent;
            var keyDefinition = definition as KeysPathComponent;
            if (keyDefinition != null)
            {
                if (keyInput == null)
                {
                    return false;
                }

                return keyDefinition.Keys.Count == 0 || keyDefinition.Keys.Any(i => keyInput.Keys.Any(j => j == i));
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
