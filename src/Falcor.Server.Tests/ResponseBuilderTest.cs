using System.Collections.Generic;
using Xunit;

namespace Falcor.Server.Tests
{
    public class ResponseBuilderTest
    {
        [Fact]
        public void Should_create_response_for_simple_property()
        {
            var target = new ResponseBuilder();
            var result = target.CreateResponse(new[]
            {
                PathValue.Create("name1", 
                    new KeysPathComponent("settings"), 
                    new KeysPathComponent("Name"))
            });

            var settings = VerifyObjectProperty(result.Data, "settings");
            Assert.Equal("name1", settings["Name"]);

            Assert.Equal(1, result.Paths.Count);
            Assert.Equal(new [] { "settings", "Name"}, result.Paths[0]);
        }

        [Fact]
        public void Should_create_response_from_simple_property_from_list()
        {
            var target = new ResponseBuilder();
            var result = target.CreateResponse(new[]
            {
                PathValue.Create("name1", 
                    new KeysPathComponent("settings"), 
                    new IntegersPathComponent(0),
                    new KeysPathComponent("Name"))
            });

            var settings = VerifyObjectProperty(result.Data, "settings");
            var firstSettings = VerifyObjectProperty(settings, "0");
            Assert.Equal("name1", firstSettings["Name"]);

            Assert.Equal(1, result.Paths.Count);
            Assert.Equal(new object [] { "settings", 0, "Name" }, result.Paths[0]);
        }

        [Fact]
        public void Should_create_response_from_simple_properties_from_list()
        {
            var target = new ResponseBuilder();
            var result = target.CreateResponse(new[]
            {
                PathValue.Create("name1", 
                    new KeysPathComponent("settingById"), 
                    new KeysPathComponent("1"),
                    new KeysPathComponent("Name")),
                PathValue.Create(10,
                    new KeysPathComponent("settingById"), 
                    new KeysPathComponent("1"),
                    new KeysPathComponent("Number"))
            });

            var settings = VerifyObjectProperty(result.Data, "settingById");
            var firstSettings = VerifyObjectProperty(settings, "1");
            Assert.Equal("name1", firstSettings["Name"]);
            Assert.Equal(10, firstSettings["Number"]);

            Assert.Equal(2, result.Paths.Count);
            Assert.Equal(new object[] { "settingById", "1", "Name" }, result.Paths[0]);
            Assert.Equal(new object[] { "settingById", "1", "Number" }, result.Paths[1]);
        }

        [Fact]
        public void Should_create_response_for_reference()
        {
            var target = new ResponseBuilder();
            var result = target.CreateResponse(new[]
            {
                PathValue.Create(new Ref(new KeysPathComponent("settings"), new IntegersPathComponent(0)), 
                    new KeysPathComponent("settings"), 
                    new IntegersPathComponent(0))
            });

            var settings = VerifyObjectProperty(result.Data, "settings");
            var firstSettings = settings["0"];
            Assert.True(firstSettings is Ref);

            Assert.Equal(1, result.Paths.Count);
            Assert.Equal(new object[] { "settings", 0 }, result.Paths[0]);
        }

        private static IDictionary<string,object> VerifyObjectProperty(IDictionary<string,object> input, string key)
        {
            Assert.True(input.ContainsKey(key), "The dictionary doesn't include key " + key);
            var settings = input[key] as IDictionary<string, object>;
            Assert.NotNull(settings); //, "The dictionary value should be type of IDictionary<string,object>"
            return settings;
        }
    }
}
