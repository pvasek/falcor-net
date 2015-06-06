using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IPathCollapser
    {
        IEnumerable<IPath> Collapse(IEnumerable<IPath> paths);
    }
}