using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcor.Router
{
    public delegate Task<IEnumerable<PathValue>> RouteHandler(RouteHandlerContext ctx);
}
