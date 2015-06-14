using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Web.Hosting;
using Falcor.Server;
using Falcor.Server.Builder;
using Falcor.WebExample;
using Kiwi.Markdown;
using Kiwi.Markdown.ContentProviders;
using Microsoft.Owin;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json.Linq;
using Owin;
using Path = Falcor.Server.Path;

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

            app.UseStaticFiles();

            app.Run(async context =>
            {                
                await context.Response.WriteAsync(ContentHelper.GetIndex());
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
                        new KeysPathComponent("EventById"),
                        new KeysPathComponent("980" + index));

                    return Observable.Return(PathValue.Create(reference, p.Components.Take(2)));
                });

            routes.MapRoute<Model>()
                .List(i => i.Clubs)
                .AsIndex()
                .To(p =>
                {
                    var index = ((IntegersPathComponent)p.Components[1]).Integers.First() + 1;
                    var reference = new Ref(
                        new KeysPathComponent("ClubById"),
                        new KeysPathComponent("600" + index));

                    return Observable.Return(PathValue.Create(reference, p.Components.Take(2)));
                });

            routes.MapRoute<Model>()
                .Dictionary(i => i.EventById)
                .AsKey()
                .Properties(i => i.Name, i => i.Number, i => i.Club)
                .To(p =>
                {
                    var key = (string) p.Components[1].Key;
                    var properties = (KeysPathComponent) p.Components[2];
                    var result = new List<PathValue>();
                    if (properties.Keys.Contains("Name"))
                    {
                        result.Add(PathValue.Create("name" + key, p));
                    }
                    if (properties.Keys.Contains("Number"))
                    {
                        result.Add(PathValue.Create(Int32.Parse(key), p));
                    }
                    if (properties.Keys.Contains("Club"))
                    {
                        var reference = new Ref(new KeysPathComponent("ClubById"), new KeysPathComponent(key));
                        result.Add(PathValue.Create(reference, p.Components.Take(3)));
                    }

                    return result.ToObservable();
                });

            routes.MapRoute<Model>()
                .Dictionary(i => i.ClubById)
                .AsKey()
                .Properties(i => i.Name, i => i.Description)
                .To(p =>
                {
                    var key = p.Components[1].Key;
                    var properties = (KeysPathComponent)p.Components[2];
                    var result = new List<PathValue>();
                    if (properties.Keys.Contains("Name"))
                    {
                        result.Add(PathValue.Create("club" + key, p));
                    }
                    if (properties.Keys.Contains("Description"))
                    {
                        result.Add(PathValue.Create(String.Format("club{0} description", key), p));
                    }

                    return result.ToObservable();
                });

            routes.MapRoute<Model>()
                .Dictionary(i => i.ParticipantById)
                .AsKey()
                .Properties(i => i.FirstName, i => i.LastName)
                .To(p =>
                {
                    var key = p.Components[1].Key;
                    var properties = (KeysPathComponent)p.Components[2];
                    var result = new List<PathValue>();
                    if (properties.Keys.Contains("FirstName"))
                    {
                        result.Add(PathValue.Create("first" + key, p));
                    }
                    if (properties.Keys.Contains("LastName"))
                    {
                        result.Add(PathValue.Create("last" + key, p));
                    }

                    return result.ToObservable();
                });

            var routeResolver = new RouteResolver(routes);
            var pathCollapser = new PathCollapser();
            var responseBuilder = new ResponseBuilder();

            _falcorRouter = new Router(routeResolver, pathCollapser, responseBuilder);
        }

        private async Task FalcorHandler(IOwinContext ctx)
        {
            var path = ctx.Request.Query["path"];
            var pathParser = new PathParser();
            var result = _falcorRouter.Execute(pathParser.ParsePaths(path));
            var serializer = new ResponseSerializer();
            ctx.Response.Headers.Set("content-type", "application/json");
            await ctx.Response.WriteAsync(serializer.Serialize(result));
        }
    }
}
