using System;
using System.Collections.Generic;

namespace Falcor.Server
{
    public class RangePathComponent: IPathComponent
    {
        public RangePathComponent(int? from = null, int? to = null)
        {
            From = from;
            To = to;
        }

        public int? From { get; private set; }
        public int? To { get; private set; }

        public object Key 
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<object> AllKeys { get { throw new NotImplementedException(); } }
    }
}