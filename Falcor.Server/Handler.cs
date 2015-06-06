using System;

namespace Falcor.Server
{
    public delegate IObservable<PathValue> Handler(IPath path);
}
