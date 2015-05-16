using System;
using System.Collections.Generic;
using System.Linq;
using Falcor.Server.Routing;
using NUnit.Framework;

namespace Falcor.Server.Tests.Routing
{
    [TestFixture]
    public class RouteBuilderTest
    {
        [Test]
        public void Should_map_simple_property()
        {
            var routes = new List<Route>();
            routes.MapRoute<Model>().Property(i => i.Name).To(() => null);

            AssertSingleRoutePath(routes, new PropertyPathFragment("Name"));
        }
       
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_error_if_property_is_used_instead_of_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<Model>().Property(i => i.Users).To(() => null);
        }

        [Test]
        public void Should_map_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<Model>().List(i => i.Users).To(() => null);

            AssertSingleRoutePath(routes, new ListPathFragment("Users"));           
        }

        [Test]
        public void Should_map_simple_property_from_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<Model>()
                .List(i => i.Users)
                .Property(i => i.FirstName)
                .To(() => null);

            AssertSingleRoutePath(routes, 
                new ListPathFragment("Users"), 
                new PropertyPathFragment("FirstName"));
        }

        [Test]
        public void Should_map_list_of_properties_from_list()
        {
            var routes = new List<Route>();
            routes.MapRoute<Model>()
                .List(i => i.Users)
                .Properties(i => i.FirstName, i => i.LastName)
                .To(() => null);

            AssertSingleRoutePath(routes,
                new ListPathFragment("Users"),
                new PropertiesPathFragment("FirstName", "LastName"));
        }

        private void AssertSingleRoutePath(List<Route> routes, params PathFragment[] fragments)
        {
            Assert.AreEqual(1, routes.Count, "Expected only one route in routes collection");
            var route = routes.First();

            Assert.AreEqual(fragments.Length, route.Path.Count);
            var pathFragments = fragments
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

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is PropertyPathFragment))
            {
                Assert.AreEqual(
                    ((PropertyPathFragment)pathFragment.Expected).Key, 
                    ((PropertyPathFragment)pathFragment.Actual).Key);
            }

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is PropertiesPathFragment))
            {
                var expected = (PropertiesPathFragment) pathFragment.Expected;
                var actual = (PropertiesPathFragment) pathFragment.Actual;
                Assert.AreEqual(expected.Keys, actual.Keys);
            }
        }
    }

    public class Model
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
