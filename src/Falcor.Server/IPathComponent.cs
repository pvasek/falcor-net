using System.Collections;
using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IPathComponent
    {
        object Key { get; }
        IEnumerable<object> AllKeys { get; }
    }
}