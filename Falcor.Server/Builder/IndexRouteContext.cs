using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            get { return ((IntegersPathComponent) Path.Components[_index]).Integers; }
        }

        public PathValue CreateResult(int index, object value)
        {
            var pathComponents = Path.Components
                .Take(Path.Components.Count - 2)
                .ToList();

            pathComponents.Add(new IntegersPathComponent(index));

            return PathValue.Create(value, pathComponents);
        }
    }
}