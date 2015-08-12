using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcor.Server.Extensions;
using Falcor.Server.Tests.Model;
using NUnit.Framework;

namespace Falcor.Server.Tests
{
    [TestFixture]
    public class IntegrationTest
    {
        [Test]
        public async Task Single_properties_per_route()
        {
            var routes = new List<Route>();
            routes.MapRoute()
                .PathItem(Keys.For("Events"))
                .PathItem("index", Integers.Any())
                .To(ctx =>
                {
                    var indexItem = ctx.Item<Integers>("index");
                    var index = indexItem.Values.First();
                    var reference = new Ref(
                        new Keys("EventById"), 
                        new Keys("980" + index));

                    return Task.FromResult(PathValue
                                .Create(reference, new Keys("Events"), Integers.For(index))
                                .AsEnumerable());
                });   

            routes.MapRoute()
                .PathItem(Keys.For("EventById"))
                .PathItem("key", Keys.Any())
                .PathItem(Keys.For("Name"))
                .To(ctx =>
                {
                    var pathValue = new PathValue
                        {
                            Value = "name1",
                            Path = ctx.Path
                        };

                    return Task.FromResult(pathValue.AsEnumerable());
                });

            var routeResolver = new RouteResolver(routes);
            var pathCollapser = new PathCollapser();
            var responseBuilder = new ResponseBuilder();

            var target = new Router(routeResolver, pathCollapser, responseBuilder);

            var path1 = new Path(
                new Keys("Events"),
                new Integers(0),
                new Keys("Name"));

            var path2 = new Path(
                new Keys("Events"),
                new Integers(1),
                new Keys("Name"));

            var result = await target.Execute(path1, path2);
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
            Assert.AreEqual("EventById", event0.Path.Items[0].Value);
            Assert.AreEqual("9800", event0.Path.Items[1].Value);
            Assert.AreEqual("EventById", event1.Path.Items[0].Value);
            Assert.AreEqual("9801", event1.Path.Items[1].Value);

            var eventById1 = (IDictionary<string,object>)eventById["9800"];
            var eventById2 = (IDictionary<string, object>)eventById["9801"];
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


        [Test]
        public async Task Multiple_properties_per_route()
        {
            var routes = new List<Route>();
            routes.MapRoute()
                .PathItem(Keys.For("Events"))
                .PathItem("index", Integers.Any())
                .To(ctx =>
                {
                    var indexItem = ctx.Item<Integers>("index");
                    var index = indexItem.Values.First() + 1;
                    var reference = new Ref(
                        new Keys("EventById"),
                        new Keys("980" + index));

                    return Task.FromResult(PathValue.Create(reference, new Keys("Events"), ctx.Path.Items[1]).AsEnumerable());
                });

            routes.MapRoute()
                .PathItem(Keys.For("EventById"))
                .PathItem("key", Keys.Any())
                .PathItem("properties", Keys.For("Name", "Number"))
                .To(ctx =>
                {
                    var pathValues = new List<PathValue>();
                    var properties = ctx.Item<Keys>("properties").Values;
                    if (properties.Contains("Name"))
                    {
                        pathValues.Add(new PathValue
                        {
                            Value = "name1",
                            Path = ctx.Path
                        });
                    }
                    if (properties.Contains("Number"))
                    {
                        pathValues.Add(new PathValue
                        {
                            Value = 1,
                            Path = ctx.Path
                        });
                    }
                    
                    return Task.FromResult(pathValues.AsEnumerable());
                });

            var routeResolver = new RouteResolver(routes);
            var pathCollapser = new PathCollapser();
            var responseBuilder = new ResponseBuilder();

            var target = new Router(routeResolver, pathCollapser, responseBuilder);

            var path1 = new Path(
                new Keys("Events"),
                new Integers(0),
                new Keys("Name"));

            var path2 = new Path(
                new Keys("Events"),
                new Integers(0),
                new Keys("Number"));

            var result = await target.Execute(path1, path2);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);

            var events = result.Data["Events"] as IDictionary<string, object>;
            Assert.IsNotNull(events);
            var eventById = result.Data["EventById"] as IDictionary<string, object>;
            Assert.IsNotNull(eventById);

            var event0 = events["0"] as Ref;
            Assert.IsNotNull(event0);
            Assert.AreEqual("EventById", event0.Path.Items[0].Value);
            Assert.AreEqual("9801", event0.Path.Items[1].Value);

            var eventById1 = (IDictionary<string, object>)eventById["9801"];
            Assert.IsNotNull(eventById1);
            Assert.AreEqual("name1", eventById1["Name"]);
            Assert.AreEqual(1, eventById1["Number"]);            
        }
    }
}
