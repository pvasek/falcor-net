using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class ResponseBuilder : IResponseBuilder
    {
        public Response CreateResponse(IList<PathValue> values)
        {
            var result = new Response();

            foreach (var pathValue in values)
            {
                AddPath(result.Data, pathValue.Path.Items, pathValue.Value);
            }

            result.Paths = values
                .Select(i => (IList<object>)i
                    .Path
                    .Items
                    .Select(j => j.Value)
                    .ToList())
                .ToList();

            return result;
        }

        private void AddPath(IDictionary<string,object> data, IList<IPathItem> path, object value)
        {
            var key = path.First().Value.ToString();

            if (path.Count() == 1)
            {
                data[key] = value;
            }
            else
            {
                IDictionary<string, object> nextData;
                if (!data.ContainsKey(key))
                {
                    nextData = new Dictionary<string, object>();
                    data.Add(key, nextData);
                }
                else
                {
                    nextData = (IDictionary<string, object>) data[key];
                }
                AddPath(nextData, path.Skip(1).ToList(), value);
            }
        }
    }
}