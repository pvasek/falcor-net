using System;
using Falcor.Server.Routing;

namespace Falcor.Server
{
    public delegate IObservable<PathValue> Handler(IPath path);
}
