using System.Collections.Generic;
using Falcor.Server.Parser;
using NUnit.Framework;

namespace Falcor.Server.Tests
{
    public class InputParserTest
    {
        [Test]
        public void Should_parse_string_array()
        {
            var result = InputPathParser.ParseInput("[ 'events' , 'name']");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("events", result[0]);
            Assert.AreEqual("name", result[1]);
        }

        [Test]
        public void Should_parse_int_array()
        {
            var result = InputPathParser.ParseInput("[ 10 , 22]");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(10, result[0]);
            Assert.AreEqual(22, result[1]);
        }

        [Test]
        public void Should_parse_range_array()
        {
            var result = InputPathParser.ParseInput("[ 10..15 , 3...30]");

            Assert.AreEqual(2, result.Count);
            AssertRange(10, 15, result[0]);
            AssertRange(3, 30, result[1]);
        }

        [Test]
        public void Should_parse_mixed_array()
        {
            var result = InputPathParser.ParseInput("[ 'events' , 10, 0..50, 'name', 22]");

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("events", result[0]);
            Assert.AreEqual(10, result[1]);
            AssertRange(0, 50, result[2]);
            Assert.AreEqual("name", result[3]);
            Assert.AreEqual(22, result[4]);
        }

        [Test]
        public void Should_parse_array_in_array()
        {
            var result = InputPathParser.ParseInput("[ 'events',0..5, ['name', 'order']]");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("events", result[0]);
            AssertRange(0, 5, result[1]);
            var list = result[2] as IList<object>;            
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("name", list[0]);
            Assert.AreEqual("order", list[1]);
        }

        [Test]
        public void Should_parse_multiple_arrays()
        {
            var result = InputPathParser.ParseInput("[['events',0,'name'],['events',0,'order']]");

            Assert.AreEqual(2, result.Count);
            var list1 = result[0] as IList<object>;
            Assert.IsNotNull(list1);

            var list2 = result[1] as IList<object>;
            Assert.IsNotNull(list2);
        }

        private static void AssertRange(int from, int to, object actual)
        {
            var range = actual as Range;
            Assert.IsNotNull(range);
            Assert.AreEqual(from, range.From);
            Assert.AreEqual(to, range.To);
        }
    }
}