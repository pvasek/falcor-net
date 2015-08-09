using System.Collections;
using System.Collections.Generic;

namespace Falcor.Server
{
    public interface IPathComponent
    {
        string Name { get; }
        object Value { get; }
        IEnumerable<object> AllKeys { get; }
        IPathComponent CloneAs(string name);
    }
}