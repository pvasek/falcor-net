using System.Collections;
using System.Collections.Generic;

namespace Falcor.Router
{
    public interface IPathItem
    {
        object Value { get; }
        IEnumerable<object> AllKeys { get; }
    }
}