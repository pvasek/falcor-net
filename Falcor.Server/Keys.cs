using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class Keys : IPathItem
    {
        public Keys(IEnumerable<string> keys)
        {
            Values = keys.ToList();
        }

        public Keys(params string[] values)
        {
            Values = values;
        }

        public IList<string> Values { get; }

        public object Value => Values.FirstOrDefault();

        public IEnumerable<object> AllKeys => Values;

        public bool HasKey(string key)
        {
            return Values.Contains(key);
        }

        public static Keys Any()
        {
            return new Keys();
        }

        public static Keys For(params string[] keys)
        {
            return new Keys(keys);
        }
    }
}