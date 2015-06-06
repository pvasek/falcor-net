using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Falcor.Server
{
    public class ResponseSerializer: IResponseSerializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public ResponseSerializer()
        {
            _jsonSerializer = new JsonSerializer();
        }

        public string Serialize(Response response)
        {
            var stringWriter = new StringWriter();
            _jsonSerializer.Serialize(stringWriter, response);
            return stringWriter.ToString();
        }
    }
}