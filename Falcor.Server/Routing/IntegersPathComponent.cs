using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public class IntegersPathComponent : IPathComponent
    {
        public IntegersPathComponent(params int[] integers)
        {
            Integers = integers;
        }

        public IList<int> Integers { get; private set; }

        public object Key { get { return Integers.FirstOrDefault(); } }
    }
}