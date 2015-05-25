﻿using System.Collections.Generic;
using Falcor.Server.Routing;
using NUnit.Framework;

namespace Falcor.Server.Tests.Routing
{
    [TestFixture]
    public class ResponseBuilderTest
    {
        [Test]
        public void Should_create_response_for_simple_property()
        {
            var target = new ResponseBuilder();
            var result = target.CreateResponse(new[]
            {
                PathValue.Create("name1", 
                    new PropertiesPathComponent("settings"), 
                    new PropertiesPathComponent("Name"))
            });

            var settings = VerifyObjectProperty(result.Data, "settings");
            Assert.AreEqual("name1", settings["Name"]);

            Assert.AreEqual(1, result.Paths.Count);
            Assert.AreEqual(new [] { "settings", "Name"}, result.Paths[0]);
        }

        [Test]
        public void reShould_create_response_from_simple_property_from_list()
        {
            var target = new ResponseBuilder();
            var result = target.CreateResponse(new[]
            {
                PathValue.Create("name1", 
                    new PropertiesPathComponent("settings"), 
                    new IntegersPathComponent(0),
                    new PropertiesPathComponent("Name"))
            });

            var settings = VerifyObjectProperty(result.Data, "settings");
            var firstSettings = VerifyObjectProperty(settings, "0");
            Assert.AreEqual("name1", firstSettings["Name"]);

            Assert.AreEqual(1, result.Paths.Count);
            Assert.AreEqual(new object [] { "settings", 0, "Name" }, result.Paths[0]);
        }

        [Test]
        public void Should_create_response_from_simple_properties_from_list()
        {
            var target = new ResponseBuilder();
            var result = target.CreateResponse(new[]
            {
                PathValue.Create("name1", 
                    new PropertiesPathComponent("settings"), 
                    new IntegersPathComponent(0),
                    new PropertiesPathComponent("Name")),
                PathValue.Create(10,
                    new PropertiesPathComponent("settings"), 
                    new IntegersPathComponent(0),
                    new PropertiesPathComponent("Number"))
            });

            var settings = VerifyObjectProperty(result.Data, "settings");
            var firstSettings = VerifyObjectProperty(settings, "0");
            Assert.AreEqual("name1", firstSettings["Name"]);
            Assert.AreEqual(10, firstSettings["Number"]);

            Assert.AreEqual(2, result.Paths.Count);
            Assert.AreEqual(new object[] { "settings", 0, "Name" }, result.Paths[0]);
            Assert.AreEqual(new object[] { "settings", 0, "Number" }, result.Paths[1]);
        }

        private static IDictionary<string,object> VerifyObjectProperty(IDictionary<string,object> input, string key)
        {
            Assert.IsTrue(input.ContainsKey(key), "The dictionary doesn't include key " + key);
            var settings = input[key] as IDictionary<string, object>;
            Assert.IsNotNull(settings, "The dictionary value should be type of IDictionary<string,object>");
            return settings;
        }
    }
}
