using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class PathValue
    {
        public PathValue(object value, params IPathItem[] path)
        {
            Value = value;
            Path = new Path(path);
        }

        public IPath Path { get; set; }
        public object Value { get; set; }
    }
}