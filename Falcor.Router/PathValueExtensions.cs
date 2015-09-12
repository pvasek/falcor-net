using System.Collections.Generic;

namespace Falcor.Router
{
    public static class PathValueExtensions
    {
        public static IEnumerable<PathValue> AsEnumerable(this PathValue value)
        {
            yield return value;
        }
    }
}