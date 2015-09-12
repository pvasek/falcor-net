using System;
using System.Collections.Generic;

namespace Falcor.Router
{
    public class Range: IPathItem
    {
        public Range(int? from = null, int? to = null)
        {
            From = from;
            To = to;
        }

        public int? From { get; private set; }
        public int? To { get; private set; }

        public object Value 
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<object> AllKeys { get { throw new NotImplementedException(); } }
    }
}