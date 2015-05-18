using System;
using System.Collections.Generic;
using System.Linq;
using Falcor.Server.Routing;
using Falcor.Server.Routing.Builder;
using Falcor.Server.Tests.Model;
using NUnit.Framework;

namespace Falcor.Server.Tests.Routing.Builder
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
                .To(() => null);

            AssertSingleRoutePath(routes, new PropertiesPathComponent("Name"));
        }
       
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_error_if_property_is_used_instead_of_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .Property(i => i.Users)
                .To(() => null);
        }

        [Test]
        public void Should_map_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .List(i => i.Users)
                .AsIndex()
                .To(() => null);

            AssertSingleRoutePath(routes, 
                new PropertiesPathComponent("Users"),
                new IntegersPathComponent()); 
        }

        [Test]
        public void Should_map_list_with_range()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .List(i => i.Users)
                .AsRange(0, 10)
                .To(() => null);

            AssertSingleRoutePath(routes, 
                new PropertiesPathComponent("Users"), 
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
                .To(() => null);

            AssertSingleRoutePath(routes,
                new PropertiesPathComponent("Users"),
                new IntegersPathComponent(),
                new PropertiesPathComponent("FirstName"));
        }

        [Test]
        public void Should_map_list_of_properties_from_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestModel>()
                .List(i => i.Users)
                .AsIndex()
                .Properties(i => i.FirstName, i => i.LastName)
                .To(() => null);

            AssertSingleRoutePath(routes,
                new PropertiesPathComponent("Users"),
                new IntegersPathComponent(),
                new PropertiesPathComponent("FirstName", "LastName"));
        }

        private void AssertSingleRoutePath(List<Route> routes, params IPathComponent[] components)
        {
            Assert.AreEqual(1, routes.Count, "Expected only one route in routes collection");
            var route = routes.First();

            Assert.AreEqual(components.Length, route.Path.Count);
            var pathFragments = components
                .Zip(route.Path, (expected, actual) => new
                    {
                        Actual = actual,
                        Expected = expected
                    })
                .ToList();

            foreach (var pathFragment in pathFragments)
            {
                Assert.AreEqual(pathFragment.Expected.GetType(), pathFragment.Actual.GetType());                
            }

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is PropertiesPathComponent))
            {
                var expected = (PropertiesPathComponent) pathFragment.Expected;
                var actual = (PropertiesPathComponent) pathFragment.Actual;
                Assert.AreEqual(expected.Properties, actual.Properties);
            }

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is RangePathComponent))
            {
                var expected = (RangePathComponent)pathFragment.Expected;
                var actual = (RangePathComponent)pathFragment.Actual;
                Assert.AreEqual(expected.From, actual.From);
                Assert.AreEqual(expected.To, actual.To);
            }
        }
    }
}
