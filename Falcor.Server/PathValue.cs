using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class PathValue
    {
        public static PathValue Create(object value, params string[] path)
        {
            return new PathValue
            {
                Value = value,
                Path = path.ToList()
            };
        }

        public IList<string> Path { get; set; }
        public object Value { get; set; }
    }
}