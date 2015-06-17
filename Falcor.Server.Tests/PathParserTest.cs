using Falcor.Server.Tests.Builder;
using NUnit.Framework;

namespace Falcor.Server.Tests
{
    [TestFixture]
    public class PathParserTest
    {
        [Test]
        public void Should_parse_single_path()
        {
            var target = new PathParser();
            var result = target.ParsePaths("['events',0,'name']");

            Assert.AreEqual(1, result.Count);
            RouteAssertions.AssertPath(result[0], 
                new KeysPathComponent("events"),
                new IntegersPathComponent(0),
                new KeysPathComponent("name"));
        }

        [Test]
        public void Should_parse_multiple_paths()
        {
            var target = new PathParser();
            var result = target.ParsePaths("[['events',0,'name'],['users',1,'userName']]");

            Assert.AreEqual(2, result.Count);

            RouteAssertions.AssertPath(result[0],
                new KeysPathComponent("events"),
                new IntegersPathComponent(0),
                new KeysPathComponent("name"));

            RouteAssertions.AssertPath(result[1],
                new KeysPathComponent("users"),
                new IntegersPathComponent(1),
                new KeysPathComponent("userName"));
        }

        [Test]
        public void Should_parse_ranges_as_integers()
        {
            var target = new PathParser();
            var result = target.ParsePaths("['events',0..5,'name']");

            Assert.AreEqual(1, result.Count);

            RouteAssertions.AssertPath(result[0],
                new KeysPathComponent("events"),
                new IntegersPathComponent(0, 1, 2, 3, 4, 5),
                new KeysPathComponent("name"));
        }
    }
}
