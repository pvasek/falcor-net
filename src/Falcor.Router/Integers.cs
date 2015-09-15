using System.Collections.Generic;
using System.Linq;

namespace Falcor.Router
{
    public class Integers : IPathItem
    {
        public Integers(IEnumerable<int> integers)
        {
            Values = integers.ToList();
        }

        public Integers(params int[] integers): this(integers.AsEnumerable())
        {            
        }
     

        public IList<int> Values { get; }
        
        public object Value { get { return Values.FirstOrDefault(); } }

        public IEnumerable<object> AllKeys { get { return Values.Cast<object>(); } }

        public static Integers Any()
        {
            return new Integers();
        }
        public static Integers For(params int[] integers)
        {
            return new Integers(integers);
        }
    }
}