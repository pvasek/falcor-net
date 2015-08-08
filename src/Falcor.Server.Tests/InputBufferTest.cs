using System;
using Falcor.Server.Parser;
using Xunit;

namespace Falcor.Server.Tests
{
    public class InputBufferTest
    {
        [Fact]
        public void Should_skip_white_space()
        {
            var target = new InputBuffer("  []");
            target.SkipWhiteSpaces();
            Assert.Equal('[', target.Current);
            Assert.Equal(true, target.Next());
        }

        [Fact]
        public void Should_skip_white_space_to_the_end()
        {
            var target = new InputBuffer("  ");
            target.SkipWhiteSpaces();
            Assert.Equal(Char.MaxValue, target.Current);
            Assert.Equal(false, target.Next());
        }
    }
}
