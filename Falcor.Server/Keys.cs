using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class Keys : IPathComponent
    {
        public Keys(IEnumerable<string> keys)
        {
            Values = keys.ToList();
        }

        public Keys(params string[] values)
        {
            Values = values;
        }

        public static Keys Any(string name = null)
        {
            return new Keys { Name = name };
        }

        public IList<string> Values { get; }

        public string Name { get; private set; }

        public object Value { get { return Values.FirstOrDefault(); } }

        public IEnumerable<object> AllKeys { get { return Values; } }

        public IPathComponent CloneAs(string name)
        {
            return new Keys(Values) { Name = name };
        }
    }
}