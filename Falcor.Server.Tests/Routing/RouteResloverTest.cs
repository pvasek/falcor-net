using System.Collections.Generic;
using System.Linq;
using Falcor.Server.Routing;
using NUnit.Framework;

namespace Falcor.Server.Tests.Routing
{
    [TestFixture]
    public class RouteResloverTest
    {
        [Test]
        public void Should_match_simple_route()
        {
            var route1 = new Route(new PropertiesPathComponent("events"));
            var route2 = new Route(new PropertiesPathComponent("users"), new RangePathComponent());
            var route3 = new Route(new PropertiesPathComponent("users"), new IndexPathComponent());

            var target = new RouteResolver(new[] {route1, route2, route3});
            var result = target.FindRoute(new IPathComponent[]
                {
                    new PropertiesPathComponent("users"),
                    new IndexPathComponent(),
                })
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route3, result[0]);
        }
    }
}
