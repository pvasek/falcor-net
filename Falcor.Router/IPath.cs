using System.Collections.Generic;

namespace Falcor.Router
{
    public interface IPath
    {
        IList<IPathItem> Items { get; } 
    }
}