using NUnit.Framework;

namespace Falcor.Server.Tests
{
    [TestFixture]
    public class PathExtensionsTest
    {
        [Test]
        public void Should_match_path_component_by_name()
        {
            var key1 = Keys.Any("key1");
            var key2 = Keys.Any("key2");
            var key3 = Keys.Any("key3");
            var path = new Path(key1, key2, key3);
            Assert.AreEqual(key1, path.As<Keys>("key1"));
            Assert.AreEqual(key2, path.As<Keys>("key2"));
            Assert.AreEqual(key3, path.As<Keys>("key3"));
        }

        [Test]
        public void Should_throw_exception_if_the_name_doesnt_exists()
        {
            var key1 = Keys.Any("key1");
            var path = new Path(key1);
            try
            {
                path.As<Keys>("key4");
                Assert.Fail();
            }
            catch
            {                
            }            
        }

        [Test]
        public void Should_throw_exception_if_the_type_is_wrong()
        {
            var key1 = Keys.Any("key1");
            var path = new Path(key1);
            try
            {
                path.As<Integers>("key1");
                Assert.Fail();
            }
            catch
            {
            }
        }
    }
}