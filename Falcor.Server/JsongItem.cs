using Newtonsoft.Json;

namespace Falcor.Server
{
    public class JsongItem
    {
        [JsonProperty("$type")]
        public JsongItemType Type { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("$size")]
        public int Size { get; set; }
    }
}