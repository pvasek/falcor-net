using System.Collections.Generic;
using System.Reactive.Linq;
using Falcor.Server.Routing;
using Moq;
using NUnit.Framework;

namespace Falcor.Server.Tests.Routing
{
    [TestFixture]
    public class RouterTest
    {
        [Test]
        public void Should_route_simple_route()
        {
            // TODO: fix this test
            var routeResolver = new Mock<IRouteResolver>();
            var pathCollapser = new Mock<IPathCollapser>();
            var responseBuilder = new Mock<IResponseBuilder>();
            var paths = new List<IPath>();
            var collapsedPaths1 = new List<IPath>
            {
                new Path(new PropertiesPathComponent("settings"), new IntegersPathComponent(0)/* TODO: match against the full reminders-> new PropertiesPathComponent("Name")*/)                
            };
            
            var route1a = new Route();
            route1a.Handler = path => 
                new[]
                    {
                        PathValue.Create(
                            new Ref(new PropertiesPathComponent("settingById"), new KeysPathComponent("1")),
                            new PropertiesPathComponent("settingById"), new KeysPathComponent("1"))
                    }
                 .ToObservable();

            var route1b = new Route();
            route1b.Handler = path =>
            {
                Assert.Fail("This route should not be used");
                return null;
            };
            var routes1 = new List<Route>{ route1a, route1b };

            var collapsedPaths2 = new List<IPath>
            {
                new Path(new PropertiesPathComponent("settingById"), new KeysPathComponent("1")),
            };
            var route2a = new Route();
            route2a.Handler = path =>
            new[]
                    {
                        PathValue.Create(
                            "test1", 
                            new PropertiesPathComponent("Name"))
                    }
                 .ToObservable();

            var routes2 = new List<Route>{ route2a };

            pathCollapser.Setup(i => i.Collapse(It.IsAny<IEnumerable<IPath>>())).Returns(collapsedPaths1);
            routeResolver.Setup(i => i.FindRoutes(collapsedPaths1[0])).Returns(routes1);
            pathCollapser.Setup(i => i.Collapse(It.IsAny<IEnumerable<IPath>>())).Returns(collapsedPaths2);            
            routeResolver.Setup(i => i.FindRoutes(collapsedPaths2[0])).Returns(routes2);

            var target = new Router(routeResolver.Object, pathCollapser.Object, responseBuilder.Object);

            target.Execute(paths);

        }
    }
}
