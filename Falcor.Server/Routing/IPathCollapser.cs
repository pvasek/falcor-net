using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public interface IPathCollapser
    {
        IEnumerable<IPath> Collapse(IEnumerable<IPath> paths);
    }
}