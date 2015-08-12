 using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IUrlComponentParser
    {
        IList<IPathItem> ParseQueryString(string queryString);
    }
}