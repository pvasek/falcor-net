using System.Collections.Generic;

namespace Falcor.Router
{
    public interface IPathCollapser
    {
        IEnumerable<IPath> Collapse(IEnumerable<IPath> paths);
    }
}