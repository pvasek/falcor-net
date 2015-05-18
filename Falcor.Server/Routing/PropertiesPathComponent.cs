using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public class PropertiesPathComponent : IPathComponent
    {
        public PropertiesPathComponent()
        {            
        }

        public PropertiesPathComponent(params string[] properties)
        {
            Properties = properties;
        }

        public IList<string> Properties { get; private set; }
    }
}