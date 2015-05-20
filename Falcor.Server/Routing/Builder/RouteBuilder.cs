using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Falcor.Server.Routing.Builder
{
    public static class RouteBuilder
    {
        public static PropertyRouteJourney<T> MapRoute<T>(this IList<Route> routes)
        {
            return new PropertyRouteJourney<T>(new Route(), routes);
        }

        public static PropertyRouteJourney<TProperty> Property<T, TProperty>(this PropertyRouteJourney<T> journey, Expression<Func<T, TProperty>> func)
        {
            var propertyInfo = ExpressionHelper.GetProperty(func);
            if (typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType))
            {
                throw new ArgumentException("For properties implementing ICollection you have to use 'List' method");
            }

            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Add(new PropertiesPathComponent(propertyInfo.Name));
            return new PropertyRouteJourney<TProperty>(routeJourney.Route, routeJourney.Routes);
        }

        public static IFinalRouteJourney Properties<T>(this PropertyRouteJourney<T> journey, params Expression<Func<T, object>>[] funcs)
        {
            var properties = funcs
                .Select(ExpressionHelper.GetProperty)
                .ToList();

            if (properties.Any(i => typeof(ICollection).IsAssignableFrom(i.PropertyType)))
            {
                throw new ArgumentException("For properties implementing ICollection you have to use 'List' method");
            }

            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Add(new PropertiesPathComponent(properties.Select(i => i.Name).ToArray()));
            return new FinalRouteJourney(routeJourney.Route, routeJourney.Routes);
        }

        public static ListRouteJourney<TProperty> List<T, TProperty>(this PropertyRouteJourney<T> journey, Expression<Func<T, IList<TProperty>>> func)
        {            
            var propertyInfo = ExpressionHelper.GetProperty(func);
            if (!typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType))
            {
                throw new ArgumentException("For simple properties you have to use 'Property' method");
            }

            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Add(new PropertiesPathComponent(propertyInfo.Name));
            return new ListRouteJourney<TProperty>(routeJourney.Route, routeJourney.Routes);
        }

        public static DictionarRouteJourney<TProperty> Dictionary<T, TKey, TProperty>(this PropertyRouteJourney<T> journey, Expression<Func<T, IDictionary<TKey, TProperty>>> func)
        {
            var propertyInfo = ExpressionHelper.GetProperty(func);
            if (!typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType))
            {
                throw new ArgumentException("For simple properties you have to use 'Property' method");
            }

            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Add(new PropertiesPathComponent(propertyInfo.Name));
            return new DictionarRouteJourney<TProperty>(routeJourney.Route, routeJourney.Routes);
        }

        public static PropertyRouteJourney<T> AsRange<T>(this ListRouteJourney<T> journey, int? from, int? to)
        {
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Add(new RangePathComponent(from, to));
            return new PropertyRouteJourney<T>(routeJourney.Route, routeJourney.Routes);
        }

        public static PropertyRouteJourney<T> AsIndex<T>(this ListRouteJourney<T> journey, int? index = null)
        {
            var routeJourney = (IRouteJourney)journey;
            var integers = index != null ? new[] { index.Value } : null;
            routeJourney.Route.Path.Add(new IntegersPathComponent(integers));
            return new PropertyRouteJourney<T>(routeJourney.Route, routeJourney.Routes);
        }

        public static PropertyRouteJourney<T> AsKey<T>(this DictionarRouteJourney<T> journey, params string[] keys)
        {
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Add(new KeysPathComponent(keys));
            return new PropertyRouteJourney<T>(routeJourney.Route, routeJourney.Routes);
        }

        public static void To(this IFinalRouteJourney journey, Handler handler)
        {
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Handler = handler;
            routeJourney.Routes.Add(routeJourney.Route);
        }

        public interface IRouteJourney
        {
            Route Route { get; set; }
            IList<Route> Routes { get; set; }
        }

        public interface IFinalRouteJourney : IRouteJourney
        {
        }

        private class FinalRouteJourney : RouteJourney, IFinalRouteJourney
        {
            public FinalRouteJourney(Route route, IList<Route> routes) : base(route, routes)
            {
            }
        }

        public class RouteJourney : IRouteJourney
        {
            public RouteJourney(Route route, IList<Route> routes)
            {
                ((IRouteJourney)this).Route = route;
                ((IRouteJourney)this).Routes = routes;
            }

            Route IRouteJourney.Route { get; set; }
            IList<Route> IRouteJourney.Routes { get; set; }
        }

        public class PropertyRouteJourney<T> : RouteJourney, IFinalRouteJourney
        {
            public PropertyRouteJourney(Route route, IList<Route> routes)
                : base(route, routes)
            {
            }
        }

        public class ListRouteJourney<T> : RouteJourney
        {
            public ListRouteJourney(Route route, IList<Route> routes)
                : base(route, routes)
            {
            }
        }
    
        public class DictionarRouteJourney<T> : RouteJourney
        {
            public DictionarRouteJourney(Route route, IList<Route> routes)
                : base(route, routes)
            {
            }
        }
    }
}