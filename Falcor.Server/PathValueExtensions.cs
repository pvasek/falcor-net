using System.Collections.Generic;

namespace Falcor.Server
{
    public static class PathValueExtensions
    {
        public static IEnumerable<PathValue> AsEnumerable(this PathValue value)
        {
            yield return value;
        }
    }
}