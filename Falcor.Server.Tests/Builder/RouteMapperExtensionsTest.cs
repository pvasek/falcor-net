using System.Collections.Generic;
using Falcor.Server.Builder;
using Falcor.Server.Tests.Model;
using NUnit.Framework;

namespace Falcor.Server.Tests.Builder
{
    [TestFixture]
    public class RouteMapperExtensionsTest
    {
        [Test]
        public void Should_map_simple_property()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .Property(i => i.Name)
                .To(null);

            RouteAssertions.AssertSingleRoutePath(routes, new KeysPathComponent("Name"));
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
