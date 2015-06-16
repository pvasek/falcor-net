using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class IntegersPathComponent : IPathComponent
    {
        public IntegersPathComponent(IEnumerable<int> integers)
        {
            Integers = integers.ToList();
        }

        public IntegersPathComponent(params int[] integers)
        {
            Integers = integers ?? (IList<int>) new List<int>();
        }

        public IList<int> Integers { get; }

        public object Key { get { return Integers.FirstOrDefault(); } }
        public IEnumerable<object> AllKeys { get { return Integers.Cast<object>(); } }
    }
}