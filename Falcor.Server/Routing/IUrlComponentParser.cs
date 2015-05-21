 using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public interface IUrlComponentParser
    {
        IList<IPathComponent> ParseQueryString(string queryString);
    }
}