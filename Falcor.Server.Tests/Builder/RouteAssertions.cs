using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Falcor.Server.Tests.Builder
{
    public class RouteAssertions
    {
        public static void AssertSingleRoutePath(List<Route> routes, params IPathItem[] expected)
        {
            Assert.AreEqual(1, routes.Count, "Expected only one route in routes collection");
            var route = routes.First();
            AssertPath(route.Items.Select(i => i.Item).ToList(), expected);
        }

        public static void AssertPath(IPath actual, params IPathItem[] expected)
        {
            AssertPath(actual.Items, expected);
        }

        public static void AssertPath(IList<IPathItem> actual, params IPathItem[] expected)
        { 
            Assert.AreEqual(expected.Length, actual.Count);
            var pathFragments = expected
                .Zip(actual, (expectedFragments, actualFragments) => new
                {
                    Actual = actualFragments,
                    Expected = expectedFragments
                })
                .ToList();

            foreach (var pathFragment in pathFragments)
            {
                Assert.AreEqual(pathFragment.Expected.GetType(), pathFragment.Actual.GetType());                
            }

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is Keys))
            {
                var expectedFragments = (Keys) pathFragment.Expected;
                var actualFragments = (Keys) pathFragment.Actual;
                Assert.AreEqual(expectedFragments.Values, actualFragments.Values);
            }

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is Range))
            {
                var expectedFragments = (Range)pathFragment.Expected;
                var actualFragments = (Range)pathFragment.Actual;
                Assert.AreEqual(expectedFragments.From, actualFragments.From);
                Assert.AreEqual(expectedFragments.To, actualFragments.To);
            }

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is Integers))
            {
                var expectedFragments = (Integers)pathFragment.Expected;
                var actualFragments = (Integers)pathFragment.Actual;
                Assert.AreEqual(expectedFragments.Values, actualFragments.Values);
            }
        }
    }
}