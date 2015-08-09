using System;
using System.Linq;

namespace Falcor.Server
{
    public static class PathExtensions
    {
        public static T As<T>(this IPath path, int index) where T : class, IPathComponent
        {
            var pathComponent = path.Components[index];
            var result = pathComponent as T;
            if (result == null)
            {
                throw new ArgumentException(string.Format("The path component with the index '{0}' is type of '{1}' not '{2}'.", index, pathComponent.GetType(), typeof(T)));
            }
            return result;
        }

        public static T As<T>(this IPath path, string name) where T : class, IPathComponent
        {
            var pathComponent = path.Components.FirstOrDefault(i => i.Name == name);
            if (pathComponent == null)
            {
                throw new ArgumentException(string.Format("The path component with the name '{0}' doesn't exist.", name));
            }
            var result = pathComponent as T;
            if (result == null)
            {
                throw new ArgumentException(string.Format("The path component with the name '{0}' is type of '{1}' not '{2}'.", name, pathComponent.GetType(), typeof(T)));
            }
            return result;
        }
    }
}