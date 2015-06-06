using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IRouteResolver
    {
        IEnumerable<Route> FindRoutes(IPath path);
    }
}