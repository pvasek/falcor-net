using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public class RouteResolver
    {
        private readonly List<Route> _routes;

        public RouteResolver(IEnumerable<Route> routes)
        {
            _routes = routes.ToList();
        }

        public IEnumerable<Route> FindRoute(IList<IPathComponent> path)
        {
            return _routes.Where(i => Match(i.Path, path));
        }

        public static bool Match(IList<IPathComponent> input, IList<IPathComponent> definition)
        {
            if (input.Count != definition.Count)
            {
                return false;
            }

            var result = input.Zip(definition, Match).All(i => i);
            return result;
        }

        public static bool Match(IPathComponent input, IPathComponent definition)
        {
            var propertiesIntput = input as PropertiesPathComponent;
            var propertiesDefinition = definition as PropertiesPathComponent;

            if (propertiesDefinition != null)
            {
                if (propertiesIntput == null)
                {
                    return false;
                }

                var result = propertiesIntput.Keys.Any(i => propertiesDefinition.Keys.Any(j => j == i));
                return result;
            }

            var rangeInput = input as RangePathComponent;
            var rangeDefinition = definition as RangePathComponent;
            if (rangeDefinition != null)
            {
                return rangeInput != null;
            }

            var indexInput = input as IndexesPathComponent;
            var indexDefinition = definition as IndexesPathComponent;
            return indexInput != null && indexDefinition != null;
        }
    }
}
