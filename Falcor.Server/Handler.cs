using System;
using System.Collections.Generic;
using Falcor.Server.Routing;

namespace Falcor.Server
{
    public delegate IObservable<Response> Handler(IList<IPathComponent> path);
}
