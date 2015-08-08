using System.Collections.Generic;
using Falcor.Server.Parser;
using Xunit;

namespace Falcor.Server.Tests
{
    public class InputParserTest
    {
        [Fact]
        public void Should_parse_string_array()
        {
            var result = InputPathParser.ParseInput("[ 'events' , 'name']");

            Assert.Equal(2, result.Count);
            Assert.Equal("events", result[0]);
            Assert.Equal("name", result[1]);
        }

        [Fact]
        public void Should_parse_int_array()
        {
            var result = InputPathParser.ParseInput("[ 10 , 22]");

            Assert.Equal(2, result.Count);
            Assert.Equal(10, result[0]);
            Assert.Equal(22, result[1]);
        }

        [Fact]
        public void Should_parse_range_array()
        {
            var result = InputPathParser.ParseInput("[ 10..15 , 3...30]");

            Assert.Equal(2, result.Count);
            AssertRange(10, 15, result[0]);
            AssertRange(3, 30, result[1]);
        }

        [Fact]
        public void Should_parse_mixed_array()
        {
            var result = InputPathParser.ParseInput("[ 'events' , 10, 0..50, 'name', 22]");

            Assert.Equal(5, result.Count);
            Assert.Equal("events", result[0]);
            Assert.Equal(10, result[1]);
            AssertRange(0, 50, result[2]);
            Assert.Equal("name", result[3]);
            Assert.Equal(22, result[4]);
        }

        [Fact]
        public void Should_parse_array_in_array()
        {
            var result = InputPathParser.ParseInput("[ 'events',0..5, ['name', 'order']]");

            Assert.Equal(3, result.Count);
            Assert.Equal("events", result[0]);
            AssertRange(0, 5, result[1]);
            var list = result[2] as IList<object>;            
            Assert.Equal(2, list.Count);
            Assert.Equal("name", list[0]);
            Assert.Equal("order", list[1]);
        }

        [Fact]
        public void Should_parse_multiple_arrays()
        {
            var result = InputPathParser.ParseInput("[['events',0,'name'],['events',0,'order']]");

            Assert.Equal(2, result.Count);
            var list1 = result[0] as IList<object>;
            Assert.NotNull(list1);

            var list2 = result[1] as IList<object>;
            Assert.NotNull(list2);
        }

        private static void AssertRange(int from, int to, object actual)
        {
            var range = actual as Range;
            Assert.NotNull(range);
            Assert.Equal(from, range.From);
            Assert.Equal(to, range.To);
        }
    }
}