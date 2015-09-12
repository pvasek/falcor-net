using System.Collections.Generic;

namespace Falcor.Router
{
    public interface IRouteResolver
    {
        IEnumerable<Route> FindRoutes(IPath path);
    }
}