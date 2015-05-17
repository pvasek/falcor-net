using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public class PropertiesPathComponent : IPathComponent
    {
        public PropertiesPathComponent()
        {            
        }

        public PropertiesPathComponent(params string[] keys)
        {
            Keys = keys.ToList();
        }

        public IList<string> Keys { get; set; }
    }
}