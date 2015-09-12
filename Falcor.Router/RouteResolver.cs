using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Falcor.Router
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
            return _routes.Where(i => Match(path.Items, i.Items));
        }

        private static bool Match(IList<IPathItem> input, IList<IRoutePathItem> definition)
        {
            if (input.Count < definition.Count)
            {
                return false;
            }

            var result = definition.Zip(input, (def, inp) => MatchComponent(def.Item, inp)).All(i => i);
            return result;
        }

        private static bool MatchComponent(IPathItem definition, IPathItem input)
        {
            var keysInput = input as Keys;
            var indexInput = input as Integers;

            var keysDefinition = definition as Keys;

            if (keysDefinition != null)
            {
                if (keysInput == null)
                {
                    if (indexInput != null && indexInput.Values.Count == 1)
                    {
                        keysInput = new Keys(indexInput.Values[0].ToString());
                    }
                    else
                    {
                        return false;
                    }
                }

                return keysDefinition.Values.Count == 0 
                    || keysDefinition.Values.Any(i => keysInput.Values.Any(j => string.Compare(j, i, true, CultureInfo.InvariantCulture) == 0));
            }            

            var rangeInput = input as Range;
            var rangeDefinition = definition as Range;
            if (rangeDefinition != null)
            {
                return rangeInput != null;
            }

            var indexDefinition = definition as Integers;
            return indexInput != null && indexDefinition != null;
        }
    }
}
