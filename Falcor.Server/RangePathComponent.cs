using System;
using System.Collections.Generic;

namespace Falcor.Server
{
    public class Range: IPathComponent
    {
        public Range(int? from = null, int? to = null)
        {
            From = from;
            To = to;
        }

        public Range(string name, int? from = null, int? to = null)
        {
            From = from;
            To = to;
        }

        public int? From { get; private set; }
        public int? To { get; private set; }

        public string Name { get; private set; }

        public object Value 
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<object> AllKeys { get { throw new NotImplementedException(); } }

        public IPathComponent CloneAs(string name)
        {
            return new Range(name, From, To);
        }
    }
}