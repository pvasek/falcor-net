using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public interface IResponseBuilder
    {
        Response CreateResponse(IList<PathValue> values);
    }
}