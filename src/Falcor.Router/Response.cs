using System.Collections.Generic;
using Newtonsoft.Json;

namespace Falcor.Router
{
    public class Response
    {
        public Response()
        {
            Data = new Dictionary<string, object>();
            Paths = new List<IList<object>>();
        }
            
        [JsonProperty("jsong")]
        public IDictionary<string, object> Data { get; private set; } 

        [JsonProperty("paths")]
        public IList<IList<object>> Paths { get; set; } 
    }
}
