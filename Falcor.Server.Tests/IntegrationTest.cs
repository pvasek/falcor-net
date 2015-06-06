﻿using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Falcor.Server.Builder;
using Falcor.Server.Tests.Model;
using NUnit.Framework;

namespace Falcor.Server.Tests
{
    [TestFixture]
    public class IntegrationTest
    {
        [Test]
        public void Integration_test()
        {
            var routes = new List<Route>();
            routes.MapRoute<TestEventModel>()
                .List(i => i.Events)
                .AsIndex()
                .To(p =>
                {
                    var index = ((IntegersPathComponent) p.Components[1]).Integers.First() + 1;
                    var reference = new Ref(
                        new PropertiesPathComponent("EventById"), 
                        new KeysPathComponent("980" + index));

                    return Observable.Return(PathValue.Create(reference, new PropertiesPathComponent("Events"), p.Components[1]));
                });   

            routes.MapRoute<TestEventModel>()
                .Dictionary(i => i.EventById)
                .AsKey()
                .Property(i => i.Name)
                .To(p =>
                {
                    var pathValue = new PathValue
                        {
                            Value = "name1",
                            Path = p
                        };

                    return Observable.Return(pathValue);
                });

            var routeResolver = new RouteResolver(routes);
            var pathCollapser = new PathCollapser();
            var responseBuilder = new ResponseBuilder();

            var target = new Router(routeResolver, pathCollapser, responseBuilder);

            var path1 = new Path(
                new PropertiesPathComponent("Events"),
                new IntegersPathComponent(0),
                new PropertiesPathComponent("Name"));

            var path2 = new Path(
                new PropertiesPathComponent("Events"),
                new IntegersPathComponent(1),
                new PropertiesPathComponent("Name"));

            var result = target.Execute(path1, path2);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);

            var events = result.Data["Events"] as IDictionary<string, object>;
            Assert.IsNotNull(events);
            var eventById = result.Data["EventById"] as IDictionary<string, object>;
            Assert.IsNotNull(eventById);

            var event0 = events["0"] as Ref;
            var event1 = events["1"] as Ref;
            Assert.IsNotNull(event0);
            Assert.IsNotNull(event1);
            Assert.AreEqual("EventById", event0.Path.Components[0].Key);
            Assert.AreEqual("9801", event0.Path.Components[1].Key);
            Assert.AreEqual("EventById", event1.Path.Components[0].Key);
            Assert.AreEqual("9802", event1.Path.Components[1].Key);

            var eventById1 = (IDictionary<string,object>)eventById["9801"];
            var eventById2 = (IDictionary<string, object>)eventById["9802"];
            Assert.IsNotNull(eventById1);
            Assert.IsNotNull(eventById2);
            Assert.AreEqual("name1", eventById1["Name"]);
            Assert.AreEqual("name1", eventById2["Name"]);

            // TODO: thinkg about path meaning and how to reconstruct it
//            Assert.AreEqual(2, result.Paths.Count);
//            Assert.AreEqual(new object[] { "Events", 0, "Name"}, result.Paths[0]);
//            Assert.AreEqual(new object[] { "Events", 1, "Name" }, result.Paths[1]);

            /*
response = { 
    jsong : {
        "events": {
            "0": $ref(["eventsById","99801"]),
            "1": $ref(["eventsById","99802"])
        },
        "eventById": {
            "99801": { name: "name1" },
            "99802": { name: "name2" }
        } 
    },
    // we don't need to worry about this for get/set, it's important only for call
    // paths : [
    //     ["events", 0, "name"],
    //     ["events", 1, "name"],
    // ]   
       }*/

        }
    }
}