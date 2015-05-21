using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public interface IPath
    {
        IList<IPathComponent> Components { get; } 
    }
}