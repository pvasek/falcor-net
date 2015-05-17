using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public class RouteResolver
    {
        private List<Route> _routes;

        public RouteResolver(IEnumerable<Route> routes)
        {
            _routes = routes.ToList();
        }

        public IEnumerable<Route> FindRoute(IList<IPathComponent> path)
        {

            return null;
        }

        public static bool Match(IPathComponent input, IPathComponent definition)
        {
            var propertyInput = input as PropertiesPathComponent;
            var propertyDefinition = definition as PropertiesPathComponent;

            if (propertyDefinition != null)
            {
                //return propertyInput != null && propertyInput.Key == propertyDefinition.Key;
            }

            var listInput = input as RangePathComponent;
            var listDefinition = definition as RangePathComponent;
            if (listDefinition != null)
            {
                //return listInput != null && listInput.Key == listDefinition.Key;
            }

            return false;
        }
    }
}
