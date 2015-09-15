using System.Collections.Generic;

namespace Falcor.Router
{
    public interface IPathParser
    {
        IList<IPath> ParsePaths(string pathString);
    }
}
