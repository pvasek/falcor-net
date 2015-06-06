using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IResponseBuilder
    {
        Response CreateResponse(IList<PathValue> values);
    }
}