using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class PathValue
    {
        public static PathValue Create(object value, IPath path)
        {
            return Create(value, path.Items);
        }

        public static PathValue Create(object value, IEnumerable<IPathItem> path)
        {
            return Create(value, path.ToArray());
        }

        public static PathValue Create(object value, params IPathItem[] path)
        {
            return new PathValue
            {
                Value = value,
                Path = new Path(path)
            };
        }

        public IPath Path { get; set; }
        public object Value { get; set; }
    }
}