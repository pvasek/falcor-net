using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IPathParser
    {
        IList<IPath> ParsePaths(string pathString);
    }
}
