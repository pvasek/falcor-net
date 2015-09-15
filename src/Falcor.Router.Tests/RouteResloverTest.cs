using System.Linq;
using NUnit.Framework;

namespace Falcor.Router.Tests
{
    [TestFixture]
    public class RouteResloverTest
    {
        [Test]
        public void Should_match_route_with_index()
        {
            var route1 = new Route(new Keys("events"));
            var route2 = new Route(new Keys("users"), new Range());
            var route3 = new Route(new Keys("users"), new Integers());

            var target = new RouteResolver(new[] {route1, route2, route3});
            var result = target.FindRoutes(new Path(
                    new Keys("users"),
                    new Integers()))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route3, result[0]);
        }

        [Test]
        public void Should_match_single_property_with_list_of_properties()
        {
            var route1 = new Route(new Keys("events"), new Integers(), new Keys("name"));
            var route2 = new Route(new Keys("events"), new Integers(), new Keys("from", "to"));
            var route3 = new Route(new Keys("users"), new Integers());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(                
                    new Keys("events"),
                    new Integers(),
                    new Keys("from")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route2, result[0]);
        }

        [Test]
        public void Should_match_subpath()
        {
            var route1 = new Route(new Keys("events"), new Integers());
            var route2 = new Route(new Keys("users"), new Integers());

            var target = new RouteResolver(new[] { route1, route2 });
            var result = target.FindRoutes(new Path(
                    new Keys("events"),
                    new Integers(),
                    new Keys("from")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route1, result[0]);
        }

        [Test]
        public void Should_match_list_of_properties_with_list_of_properties()
        {
            var route1 = new Route(new Keys("events"), new Integers(), new Keys("name"));
            var route2 = new Route(new Keys("events"), new Integers(), new Keys("from", "to"));
            var route3 = new Route(new Keys("users"), new Integers());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new Keys("events"),
                    new Integers(),
                    new Keys("to", "from")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route2, result[0]);
        }

        [Test]
        public void Should_match_list_of_properties_with_list_of_properties_case_intensitive()
        {
            var route1 = new Route(new Keys("events"), new Integers(), new Keys("name"));
            var route2 = new Route(new Keys("events"), new Integers(), new Keys("From", "To"));
            var route3 = new Route(new Keys("users"), new Integers());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new Keys("events"),
                    new Integers(),
                    new Keys("to", "from")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route2, result[0]);
        }

        [Test]
        public void Should_match_list_of_properties_with_list_of_properties_and_simple_property()
        {
            var route1 = new Route(new Keys("events"), new Integers(), new Keys("name"));
            var route2 = new Route(new Keys("events"), new Integers(), new Keys("from", "to"));
            var route3 = new Route(new Keys("users"), new Integers());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new Keys("events"),
                    new Integers(),
                    new Keys("to", "name")))
                .ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(route1, result[0]);
            Assert.AreEqual(route2, result[1]);
        }

        [Test]
        public void Should_match_dictionary_key_with_simple_property()
        {
            var route1 = new Route(new Keys("events"), new Integers(), new Keys("name"));
            var route2 = new Route(new Keys("eventById"), Keys.Any(), new Keys("from", "to"));
            var route3 = new Route(new Keys("users"), new Integers());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new Keys("eventById"),
                    Keys.Any(),
                    new Keys("to", "name")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route2, result[0]);
        }

        [Test]
        public void Should_match_dictionary_key_with_simple_property_where_key_can_be_integer()
        {
            var route1 = new Route(new Keys("events"), new Integers(), new Keys("name"));
            var route2 = new Route(new Keys("eventById"), Keys.Any(), new Keys("from", "to"));
            var route3 = new Route(new Keys("users"), new Integers());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new Keys("eventById"),
                    new Integers(1),
                    new Keys("to", "name")))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route2, result[0]);
        }

        [Test]
        public void Should_match_index_with_indexes()
        {
            var route1 = new Route(new Keys("events"), new Integers(0, 1, 2, 3, 4, 5));

            var target = new RouteResolver(new[] { route1 });
            var result = target.FindRoutes(new Path(
                    new Keys("events"),
                    new Integers()))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route1, result[0]);
        }

        [Test]
        public void Should_match_route_with_index_case_insensitive()
        {
            var route1 = new Route(new Keys("events"));
            var route2 = new Route(new Keys("users"), new Range());
            var route3 = new Route(new Keys("users"), new Integers());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new Keys("Users"),
                    new Integers()))
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(route3, result[0]);
        }
    }
}
