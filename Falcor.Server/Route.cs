using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Falcor.Server
{
    public class Route
    {
        public Route()
        {
            Path = new Path();
        }

        public Route(params IPathComponent[] pathComponents)
        {
            Path = new Path(pathComponents);
        }

        public IPath Path { get; private set; }
        public Handler Handler { get; set; }

        public Task<IEnumerable<PathValue>> Execute(IPath path)
        {
            var pathComponents = path
                .Components
                .Zip(Path.Components, (i, j) => new {inputComponent = i, defineComponent = j})
                .Select(i => i.inputComponent.CloneAs(i.defineComponent.Name));

            return Handler(new Path(pathComponents));
        }
    }
}