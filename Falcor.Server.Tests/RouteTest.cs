using NUnit.Framework;

namespace Falcor.Server.Tests
{
    [TestFixture]
    public class RouteTest
    {
        [Test]
        public void Should_call_handler_with_translated_path()
        {
            var key1 = Keys.Any();
            var key2 = Keys.Any();
            var key3 = Keys.Any();

            var target = new Route(
                new RoutePathItem("key1", key1),
                new RoutePathItem("key2", key2),
                new RoutePathItem("key3", key3));

            var handlerCalled = false;
            target.RouteHandler = ctx =>
            {
                Assert.AreEqual(3, ctx.Path.Items.Count);
                Assert.AreEqual("val1", ctx.Item<Keys>("key1").Value);
                Assert.AreEqual("val2", ctx.Item<Keys>("key2").Value);
                Assert.AreEqual("val3", ctx.Item<Keys>("key3").Value);
                handlerCalled = true;
                return null;
            };

            var inputPath = new Path(
                new Keys("val1"), 
                new Keys("val2"), 
                new Keys("val3"));

            target.Execute(inputPath);
            Assert.AreEqual(true, handlerCalled);
        }
    }
}