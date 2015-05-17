using System.Collections.Generic;

namespace Falcor.Server
{
    public class Response
    {
        public IDictionary<string, object> Data { get; set; } 
        public IList<object> Paths { get; set; } 
    }
}
