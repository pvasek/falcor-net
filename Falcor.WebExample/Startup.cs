using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Falcor.Server;
using Falcor.Server.Builder;
using Falcor.Server.Owin;
using Falcor.WebExample;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Falcor.WebExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseStaticFiles();
            app.UseFalcor(GetFalcorRoutes());

            app.Run(async context =>
            {                
                await context.Response.WriteAsync(ContentHelper.GetIndex());
            });
        }

        private List<Route> GetFalcorRoutes()
        {
            var routes = new List<Route>();

            routes.MapRoute<Model>()
                .List(i => i.Events)
                .AsIndex()
                .ToRoute(req =>
                {
                    var result = req
                        .Indexes
                        .Select(i => req.CreateResult(i,
                            req.CreateRef(m => m.EventById, "980" + i)));

                    return result.ToObservable();
                });

            routes.MapRoute<Model>()
                .List(i => i.Clubs)
                .AsIndex()
                .ToRoute(req =>
                {
                    return req
                        .Indexes
                        .Select(i => req.CreateResult(i,
                            req.CreateRef(m => m.ClubById, "600" + i)))
                        .ToObservable();
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
                        result.Add(PathValue.Create("name" + key, p.Components[0], p.Components[1], new KeysPathComponent("Name")));
                    }
                    if (properties.Keys.Contains("Number"))
                    {
                        result.Add(PathValue.Create(Int32.Parse(key), p.Components[0], p.Components[1], new KeysPathComponent("Number")));
                    }
                    if (properties.Keys.Contains("Club"))
                    {
                        var reference = new Ref(new KeysPathComponent("ClubById"), new KeysPathComponent(key));
                        result.Add(PathValue.Create(reference, p.Components[0], p.Components[1], new KeysPathComponent("Club")));
                    }

                    return result.ToObservable();
                });

            routes.MapRoute<Model>()
                .Dictionary(i => i.ClubById)
                .AsKey()
                .Properties(i => i.Name, i => i.Description)
                .To(p =>
                {
                    var list = p.Components[1]
                        .AllKeys
                        .Select(key =>
                        {
                            var properties = (KeysPathComponent) p.Components[2];
                            var result = new List<PathValue>();
                            if (properties.Keys.Contains("Name"))
                            {
                                result.Add(PathValue.Create("club" + key, p.Components[0], 
                                    new KeysPathComponent((string)key),
                                    new KeysPathComponent("Name")));
                            }
                            if (properties.Keys.Contains("Description"))
                            {
                                result.Add(PathValue.Create(String.Format("club{0} description", key), p.Components[0],
                                    new KeysPathComponent((string)key),
                                    new KeysPathComponent("Description")));
                            }
                            return result;
                        })
                        .SelectMany(i => i);

                    return list.ToObservable();
                });

            routes.MapRoute<Model>()
                .Dictionary(i => i.ParticipantById)
                .AsKey()
                .Properties(i => i.FirstName, i => i.LastName)
                .To(p =>
                {
                    /*
                    context.Keys: IList<string>
                    context.Properties: IList<string>
                    context.HasProperty(i => i.FirstName)
                    result.AddProperty(i => i.FirstName, "first" + key): PathValue
                    result.Done()
                    */
                    var key = p.Components[1].Key;
                    var properties = (KeysPathComponent)p.Components[2];
                    var result = new List<PathValue>();
                    if (properties.Keys.Contains("FirstName"))
                    {
                        result.Add(PathValue.Create("first" + key, p.Components[0], p.Components[1], new KeysPathComponent("FirstName")));
                    }
                    if (properties.Keys.Contains("LastName"))
                    {
                        result.Add(PathValue.Create("last" + key, p.Components[0], p.Components[1], new KeysPathComponent("LastName")));
                    }

                    return result.ToObservable();
                });
            return routes;
        }      
    }
}
