using System.Collections.Generic;
using Falcor.Server.Builder;
using Falcor.Server.Tests.Model;
using Xunit;

namespace Falcor.Server.Tests.Builder
{
    public class RouteMapperExtensionsTest
    {
        [Fact]
        public void Should_map_simple_property()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .Property(i => i.Name)
                .To(null);

            RouteAssertions.AssertSingleRoutePath(routes, new KeysPathComponent("Name"));
        }

        [Fact]
        public void Should_map_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .List(i => i.Users)
                .AsIndex()
                .To(null);

            RouteAssertions.AssertSingleRoutePath(routes,
                new KeysPathComponent("Users"),
                new IntegersPathComponent());
        }

        [Fact]
        public void Should_map_list_with_range()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .List(i => i.Users)
                .AsRange(0, 10)
                .To(null);

            RouteAssertions.AssertSingleRoutePath(routes,
                new KeysPathComponent("Users"),
                new RangePathComponent(0, 10));
        }

        [Fact]
        public void Should_map_simple_property_from_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .List(i => i.Users)
                .AsIndex()
                .Property(i => i.FirstName)
                .To(null);

            RouteAssertions.AssertSingleRoutePath(routes,
                new KeysPathComponent("Users"),
                new IntegersPathComponent(),
                new KeysPathComponent("FirstName"));
        }

        [Fact]
        public void Should_map_list_of_properties_from_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .List(i => i.Users)
                .AsIndex()
                .Properties(i => i.FirstName, i => i.LastName)
                .To(null);

            RouteAssertions.AssertSingleRoutePath(routes,
                new KeysPathComponent("Users"),
                new IntegersPathComponent(),
                new KeysPathComponent("FirstName", "LastName"));
        }
    }
}
