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
            return _routes.Where(i => Match(i.Path, path.Components));
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

                var result = propertiesIntput.Properties.Any(i => propertiesDefinition.Properties.Any(j => j == i));
                return result;
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
