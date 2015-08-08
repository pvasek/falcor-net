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
            var model = ModelGenerator.Generate();
            var routes = new List<Route>();

            var route = new Route();
            route.Path.Components.Add(new KeysPathComponent("EventsQuery"));
            route.Path.Components.Add(new KeysPathComponent());
            route.Path.Components.Add(new KeysPathComponent());
            route.Path.Components.Add(new IntegersPathComponent());
            route.Handler = path =>
            {
                var orderComponent = (KeysPathComponent) path.Components[1];
                var filterComponent = (KeysPathComponent)path.Components[2];
                var intPathComponent = (IntegersPathComponent)path.Components[3];

                var events = model.Events.AsEnumerable();
                if (orderComponent.Key.Equals("Name"))
                {
                    events = events.OrderBy(i => i.Name);
                } else if (orderComponent.Key.Equals("Number"))
                {
                    events = events.OrderBy(i => i.Number);
                }
                var eventsList = events.ToList();

                return Task.FromResult(intPathComponent.Integers
                    .Select(i => PathValue.Create(
                        new Ref(
                            new KeysPathComponent("EventById"),
                            new KeysPathComponent(eventsList[i].Id)),
                        path.Components[0],
                        path.Components[1],
                        path.Components[2],
                        new IntegersPathComponent(i)
                    )));
            };

            routes.Add(route);

            routes.MapRoute<Model>()
                .List(i => i.Events)
                .AsIndex()
                .ToRoute(req =>
                {
                    var result = req
                        .Indexes
                        .Where(i => i < model.Events.Count)
                        .Select(i => req.CreateResult(i,
                            req.CreateRef(m => m.EventById, model.Events[i].Id)));

                    return Task.FromResult(result);
                });

            routes.MapRoute<Model>()
                .List(i => i.Countries)
                .AsIndex()
                .ToRoute(req =>
                {
                    return Task.FromResult(req
                        .Indexes
                        .Select(i => req.CreateResult(i,
                            req.CreateRef(m => m.CountryById, model.Countries[i].Id))));
                });

            routes.MapRoute<Model>()
                .Dictionary(i => i.EventById)
                .AsKey()
                .Properties(i => i.Name, i => i.Number, i => i.Country)
                .To(p =>
                {
                    var key = (string) p.Components[1].Key;
                    var properties = (KeysPathComponent) p.Components[2];
                    var result = new List<PathValue>();
                    var item = model.Events.First(i => i.Id.Equals(key));
                    if (properties.Keys.Contains("Name"))
                    {
                        result.Add(PathValue.Create(item.Name, p.Components[0], p.Components[1], new KeysPathComponent("Name")));
                    }
                    if (properties.Keys.Contains("Number"))
                    {
                        result.Add(PathValue.Create(item.Number, p.Components[0], p.Components[1], new KeysPathComponent("Number")));
                    }
                    if (properties.Keys.Contains("Country"))
                    {
                        var reference = new Ref(new KeysPathComponent("CountryById"), new KeysPathComponent(item.Country.Id));
                        result.Add(PathValue.Create(reference, p.Components[0], p.Components[1], new KeysPathComponent("Country")));
                    }

                    return Task.FromResult(result.AsEnumerable());
                });

            routes.MapRoute<Model>()
                .Dictionary(i => i.CountryById)
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

                    return Task.FromResult(list);
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

                    return Task.FromResult(result.AsEnumerable());
                });
            return routes;
        }      
    }
}
