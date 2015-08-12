using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcor.Server;
using Falcor.Server.Builder;
using Falcor.Server.Extensions;
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

            routes.MapRoute()
                .PathItem(Keys.For("EventsQuery"))
                .PathItem("orderby", Keys.Any())
                .PathItem("where", Keys.Any())
                .PathItem("indexes", Integers.Any())
                .To(ctx => 
                    {
                        var orderComponent = ctx.Item<Keys>("orderby");
                        var filterComponent = ctx.Item<Keys>("where");
                        var intPathComponent = ctx.Item<Integers>("indexes");

                        var events = model.Events.AsEnumerable();
                        if (orderComponent.HasKey("Name"))
                        {
                            events = events.OrderBy(i => i.Name);
                        } else if (orderComponent.HasKey("Number"))
                        {
                            events = events.OrderBy(i => i.Number);
                        }
                        var eventsList = events.ToList();

                        return Task.FromResult(intPathComponent.Values
                            .Select(i => PathValue.Create(
                                new Ref(
                                    new Keys("EventById"),
                                    new Keys(eventsList[i].Id)),
                                ctx.Path.Items[0],
                                ctx.Path.Items[1],
                                ctx.Path.Items[2],
                                new Integers(i)
                            )));
                    });

            routes.MapRoute()
                .PathItem(Keys.For("Events"))
                .PathItem("indexes", Integers.Any())
                .To(ctx =>
                {
                    var indexes = ctx.Item<Integers>("indexes").Values;
                    var result = indexes
                        .Where(i => i < model.Events.Count)
                        .Select(i => PathValue.Create(
                            new Ref(Keys.For("EventById"), Keys.For(model.Events[i].Id)),
                            Keys.For("Events"), 
                            Integers.For(i))
                        );

                    return Task.FromResult(result);
                });

            routes.MapRoute()
                .PathItem(Keys.For("Countries"))
                .PathItem(Integers.Any())
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
                    var key = (string) p.Items[1].Value;
                    var properties = (Keys) p.Items[2];
                    var result = new List<PathValue>();
                    var item = model.Events.First(i => i.Id.Equals(key));
                    if (properties.Values.Contains("Name"))
                    {
                        result.Add(PathValue.Create(item.Name, p.Items[0], p.Items[1], new Keys("Name")));
                    }
                    if (properties.Values.Contains("Number"))
                    {
                        result.Add(PathValue.Create(item.Number, p.Items[0], p.Items[1], new Keys("Number")));
                    }
                    if (properties.Values.Contains("Country"))
                    {
                        var reference = new Ref(new Keys("CountryById"), new Keys(item.Country.Id));
                        result.Add(PathValue.Create(reference, p.Items[0], p.Items[1], new Keys("Country")));
                    }

                    return Task.FromResult(result.AsEnumerable());
                });

            routes.MapRoute<Model>()
                .Dictionary(i => i.CountryById)
                .AsKey()
                .Properties(i => i.Name, i => i.Description)
                .To(p =>
                {
                    var list = p.Items[1]
                        .AllKeys
                        .Select(key =>
                        {
                            var properties = (Keys) p.Items[2];
                            var result = new List<PathValue>();
                            if (properties.Values.Contains("Name"))
                            {
                                result.Add(PathValue.Create("club" + key, p.Items[0], 
                                    new Keys((string)key),
                                    new Keys("Name")));
                            }
                            if (properties.Values.Contains("Description"))
                            {
                                result.Add(PathValue.Create(String.Format("club{0} description", key), p.Items[0],
                                    new Keys((string)key),
                                    new Keys("Description")));
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
                    context.Values: IList<string>
                    context.Properties: IList<string>
                    context.HasProperty(i => i.FirstName)
                    result.AddProperty(i => i.FirstName, "first" + key): PathValue
                    result.Done()
                    */
                    var key = p.Items[1].Value;
                    var properties = (Keys)p.Items[2];
                    var result = new List<PathValue>();
                    if (properties.Values.Contains("FirstName"))
                    {
                        result.Add(PathValue.Create("first" + key, p.Items[0], p.Items[1], new Keys("FirstName")));
                    }
                    if (properties.Values.Contains("LastName"))
                    {
                        result.Add(PathValue.Create("last" + key, p.Items[0], p.Items[1], new Keys("LastName")));
                    }

                    return Task.FromResult(result.AsEnumerable());
                });
            return routes;
        }      
    }
}
