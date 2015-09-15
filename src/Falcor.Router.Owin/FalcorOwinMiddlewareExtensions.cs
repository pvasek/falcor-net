using System.Collections.Generic;
using Owin;

namespace Falcor.Router.Owin
{
    public static class FalcorOwinMiddlewareExtensions
    {        
        public static void UseFalcor(this IAppBuilder appBuilder, IList<Route> routes, string path = "/model.json")
        {
            var options = new FalcorOwinMiddlewareOptions
            {                
                ServiceLocator = new FalcorServices(routes),
                Path = path
            };

            appBuilder.UseFalcor(options);
        }

        public static void UseFalcor(this IAppBuilder appBuilder, FalcorOwinMiddlewareOptions options)
        {
            appBuilder.Map(options.Path, app => app.Use<FalcorOwinMiddleware>(options));
        }
    }
}