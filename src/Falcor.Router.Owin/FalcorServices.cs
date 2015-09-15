using System;
using System.Collections.Generic;

namespace Falcor.Router.Owin
{
    public class FalcorServices: IServiceLocator
    {
        public FalcorServices(IList<Route> routes)
        {
            Register(resolver => routes);
            Register<IPathParser>(resolver => new PathParser());
            Register<IResponseSerializer>(resolver => new ResponseSerializer());
            Register<IPathCollapser>(resolver => new PathCollapser());
            Register<IResponseBuilder>(resolver => new ResponseBuilder());
            Register<IRouteResolver>(resolver => new RouteResolver(resolver.Get<IList<Route>>()));
        }

        private readonly Dictionary<Type, Func<IServiceLocator, object>> _services 
            = new Dictionary<Type, Func<IServiceLocator, object>>();        

        public void Register<T>(Func<IServiceLocator, T> getService) where T: class
        {
            _services.Add(typeof(T), getService);
        }

        public T Get<T>()
        {
            return (T)_services[typeof (T)](this);
        }
    }
}