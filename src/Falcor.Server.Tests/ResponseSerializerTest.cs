using System.Collections.Generic;
using System.IO;
using Falcor.Server.Tests.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Falcor.Server.Tests
{
    public class ResponseSerializerTest
    {
        [Fact]
        public void Should_serialize_simple_int_property()
        {
            var response = new Response();
            response.Data.Add("EventCount", 2);

            var target = new ResponseSerializer();
            var result = target.Serialize(response);

            var json = ParseJson(result);            
            var jsong = json.Value<JObject>("jsong");
            Assert.NotNull(jsong);

            var eventCount = jsong.Value<JValue>("EventCount");
            Assert.NotNull(eventCount);
            Assert.Equal(2, eventCount.Value<int>());            
        }

        [Fact]
        public void Should_serialize_simple_string_property()
        {
            var response = new Response();
            response.Data.Add("EventName", "event1");

            var target = new ResponseSerializer();
            var result = target.Serialize(response);

            var json = ParseJson(result);
            var jsong = json.Value<JObject>("jsong");
            Assert.NotNull(jsong);

            var eventName = jsong.Value<JValue>("EventName");
            Assert.NotNull(eventName);
            Assert.Equal("event1", eventName.Value<string>());
        }

        [Fact]
        public void Should_serialize_simple_object_property()
        {
            var response = new Response();
            var eventData = new Dictionary<string, object>();
            response.Data.Add("Event", eventData);
            eventData["Name"] = "event1";
            eventData["NumberOfParticipants"] = 20;

            var target = new ResponseSerializer();
            var result = target.Serialize(response);

            var json = ParseJson(result);
            var jsong = json.Value<JObject>("jsong");
            Assert.NotNull(jsong);

            var eventResult = jsong.Value<JObject>("Event");
            Assert.NotNull(eventResult);
            Assert.Equal("event1", eventResult["Name"].Value<string>());
            Assert.Equal(20, eventResult["NumberOfParticipants"].Value<int>());
        }

        [Fact]
        public void Should_serialize_simple_list_property()
        {
            var response = new Response();
            var eventData = new List<string>
            {
                "event1",
                "event2",
                "event3"
            };
            response.Data.Add("Events", eventData);

            var target = new ResponseSerializer();
            var result = target.Serialize(response);

            var json = ParseJson(result);
            var jsong = json.Value<JObject>("jsong");
            Assert.NotNull(jsong);

            var eventsResult = jsong.Value<JArray>("Events");
            Assert.NotNull(eventsResult);
            Assert.Equal(3, eventsResult.Count);
            Assert.Equal("event1", eventsResult[0].Value<string>());
            Assert.Equal("event2", eventsResult[1].Value<string>());
            Assert.Equal("event3", eventsResult[2].Value<string>());
        }

        [Fact]
        public void Should_serialize_complex_object()
        {
            var response = new Response();
            var eventData = new List<IDictionary<string, object>>
            {

                new Dictionary<string, object>
                {
                    {"Name", "event1"},
                    {"NumberOfParticipants", 20}
                },
                new Dictionary<string, object>
                {
                    {"Name", "event2"},
                    {"NumberOfParticipants", 21}
                }
            };
            response.Data.Add("Events", eventData);

            var target = new ResponseSerializer();
            var result = target.Serialize(response);

            var json = ParseJson(result);
            var jsong = json.Value<JObject>("jsong");
            Assert.NotNull(jsong);

            var eventsResult = jsong.Value<JArray>("Events");
            Assert.NotNull(eventsResult);
            Assert.Equal(2, eventsResult.Count);
            var event1 = eventsResult[0].Value<JObject>();
            Assert.Equal("event1", event1["Name"].Value<string>());
            Assert.Equal(20, event1["NumberOfParticipants"].Value<int>());
            var event2 = eventsResult[1].Value<JObject>();
            Assert.Equal("event2", event2["Name"].Value<string>());
            Assert.Equal(21, event2["NumberOfParticipants"].Value<int>());
        }

        [Fact]
        public void Should_serialize_reference()
        {
            var response = new Response();
            response.Data.Add("EventRef", new Ref(new KeysPathComponent("EventsById", "9801")));

            var target = new ResponseSerializer();
            var result = target.Serialize(response);

            var json = ParseJson(result);
            var jsong = json.Value<JObject>("jsong");
            Assert.NotNull(jsong);

            var eventRef = jsong.Value<JObject>("EventRef");
            Assert.NotNull(eventRef);
            Assert.Equal("ref", eventRef["$type"].Value<string>());
            var refPath = eventRef["value"].Value<JArray>();
            Assert.Equal(2, refPath.Count);
            Assert.Equal("EventsById", refPath[0].Value<string>());
            Assert.Equal("9801", refPath[1].Value<string>());
        }

        private static JObject ParseJson(string json)
        {
            var serializer = new JsonSerializer();
            return serializer.Deserialize<JObject>(new JsonTextReader(new StringReader(json)));
        }        
    }
}
