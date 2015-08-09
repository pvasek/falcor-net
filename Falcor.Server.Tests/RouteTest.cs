using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Falcor.Server.Tests
{
    [TestFixture]
    public class RouteTest
    {
        [Test]
        public void Should_call_handler_with_translated_path()
        {
            var target = new Route(
                Keys.Any("key1"), 
                Keys.Any("key2"),
                Keys.Any("key3"));

            var handlerCalled = false;
            target.Handler = path =>
            {
                Assert.AreEqual(3, path.Components.Count);
                Assert.AreEqual("key1", path.Components[0].Name);
                Assert.AreEqual("key2", path.Components[1].Name);
                Assert.AreEqual("key3", path.Components[2].Name);
                Assert.AreEqual("val1", path.Components[0].Value);
                Assert.AreEqual("val2", path.Components[1].Value);
                Assert.AreEqual("val3", path.Components[2].Value);
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