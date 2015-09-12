using System.Collections.Generic;

namespace Falcor.Router
{
    public interface IResponseBuilder
    {
        Response CreateResponse(IList<PathValue> values);
    }
}