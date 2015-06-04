﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Falcor.Server;
using Falcor.Server.Routing;
using Falcor.Server.Routing.Builder;
using Falcor.WebExample;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Owin;
using Path = Falcor.Server.Routing.Path;

[assembly: OwinStartup(typeof(Startup))]

namespace Falcor.WebExample
{
    public partial class Startup
    {
        private Router _falcorRouter;

        public void Configuration(IAppBuilder app)
        {
            app.Map("/model.json", builder =>
            {
                builder.Run(FalcorHandler);
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Try /model.json");
            });

            FalcorInitialize();
        }

        private void FalcorInitialize()
        {
            var routes = new List<Route>();
            routes.MapRoute<Model>()
                .List(i => i.Events)
                .AsIndex()
                .To(p =>
                {
                    var index = ((IntegersPathComponent) p.Components[1]).Integers.First() + 1;
                    var reference = new Ref(
                        new PropertiesPathComponent("EventById"),
                        new KeysPathComponent("980" + index));

                    return
                        Observable.Return(PathValue.Create(reference, new PropertiesPathComponent("Events"),
                            p.Components[1]));
                });

            routes.MapRoute<Model>()
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

            _falcorRouter = new Router(routeResolver, pathCollapser, responseBuilder);
        }

        private async Task FalcorHandler(IOwinContext ctx)
        {
            var path = ctx.Request.Query["path"];

            var json = new JsonSerializer();
            var pathObj = json.Deserialize(new JsonTextReader(new StringReader(path))) as JArray;
            if (pathObj == null)
            {
                throw new ArgumentException("path is not an array");
            }
            var paths = new List<IPath>();
            if (pathObj[0] is JArray)
            {
                paths.AddRange(pathObj.Select(i => ParsePath((JArray) i)));
            }
            else
            {
                paths.Add(ParsePath((pathObj)));
            }
            var result = _falcorRouter.Execute(paths);
            var resultText = new StringWriter();
            json.Serialize(resultText, result);
            ctx.Response.Headers.Set("content-type", "application/json");
            await ctx.Response.WriteAsync(resultText.ToString());
        }

        private static IPath ParsePath(JArray array)
        {
            var components = new List<IPathComponent>();
            foreach (var jToken in array)
            {
                var token = (JValue) jToken;
                if (token.Type == JTokenType.String)
                {
                    components.Add(new PropertiesPathComponent(token.Value<string>()));
                }
                else if (token.Type == JTokenType.Integer)
                {
                    components.Add(new IntegersPathComponent(token.Value<int>()));
                }
            }
            return new Path(components.ToArray());
        }
    }

    public class Model
    {
        public IList<Event> Events { get; set; }
        public IDictionary<string, Event> EventById { get; set; }

        public class Event
        {
            public string Name { get; set; }
            public int Number { get; set; }
        }
    }
}
