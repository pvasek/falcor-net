using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Falcor.Server
{
    public class PathParser : IPathParser
    {
        private readonly JsonSerializer _jsonSerializer;

        public PathParser()
        {
            _jsonSerializer = new JsonSerializer();
        }

        public IList<IPath> ParsePaths(string pathString)
        {
            
            var pathObj = _jsonSerializer.Deserialize(new JsonTextReader(new StringReader(pathString))) as JArray;
            if (pathObj == null)
            {
                throw new ArgumentException("path is not an array");
            }
            var paths = new List<IPath>();
            if (pathObj[0] is JArray)
            {
                paths.AddRange(pathObj.Select(i => ParsePath((JArray)i)));
            }
            else
            {
                paths.Add(ParsePath((pathObj)));
            }
            return paths;
        }

        private static IPath ParsePath(JArray array)
        {
            var components = new List<IPathComponent>();
            foreach (var jToken in array)
            {
                var token = (JValue)jToken;
                if (token.Type == JTokenType.String)
                {
                    components.Add(new PropertiesPathComponent(token.Value<string>()));
                }
                else if (token.Type == JTokenType.Integer)
                {
                    components.Add(new IntegersPathComponent(token.Value<int>()));
                }
            }
            return new Path(components.ToArray());
        }
    }
}