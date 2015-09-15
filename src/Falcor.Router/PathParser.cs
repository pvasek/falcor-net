using System;
using System.Collections.Generic;
using System.Linq;
using Falcor.Router.Parser;

namespace Falcor.Router
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

        private IPathItem InputToPathComponent(object input)
        {
            var stringInput = input as string;
            if (stringInput != null)
            {
                return new Keys(stringInput);
            }

            if (input is int)
            {
                return new Integers((int)input);
            }

            var rangeInput = input as RangeValue;
            if (rangeInput != null)
            {
                var from = rangeInput.From ?? 0;
                var count = (rangeInput.To ?? 0) - from + 1;
                return new Integers(Enumerable.Range(from, count));
            }

            var list = input as IList<object>;
            if (list == null)
            {
                throw new ArgumentException($"Unknown path component for type {input.GetType()}");
            }

            if (list.All(i => i is string))
            {
                return new Keys(list.Cast<string>());
            }

            if (list.All(i => i is int))
            {
                return new Integers(list.Cast<int>());
            }

            throw new ArgumentException($"Unknown path component for list type {input.GetType()}");
        }
    }
}