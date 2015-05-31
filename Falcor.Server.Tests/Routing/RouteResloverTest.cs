using System.Linq;
using Falcor.Server.Routing;
using NUnit.Framework;

namespace Falcor.Server.Tests.Routing
{
    [TestFixture]
    public class RouteResloverTest
    {
        [Test]
        public void Should_match_route_with_index()
        {
            var route1 = new Route(new PropertiesPathComponent("events"));
            var route2 = new Route(new PropertiesPathComponent("users"), new RangePathComponent());
            var route3 = new Route(new PropertiesPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] {route1, route2, route3});
            var result = target.FindRoutes(new Path(
                    new PropertiesPathComponent("users"),
                    new IntegersPathComponent()))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route3, result[0]);
        }

        [Test]
        public void Should_match_single_property_with_list_of_properties()
        {
            var route1 = new Route(new PropertiesPathComponent("events"), new IntegersPathComponent(), new PropertiesPathComponent("name"));
            var route2 = new Route(new PropertiesPathComponent("events"), new IntegersPathComponent(), new PropertiesPathComponent("from", "to"));
            var route3 = new Route(new PropertiesPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(                
                    new PropertiesPathComponent("events"),
                    new IntegersPathComponent(),
                    new PropertiesPathComponent("from")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route2, result[0]);
        }

        [Test]
        public void Should_match_subpath()
        {
            var route1 = new Route(new PropertiesPathComponent("events"), new IntegersPathComponent());
            var route2 = new Route(new PropertiesPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2 });
            var result = target.FindRoutes(new Path(
                    new PropertiesPathComponent("events"),
                    new IntegersPathComponent(),
                    new PropertiesPathComponent("from")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route1, result[0]);
        }

        [Test]
        public void Should_match_list_of_properties_with_list_of_properties()
        {
            var route1 = new Route(new PropertiesPathComponent("events"), new IntegersPathComponent(), new PropertiesPathComponent("name"));
            var route2 = new Route(new PropertiesPathComponent("events"), new IntegersPathComponent(), new PropertiesPathComponent("from", "to"));
            var route3 = new Route(new PropertiesPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new PropertiesPathComponent("events"),
                    new IntegersPathComponent(),
                    new PropertiesPathComponent("to", "from")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route2, result[0]);
        }

        [Test]
        public void Should_match_list_of_properties_with_list_of_properties_and_simple_property()
        {
            var route1 = new Route(new PropertiesPathComponent("events"), new IntegersPathComponent(), new PropertiesPathComponent("name"));
            var route2 = new Route(new PropertiesPathComponent("events"), new IntegersPathComponent(), new PropertiesPathComponent("from", "to"));
            var route3 = new Route(new PropertiesPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new PropertiesPathComponent("events"),
                    new IntegersPathComponent(),
                    new PropertiesPathComponent("to", "name")))
                .ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(route1, result[0]);
            Assert.AreEqual(route2, result[1]);
        }

        [Test]
        public void Should_match_dictionary_key_with_simple_property()
        {
            var route1 = new Route(new PropertiesPathComponent("events"), new IntegersPathComponent(), new PropertiesPathComponent("name"));
            var route2 = new Route(new PropertiesPathComponent("eventById"), new KeysPathComponent(), new PropertiesPathComponent("from", "to"));
            var route3 = new Route(new PropertiesPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new PropertiesPathComponent("eventById"),
                    new KeysPathComponent(),
                    new PropertiesPathComponent("to", "name")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route2, result[0]);
        }
    }
}
