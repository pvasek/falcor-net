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
        }
    }

   
}
