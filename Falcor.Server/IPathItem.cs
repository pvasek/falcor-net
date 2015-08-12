using System.Collections;
using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IPathItem
    {
        object Value { get; }
        IEnumerable<object> AllKeys { get; }
    }
}