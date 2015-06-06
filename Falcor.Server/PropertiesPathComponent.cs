using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public class PropertiesPathComponent : IPathComponent
    {
        public PropertiesPathComponent(params string[] properties)
        {
            Properties = properties;
        }

        public IList<string> Properties { get; }

        public object Key { get { return Properties.FirstOrDefault(); } }
    }
}