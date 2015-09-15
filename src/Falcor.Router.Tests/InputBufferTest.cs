using System;
using Falcor.Router.Parser;
using NUnit.Framework;

namespace Falcor.Router.Tests
{
    [TestFixture]
    public class InputBufferTest
    {
        [Test]
        public void Should_skip_white_space()
        {
            var target = new InputBuffer("  []");
            target.SkipWhiteSpaces();
            Assert.AreEqual('[', target.Current);
            Assert.AreEqual(true, target.Next());
        }

        [Test]
        public void Should_skip_white_space_to_the_end()
        {
            var target = new InputBuffer("  ");
            target.SkipWhiteSpaces();
            Assert.AreEqual(Char.MaxValue, target.Current);
            Assert.AreEqual(false, target.Next());
        }
    }
}
