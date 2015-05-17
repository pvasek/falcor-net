using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            
            routes.MapRoute<Model>()
                .List(i => i.Events)
                .AsRange(0, 10)
                .List(i => i.Competitions)
                .AsIndex()
                .Properties(i => i.Name, i => i.Competitors)
                .To(() => Task.FromResult(0));

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
            // path value returned by handler can be input for another handlers (during resolving reference)
            // path optimization - trade off for retrieving data from db => collapsing

            routes.MapRoute<Model>().List(i => i.Events).AsIndex().To(() => 
                Task.FromResult(Tuple.Create(new List<string> { "events", "0"}, new Ref("events", "0"))));  // path, value
            
            routes.MapRoute<Model>()
                .List(i => i.Events)
                .AsIndex()

            // new IList<object>() { "events", new IList<int> { 0, 5, 9} }
        }
    }

    public class Ref
    {
        public Ref(params string[] path)
        {

        }
    }
   
}
