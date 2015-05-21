using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public interface IRouteResolver
    {
        IEnumerable<Route> FindRoutes(IPath path);
    }
}