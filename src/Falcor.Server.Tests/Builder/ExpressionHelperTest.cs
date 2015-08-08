using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Falcor.Server.Builder;
using Xunit;

namespace Falcor.Server.Tests.Builder
{
    public class ExpressionHelperTest
    {
        [Fact]
        public void Should_return_string_property()
        {
            Expression<Func<Class1, string>> func1 = (Class1 a) => a.Name;
            Assert.Equal("Name", ExpressionHelper.GetProperty(func1).Name);
        }

        [Fact]
        public void Should_return_integer_property()
        {
            Expression<Func<Class1, int>> func1 = (Class1 a) => a.Number;
            Assert.Equal("Number", ExpressionHelper.GetProperty(func1).Name);
        }


        [Fact]
        public void Should_return_list_property()
        {
            Expression<Func<Class1, List<Class1>>> func2 = (Class1 a) => a.Children;
            Assert.Equal("Children", ExpressionHelper.GetProperty(func2).Name);
        }

        [Fact]
        public void Should_return_string_property_as_object()
        {
            Expression<Func<Class1, object>> func1 = (Class1 a) => a.Name;
            Assert.Equal("Name", ExpressionHelper.GetProperty(func1).Name);
        }

        [Fact]
        public void Should_return_integer_property_as_object()
        {
            Expression<Func<Class1, object>> func1 = (Class1 a) => a.Number;
            Assert.Equal("Number", ExpressionHelper.GetProperty(func1).Name);
        }

        public class Class1
        {
            public string Name { get; set; }
            public int Number { get; set; }
            public List<Class1> Children { get; set; }        
        }
    }
}
