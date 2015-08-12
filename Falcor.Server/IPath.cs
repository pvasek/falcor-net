using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IPath
    {
        IList<IPathItem> Items { get; } 
    }
}