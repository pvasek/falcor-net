using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public class PropertiesPathFragment : PathFragment
    {
        public PropertiesPathFragment()
        {            
        }

        public PropertiesPathFragment(params string[] keys)
        {
            Keys = keys.ToList();
        }

        public IList<string> Keys { get; set; }
    }
}