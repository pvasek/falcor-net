using System.Collections.Generic;
using System.Threading.Tasks;
using Falcor.Server.Routing;

namespace Falcor.Server
{
    public delegate Task<Response> Handler(IList<IPathComponent> path);
}
