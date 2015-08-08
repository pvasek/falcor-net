using System.Linq;
using Xunit;

namespace Falcor.Server.Tests
{
    public class RouteResloverTest
    {
        [Fact]
        public void Should_match_route_with_index()
        {
            var route1 = new Route(new KeysPathComponent("events"));
            var route2 = new Route(new KeysPathComponent("users"), new RangePathComponent());
            var route3 = new Route(new KeysPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] {route1, route2, route3});
            var result = target.FindRoutes(new Path(
                    new KeysPathComponent("users"),
                    new IntegersPathComponent()))
                .ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal(route3, result[0]);
        }

        [Fact]
        public void Should_match_single_property_with_list_of_properties()
        {
            var route1 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("name"));
            var route2 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("from", "to"));
            var route3 = new Route(new KeysPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(                
                    new KeysPathComponent("events"),
                    new IntegersPathComponent(),
                    new KeysPathComponent("from")))
                .ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal(route2, result[0]);
        }

        [Fact]
        public void Should_match_subpath()
        {
            var route1 = new Route(new KeysPathComponent("events"), new IntegersPathComponent());
            var route2 = new Route(new KeysPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2 });
            var result = target.FindRoutes(new Path(
                    new KeysPathComponent("events"),
                    new IntegersPathComponent(),
                    new KeysPathComponent("from")))
                .ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal(route1, result[0]);
        }

        [Fact]
        public void Should_match_list_of_properties_with_list_of_properties()
        {
            var route1 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("name"));
            var route2 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("from", "to"));
            var route3 = new Route(new KeysPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new KeysPathComponent("events"),
                    new IntegersPathComponent(),
                    new KeysPathComponent("to", "from")))
                .ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal(route2, result[0]);
        }

        [Fact]
        public void Should_match_list_of_properties_with_list_of_properties_case_intensitive()
        {
            var route1 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("name"));
            var route2 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("From", "To"));
            var route3 = new Route(new KeysPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new KeysPathComponent("events"),
                    new IntegersPathComponent(),
                    new KeysPathComponent("to", "from")))
                .ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal(route2, result[0]);
        }

        [Fact]
        public void Should_match_list_of_properties_with_list_of_properties_and_simple_property()
        {
            var route1 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("name"));
            var route2 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("from", "to"));
            var route3 = new Route(new KeysPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new KeysPathComponent("events"),
                    new IntegersPathComponent(),
                    new KeysPathComponent("to", "name")))
                .ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(route1, result[0]);
            Assert.Equal(route2, result[1]);
        }

        [Fact]
        public void Should_match_dictionary_key_with_simple_property()
        {
            var route1 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("name"));
            var route2 = new Route(new KeysPathComponent("eventById"), new KeysPathComponent(), new KeysPathComponent("from", "to"));
            var route3 = new Route(new KeysPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new KeysPathComponent("eventById"),
                    new KeysPathComponent(),
                    new KeysPathComponent("to", "name")))
                .ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal(route2, result[0]);
        }

        [Fact]
        public void Should_match_dictionary_key_with_simple_property_where_key_can_be_integer()
        {
            var route1 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(), new KeysPathComponent("name"));
            var route2 = new Route(new KeysPathComponent("eventById"), new KeysPathComponent(), new KeysPathComponent("from", "to"));
            var route3 = new Route(new KeysPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new KeysPathComponent("eventById"),
                    new IntegersPathComponent(1),
                    new KeysPathComponent("to", "name")))
                .ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal(route2, result[0]);
        }

        [Fact]
        public void Should_match_index_with_indexes()
        {
            var route1 = new Route(new KeysPathComponent("events"), new IntegersPathComponent(0, 1, 2, 3, 4, 5));

            var target = new RouteResolver(new[] { route1 });
            var result = target.FindRoutes(new Path(
                    new KeysPathComponent("events"),
                    new IntegersPathComponent()))
                .ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal(route1, result[0]);
        }

        [Fact]
        public void Should_match_route_with_index_case_insensitive()
        {
            var route1 = new Route(new KeysPathComponent("events"));
            var route2 = new Route(new KeysPathComponent("users"), new RangePathComponent());
            var route3 = new Route(new KeysPathComponent("users"), new IntegersPathComponent());

            var target = new RouteResolver(new[] { route1, route2, route3 });
            var result = target.FindRoutes(new Path(
                    new KeysPathComponent("Users"),
                    new IntegersPathComponent()))
                .ToList();

            Assert.Equal(1, result.Count);
            Assert.Equal(route3, result[0]);
        }
    }
}
