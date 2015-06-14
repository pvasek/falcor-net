using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class PropertiesPathComponent2 : IPathComponent
    {
        public PropertiesPathComponent2(params string[] properties)
        {
            Properties = properties;
        }

        public IList<string> Properties { get; }

        public object Key { get { return Properties.FirstOrDefault(); } }
        public IEnumerable<object> AllKeys { get { return Properties; } }
    }
}