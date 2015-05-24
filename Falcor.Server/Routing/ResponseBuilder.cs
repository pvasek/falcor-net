using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Falcor.Server.Routing
{
    public class ResponseBuilder : IResponseBuilder
    {
        public Response CreateResponse(IList<PathValue> values)
        {
            var result = new Response();

            foreach (var pathValue in values)
            {
                AddPath(result.Data, pathValue.Path.Components, pathValue.Value);
            }

            result.Paths = values
                .Select(i => (IList<object>)i
                    .Path
                    .Components
                    .Select(j => j.Key)
                    .ToList())
                .ToList();

            return result;
        }

        private void AddPath(IDictionary<string,object> data, IList<IPathComponent> path, object value)
        {
            var key = path.First().Key.ToString();

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