using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Falcor.Server.Tests
{
    [TestFixture]
    public class RouterTest
    {
        [Test]
        public async Task Should_route_simple_route()
        {
            // TODO: fix this test
            var routeResolver = new Mock<IRouteResolver>();
            var pathCollapser = new Mock<IPathCollapser>();
            var responseBuilder = new Mock<IResponseBuilder>();
            pathCollapser.Setup(i => i.Collapse(It.IsAny<IEnumerable<IPath>>()))
                .Returns((IEnumerable<IPath> paths) => paths);

            var path1 = new Path(new Keys("events"), new Integers(0), new Keys("name"));
            
            // the first route should match only the first part
            var route1 = CreateRoute(
                new Ref(new Keys("eventById"), new Keys("99801")),
                new Keys("events"), new Integers(0));
            
            // the second route should match the rest of the route and replace beginning with reference
            var route2 = CreateRoute(
                "name1",
                new Keys("eventById"), new Keys("99801"), new Keys("name"));

            var findRouteCount = 0;
            routeResolver
                .Setup(i => i.FindRoutes(It.IsAny<IPath>()))
                .Returns((IPath p) =>
                    {
                        if (p == path1)
                        {
                            Assert.AreEqual(0, findRouteCount);
                            findRouteCount++;
                            return new List<Route> {route1};
                        }
                        findRouteCount++;
                        return new List<Route> {route2};
                    });
            
            var response = new Response();
            responseBuilder
                .Setup(i => i.CreateResponse(It.IsAny<IList<PathValue>>()))
                .Returns((IList<PathValue> input) =>
                {
                    return response;
                });

            //["events", 0, "name"]
            //["eventsById", "99801", "name"]
            var target = new Router(routeResolver.Object, pathCollapser.Object, responseBuilder.Object);

            var result = await target.Execute(path1);

            Assert.AreEqual(response, result);
            Assert.AreEqual(2, findRouteCount);
        }

        private Route CreateRoute(object value, params IPathItem[] items)
        {
            var pathValue = new PathValue(value, items);

            return new Route
            {
                RouteHandler = path => Task.FromResult(pathValue.AsEnumerable())                
            };
        }
    }
}
