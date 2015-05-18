using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public class KeysPathComponent : IPathComponent
    {
        public KeysPathComponent(params string[] keys)
        {
            Keys = keys;
        }

        public IList<string> Keys { get; private set; }
    }
}