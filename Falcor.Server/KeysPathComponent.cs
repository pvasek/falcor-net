using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class KeysPathComponent : IPathComponent
    {
        public KeysPathComponent(IEnumerable<string> keys)
        {
            Keys = keys.ToList();
        }

        public KeysPathComponent(params string[] keys)
        {
            Keys = keys;
        }

        public IList<string> Keys { get; }

        public object Key { get { return Keys.FirstOrDefault(); } }
        public IEnumerable<object> AllKeys { get { return Keys; } }
    }
}