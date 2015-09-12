 using System.Collections.Generic;

namespace Falcor.Router
{
    public interface IUrlComponentParser
    {
        IList<IPathItem> ParseQueryString(string queryString);
    }
}