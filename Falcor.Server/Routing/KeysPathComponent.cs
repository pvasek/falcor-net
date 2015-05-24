using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public class KeysPathComponent : IPathComponent
    {
        public KeysPathComponent(params string[] keys)
        {
            Keys = keys;
        }

        public IList<string> Keys { get; private set; }

        public override string ToString()
        {
            var property = Keys.FirstOrDefault();
            return property ?? "(unknown)";
        }
    }
}