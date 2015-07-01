using System.Collections.Generic;
using System.IO;
using Falcor.Server.Tests.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Falcor.Server.Tests
{
    [TestFixture]
    public class ResponseSerializerTest
    {
        [Test]
        public void Should_serialize_simple_int_property()
        {
            var response = new Response();
            response.Data.Add("EventCount", 2);

            var target = new ResponseSerializer();
            var result = target.Serialize(response);

            var json = ParseJson(result);            
            var jsong = json.Value<JObject>("jsong");
            Assert.IsNotNull(jsong);

            var eventCount = jsong.Value<JValue>("EventCount");
            Assert.IsNotNull(eventCount);
            Assert.AreEqual(2, eventCount.Value<int>());            
        }

        [Test]
        public void Should_serialize_simple_string_property()
        {
            var response = new Response();
            response.Data.Add("EventName", "event1");

            var target = new ResponseSerializer();
            var result = target.Serialize(response);

            var json = ParseJson(result);
            var jsong = json.Value<JObject>("jsong");
            Assert.IsNotNull(jsong);

            var eventName = jsong.Value<JValue>("EventName");
            Assert.IsNotNull(eventName);
            Assert.AreEqual("event1", eventName.Value<string>());
        }

        [Test]
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
            Assert.IsNotNull(jsong);

            var eventResult = jsong.Value<JObject>("Event");
            Assert.IsNotNull(eventResult);
            Assert.AreEqual("event1", eventResult["Name"].Value<string>());
            Assert.AreEqual(20, eventResult["NumberOfParticipants"].Value<int>());
        }

        [Test]
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
            Assert.IsNotNull(jsong);

            var eventsResult = jsong.Value<JArray>("Events");
            Assert.IsNotNull(eventsResult);
            Assert.AreEqual(3, eventsResult.Count);
            Assert.AreEqual("event1", eventsResult[0].Value<string>());
            Assert.AreEqual("event2", eventsResult[1].Value<string>());
            Assert.AreEqual("event3", eventsResult[2].Value<string>());
        }

        [Test]
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
            Assert.IsNotNull(jsong);

            var eventsResult = jsong.Value<JArray>("Events");
            Assert.IsNotNull(eventsResult);
            Assert.AreEqual(2, eventsResult.Count);
            var event1 = eventsResult[0].Value<JObject>();
            Assert.AreEqual("event1", event1["Name"].Value<string>());
            Assert.AreEqual(20, event1["NumberOfParticipants"].Value<int>());
            var event2 = eventsResult[1].Value<JObject>();
            Assert.AreEqual("event2", event2["Name"].Value<string>());
            Assert.AreEqual(21, event2["NumberOfParticipants"].Value<int>());
        }

        [Test]
        public void Should_serialize_reference()
        {
            var response = new Response();
            response.Data.Add("EventRef", new Ref(new KeysPathComponent("EventsById", "9801")));

            var target = new ResponseSerializer();
            var result = target.Serialize(response);

            var json = ParseJson(result);
            var jsong = json.Value<JObject>("jsong");
            Assert.IsNotNull(jsong);

            var eventRef = jsong.Value<JObject>("EventRef");
            Assert.IsNotNull(eventRef);
            Assert.AreEqual("ref", eventRef["$type"].Value<string>());
            var refPath = eventRef["value"].Value<JArray>();
            Assert.AreEqual(2, refPath.Count);
            Assert.AreEqual("EventsById", refPath[0].Value<string>());
            Assert.AreEqual("9801", refPath[1].Value<string>());
        }

        private static JObject ParseJson(string json)
        {
            var serializer = new JsonSerializer();
            return serializer.Deserialize<JObject>(new JsonTextReader(new StringReader(json)));
        }        
    }
}
