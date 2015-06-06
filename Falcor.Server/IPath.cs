using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IPath
    {
        IList<IPathComponent> Components { get; } 
    }
}