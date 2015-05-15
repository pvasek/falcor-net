using System;
using System.Linq.Expressions;
using Falcor.Server.Routing;
using NUnit.Framework;

namespace Falcor.Server.Tests.Routing
{
    [TestFixture]
    public class ExpressionHelperTest
    {
        [Test]
        public void Should_return_property_name()
        {
            Expression<Func<Class1, string>> func = (Class1 a) => a.Name;
            Assert.AreEqual("Name", ExpressionHelper.GetPropertyName(func));
        }

        public class Class1
        {
            public string Name { get; set; }
        }
    }
}
