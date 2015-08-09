using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Falcor.Server.Builder
{
    public class IndexFalcorRequest<TModel>: FalcorRequest<TModel>
    {
        private readonly int _index;

        public IndexFalcorRequest(IPath path, IPath originalPath) : base(path)
        {
            _index = originalPath.Components.Count - 1;
        }

        public IList<int> Indexes
        {
            get { return ((Integers) Path.Components[_index]).Values; }
        }

        public PathValue CreateResult(int index, object value)
        {
            var pathComponents = Path.Components
                .Take(Path.Components.Count - 2)
                .ToList();

            pathComponents.Add(new Integers(null, index));

            return PathValue.Create(value, pathComponents);
        }
    }
}