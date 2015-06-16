using System;
using System.Collections.Generic;
using System.Linq;
using Falcor.Server.Parser;

namespace Falcor.Server
{
    public class PathParser : IPathParser
    {        
        public IList<IPath> ParsePaths(string pathString)
        {
            var input = InputPathParser.ParseInput(pathString);
            var includeMultiplePaths = input.All(i => i is IList<object>);
            var paths = includeMultiplePaths ? 
                (IEnumerable<IList<object>>)input.Cast<IList<object>>() 
                : new List<IList<object>>{input};
        
            return paths
                .Select(i => (IPath)new Path(i.Select(InputToPathComponent)))
                .ToList();
        }

        private IPathComponent InputToPathComponent(object input)
        {
            var stringInput = input as string;
            if (stringInput != null)
            {
                return new KeysPathComponent(stringInput);
            }

            if (input is int)
            {
                return new IntegersPathComponent((int)input);
            }

            var rangeInput = input as Range;
            if (rangeInput != null)
            {
                return new RangePathComponent(rangeInput.From, rangeInput.To);
            }

            var list = input as IList<object>;
            if (list == null)
            {
                throw new ArgumentException($"Unknown path component for type {input.GetType()}");
            }

            if (list.All(i => i is string))
            {
                return new KeysPathComponent(list.Cast<string>());
            }

            if (list.All(i => i is int))
            {
                return new IntegersPathComponent(list.Cast<int>());
            }

            throw new ArgumentException($"Unknown path component for list type {input.GetType()}");
        }
    }
}