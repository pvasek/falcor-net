using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Falcor.Server.Builder;
using NUnit.Framework;

namespace Falcor.Server.Tests.Builder
{
    [TestFixture]
    public class ExpressionHelperTest
    {
        [Test]
        public void Should_return_property()
        {
            Expression<Func<Class1, string>> func1 = (Class1 a) => a.Name;
            Assert.AreEqual("Name", ExpressionHelper.GetProperty(func1).Name);

            Expression<Func<Class1, List<Class1>>> func2 = (Class1 a) => a.Children;
            Assert.AreEqual("Children", ExpressionHelper.GetProperty(func2).Name);
        }

        public class Class1
        {
            public string Name { get; set; }
            public List<Class1> Children { get; set; }        
        }
    }
}
