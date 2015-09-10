using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcor.Server;
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

            routes.MapRoute(
                Keys.For("EventsQuery"),
                Keys.Any(),
                Keys.Any(),
                Integers.Any(),
                (ctx, eventsQuery, orderby, where, indexes) => 
                    {
                        var events = model.Events.AsEnumerable();
                        if (orderby.HasKey("Name"))
                        {
                            events = events.OrderBy(i => i.Name);
                        } else if (orderby.HasKey("Number"))
                        {
                            events = events.OrderBy(i => i.Number);
                        }
                        var eventsList = events.ToList();

                        return Task.FromResult(indexes.Values
                            .Select(i => new PathValue(
                                new Ref(
                                    new Keys("EventById"),
                                    new Keys(eventsList[i].Id)),
                                ctx.Path.Items[0],
                                ctx.Path.Items[1],
                                ctx.Path.Items[2],
                                new Integers(i)
                            )));
                    });

            routes.MapRoute(
                Keys.For("Events"),
                Integers.Any(),
                (ctx, events, keys) => 
                    {
                        var result = keys.Values
                            .Where(i => i < model.Events.Count)
                            .Select(i => new PathValue(
                                new Ref(Keys.For("EventById"), Keys.For(model.Events[i].Id)),
                                Keys.For("Events"), 
                                Integers.For(i))
                            );

                        return Task.FromResult(result);
                    });

            routes.MapRoute(
                Keys.For("Countries"),
                Integers.Any(),
                (ctx, countries, indexes) =>
                {
                    return Task.FromResult(indexes
                        .Values
                        .Where(i => i < model.Countries.Count)
                        .Select(i => new PathValue(
                            new Ref(Keys.For("CountryById"), Keys.For(model.Countries[i].Id)),
                            countries, Integers.For(i))));
                }
            );

            routes.MapRoute(
                Keys.For("Participants"),
                Integers.Any(),
                (ctx, participants, indexes) =>
                {
                    return Task.FromResult(indexes
                        .Values
                        .Where(i => i < model.Participants.Count)
                        .Select(i => new PathValue(
                            new Ref(Keys.For("ParticipantById"), Keys.For(model.Participants[i].Id)),
                            participants, Integers.For(i))));
                }
            );

            routes.MapRoute(
                Keys.For("EventById"),
                Keys.Any(),
                Keys.For("Name", "Number", "Country"),
                (ctx, eventById, keys, properties) => 
                    {
                        var result = new List<PathValue>();
                        foreach (var key in keys.Values)
                        {
                            var item = model.Events.First(i => i.Id.Equals(key));
                            if (properties.Values.Contains("Name"))
                            {
                                result.Add(new PathValue(item.Name, eventById, Keys.For(key), new Keys("Name")));
                            }
                            if (properties.Values.Contains("Number"))
                            {
                                result.Add(new PathValue(item.Number, eventById, Keys.For(key), new Keys("Number")));
                            }
                            if (properties.Values.Contains("Country"))
                            {
                                var reference = new Ref(new Keys("CountryById"), new Keys(item.Country.Id));
                                result.Add(new PathValue(reference, eventById, Keys.For(key), new Keys("Country")));
                            }
                        }

                        return Task.FromResult(result.AsEnumerable());
                    }
                );

            routes.MapRoute(
                Keys.For("CountryById"),
                Keys.Any(),
                Keys.For("Name", "Description"),
                (ctx, countryById, keys, properties) => 
                {
                    var result = new List<PathValue>();
                    foreach (var key in keys.Values)
                    {
                        var item = model.Countries.First(i => i.Id.Equals(key));

                        if (properties.Values.Contains("Name"))
                        {
                            result.Add(new PathValue(item.Name, countryById, Keys.For(key), new Keys("Name")));
                        }

                        if (properties.Values.Contains("Description"))
                        {
                            result.Add(new PathValue(item.Description, countryById, Keys.For(key), new Keys("Description")));
                        }
                    }

                    return Task.FromResult(result.AsEnumerable());
                });

            routes.MapRoute(
                Keys.For("ParticipantById"),
                Keys.Any(),
                Keys.For("FirstName", "LastName", "Country"),
                (ctx, participantById, keys, properties) =>
                {
                    var result = new List<PathValue>();
                    foreach (var key in keys.Values)
                    {
                        var item = model.Participants.First(i => i.Id.Equals(key));

                        if (properties.Values.Contains("FirstName"))
                        {
                            result.Add(new PathValue(item.FirstName, participantById, Keys.For(key), Keys.For("FirstName")));
                        }
                        if (properties.Values.Contains("LastName"))
                        {
                            result.Add(new PathValue(item.LastName, participantById, Keys.For(key), Keys.For("LastName")));
                        }
                        if (properties.Values.Contains("Country"))
                        {
                            var reference = new Ref(new Keys("CountryById"), new Keys(item.Country.Id));
                            result.Add(new PathValue(reference, participantById, Keys.For(key), new Keys("Country")));
                        }
                    }

                    return Task.FromResult(result.AsEnumerable());
                });

            return routes;
        }      
    }
}
