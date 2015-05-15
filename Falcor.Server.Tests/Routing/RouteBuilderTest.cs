using System.Collections.Generic;
using Falcor.Server.Routing;
using NUnit.Framework;

namespace Falcor.Server.Tests.Routing
{
    [TestFixture]
    public class RouteBuilderTest
    {
        [Test]
        public void Shoudl_map_simple_property()
        {
            var routes = new List<Route>();
            routes.MapRoute<Model>().Property(i => i.Name).To(() => null);
        }
    }

    public class Model
    {
        public string Name { get; set; }   
    }
}
