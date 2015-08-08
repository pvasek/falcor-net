using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Falcor.Server.Tests.Builder
{
    public class RouteAssertions
    {
        public static void AssertSingleRoutePath(List<Route> routes, params IPathComponent[] expected)
        {
            Assert.Equal(1, routes.Count);
            var route = routes.First();
            AssertPath(route.Path.Components, expected);
        }

        public static void AssertPath(IPath actual, params IPathComponent[] expected)
        {
            AssertPath(actual.Components, expected);
        }

        public static void AssertPath(IList<IPathComponent> actual, params IPathComponent[] expected)
        { 
            Assert.Equal(expected.Length, actual.Count);
            var pathFragments = expected
                .Zip(actual, (expectedFragments, actualFragments) => new
                {
                    Actual = actualFragments,
                    Expected = expectedFragments
                })
                .ToList();

            foreach (var pathFragment in pathFragments)
            {
                Assert.Equal(pathFragment.Expected.GetType(), pathFragment.Actual.GetType());                
            }

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is KeysPathComponent))
            {
                var expectedFragments = (KeysPathComponent) pathFragment.Expected;
                var actualFragments = (KeysPathComponent) pathFragment.Actual;
                Assert.Equal(expectedFragments.Keys, actualFragments.Keys);
            }

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is RangePathComponent))
            {
                var expectedFragments = (RangePathComponent)pathFragment.Expected;
                var actualFragments = (RangePathComponent)pathFragment.Actual;
                Assert.Equal(expectedFragments.From, actualFragments.From);
                Assert.Equal(expectedFragments.To, actualFragments.To);
            }

            foreach (var pathFragment in pathFragments.Where(i => i.Expected is IntegersPathComponent))
            {
                var expectedFragments = (IntegersPathComponent)pathFragment.Expected;
                var actualFragments = (IntegersPathComponent)pathFragment.Actual;
                Assert.Equal(expectedFragments.Integers, actualFragments.Integers);
            }
        }
    }
}