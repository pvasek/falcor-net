using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Falcor.Server;
using Falcor.Server.Routing;
using Falcor.Server.Routing.Builder;

namespace Falcor.Example1
{    
    class Program
    {
        static void Main(string[] args)
        {
            // routes.MapRoute<Model>().Property(m => m.Name).To(handler1)
            // routes.MapRoute<Model>().List(m => m.Events).Item(0).To(handler)
            // routes.MapRoute<Model>().List(m => m.Events).AsRange().Property(c => c.Name).To(handler)

            // routes.MapRoute<Model>(m => m.Name).To(handler1)
            // routes.MapRoute<Model>(m => m.Events[0]).To(handler)
            // routes.MapRoute<Model>(m => m.Events[0-9]).To(handler)
            // routes.MapRoute<Model>(m => m.Events.AsRange().Name).To(handler)
            
            var routes = new List<Route>();           

            // participantById['99805'].name
            // participantById['99805'].number
            // competitorById['99803'].participant
            // competitorById['99803'].result
            // eventById['99801'].name
            // eventById['99801'].from
            // eventById['99801'].to
            // eventById['99801'].participants[0]
            // eventById['99801'].competitors[0]
            // eventById['99801'].referees
            
            // events[0]

            // get, set, call

            // IntegersPathComponent - multiple integers
            // handler return type should be observable of PathValue
            // path value returned by handler can be input for another handlers (during resolving references)
            // path optimization - trade off for retrieving data from db => collapsing

            routes.MapRoute<Model>().List(i => i.Events).AsIndex().To(path => 
                new [] {
                    PathValue.Create(new Ref("eventsById", "99801"), "events", "0"),
                    PathValue.Create(new Ref("eventsById", "99802"), "events", "1"),
                }.ToObservable()
            );

            routes.MapRoute<Model>()
                .Dictionary(i => i.CompetitorsById)
                .AsKey()
                .Properties(i => i.Result)
                .To(path =>
                {
                    return ((KeysPathComponent) path[1])
                        .Keys
                        .Select((i, idx) => PathValue.Create(idx, "eventById", i))
                        .ToObservable();
                });

            // new IList<object>() { "events", new IList<int> { 0, 5, 9} }
        }
    }
}
