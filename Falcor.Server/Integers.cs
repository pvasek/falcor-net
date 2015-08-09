using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class Integers : IPathComponent
    {
        public Integers(IEnumerable<int> integers)
        {
            Values = integers.ToList();
        }

        public Integers(params int[] integers): this(null, integers)
        {            
        }

        public Integers(string name, params int[] integers)
        {
            Name = name;
            Values = integers ?? (IList<int>) new List<int>();
        }

        public static Integers Any(string name = null)
        {
            return new Integers(name);
        }

        public IList<int> Values { get; }

        public string Name { get; private set; }

        public object Value { get { return Values.FirstOrDefault(); } }

        public IEnumerable<object> AllKeys { get { return Values.Cast<object>(); } }

        public IPathComponent CloneAs(string name)
        {
            return new Integers(Values) { Name = name };
        }
    }
}