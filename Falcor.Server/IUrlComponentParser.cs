 using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IUrlComponentParser
    {
        IList<IPathComponent> ParseQueryString(string queryString);
    }
}