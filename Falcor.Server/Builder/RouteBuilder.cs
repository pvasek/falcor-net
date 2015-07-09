using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Falcor.Server.Builder
{
    public static class RouteBuilder
    {
        public static PropertyRouteJourney<TModel, TModel> MapRoute<TModel>(this IList<Route> routes)
        {
            return new PropertyRouteJourney<TModel, TModel>(new Route(), routes);
        }

        public static PropertyRouteJourney<TModel, TProperty> Property<TModel, T, TProperty>(this PropertyRouteJourney<TModel, T> journey, Expression<Func<T, TProperty>> func)
        {
            var propertyInfo = ExpressionHelper.GetProperty(func);
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Components.Add(new KeysPathComponent(propertyInfo.Name));
            return new PropertyRouteJourney<TModel, TProperty>(routeJourney.Route, routeJourney.Routes);
        }

        public static IFinalRouteJourney Properties<TModel ,T>(this PropertyRouteJourney<TModel, T> journey, params Expression<Func<T, object>>[] funcs)
        {
            var properties = funcs
                .Select(ExpressionHelper.GetProperty)
                .ToList();

            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Components.Add(new KeysPathComponent(properties.Select(i => i.Name).ToArray()));
            return new FinalRouteJourney(routeJourney.Route, routeJourney.Routes);
        }

        public static KeyPropertyRouteJourney<TModel,T> Properties<TModel, T>(this KeyRouteJourney<TModel, T> journey, params Expression<Func<T, object>>[] funcs)
        {
            var properties = funcs
                .Select(ExpressionHelper.GetProperty)
                .ToList();

            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Components.Add(new KeysPathComponent(properties.Select(i => i.Name).ToArray()));
            return new KeyPropertyRouteJourney<TModel, T>(routeJourney.Route, routeJourney.Routes);
        }

        public static KeyPropertyRouteJourney<TModel, T> Properties<TModel, T>(this ListRouteJourney<TModel, T> journey, params string[] properties)
        {
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Components.Add(new KeysPathComponent(properties));
            return new KeyPropertyRouteJourney<TModel, T>(routeJourney.Route, routeJourney.Routes);
        }

        public static ListRouteJourney<TModel, TProperty> List<TModel, T, TProperty>(this PropertyRouteJourney<TModel, T> journey, Expression<Func<T, IList<TProperty>>> func)
        {            
            var propertyInfo = ExpressionHelper.GetProperty(func);
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Components.Add(new KeysPathComponent(propertyInfo.Name));
            return new ListRouteJourney<TModel, TProperty>(routeJourney.Route, routeJourney.Routes);
        }

        public static DictionarRouteJourney<TModel, TProperty> Dictionary<TModel, T, TKey, TProperty>(this PropertyRouteJourney<TModel, T> journey, Expression<Func<T, IDictionary<TKey, TProperty>>> func)
        {
            var propertyInfo = ExpressionHelper.GetProperty(func);
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Components.Add(new KeysPathComponent(propertyInfo.Name));
            return new DictionarRouteJourney<TModel, TProperty>(routeJourney.Route, routeJourney.Routes);
        }

        public static PropertyRouteJourney<TModel, T> AsRange<TModel, T>(this ListRouteJourney<TModel, T> journey, int? from, int? to)
        {
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Components.Add(new RangePathComponent(from, to));
            return new PropertyRouteJourney<TModel, T>(routeJourney.Route, routeJourney.Routes);
        }

        public static IndexRouteJourney<TModel, T> AsIndex<TModel, T>(this ListRouteJourney<TModel, T> journey, int? index = null)
        {
            var routeJourney = (IRouteJourney)journey;
            var integers = index != null ? new[] { index.Value } : null;
            routeJourney.Route.Path.Components.Add(new IntegersPathComponent(integers));
            return new IndexRouteJourney<TModel, T>(routeJourney.Route, routeJourney.Routes);
        }

        public static KeyRouteJourney<TModel, T> AsKey<TModel, T>(this DictionarRouteJourney<TModel, T> journey, params string[] keys)
        {
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Path.Components.Add(new KeysPathComponent(keys));
            return new KeyRouteJourney<TModel, T>(routeJourney.Route, routeJourney.Routes);
        }

        public static void To(this IFinalRouteJourney journey, Handler handler)
        {
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Handler = handler;
            routeJourney.Routes.Add(routeJourney.Route);
        }

        public static void ToRoute<TModel, T>(this IndexRouteJourney<TModel, T> journey, Func<IndexFalcorRequest<TModel>, Task<IEnumerable<PathValue>>> handler)
        {
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Handler = (path) => handler(new IndexFalcorRequest<TModel>(path, routeJourney.Route.Path));
            routeJourney.Routes.Add(routeJourney.Route);
        }

        public static void ToRoute<TModel, T>(this KeyPropertyRouteJourney<TModel, T> journey, Func<KeyPropertyFalcorRequest<TModel, T>, Task<IEnumerable<PathValue>>> handler)
        {
            var routeJourney = (IRouteJourney)journey;
            routeJourney.Route.Handler = (path) => handler(new KeyPropertyFalcorRequest<TModel, T>(path, routeJourney.Route.Path));
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
            protected RouteJourney(Route route, IList<Route> routes)
            {
                ((IRouteJourney)this).Route = route;
                ((IRouteJourney)this).Routes = routes;
            }

            Route IRouteJourney.Route { get; set; }
            IList<Route> IRouteJourney.Routes { get; set; }
        }

        public class PropertyRouteJourney<TModel ,T> : RouteJourney, IFinalRouteJourney
        {
            public PropertyRouteJourney(Route route, IList<Route> routes)
                : base(route, routes)
            {
            }
        }

        public class IndexRouteJourney<TModel, T> : PropertyRouteJourney<TModel, T>
        {
            public IndexRouteJourney(Route route, IList<Route> routes)
                : base(route, routes)
            {
            }
        }

        public class KeyRouteJourney<TModel, T> : PropertyRouteJourney<TModel, T>
        {
            public KeyRouteJourney(Route route, IList<Route> routes)
                : base(route, routes)
            {
            }
        }

        public class KeyPropertyRouteJourney<TModel, T> : KeyRouteJourney<TModel, T>
        {
            public KeyPropertyRouteJourney(Route route, IList<Route> routes)
                : base(route, routes)
            {
            }
        }

        public class ListRouteJourney<TModel, T> : RouteJourney
        {
            public ListRouteJourney(Route route, IList<Route> routes)
                : base(route, routes)
            {
            }
        }
    
        public class DictionarRouteJourney<TModel, T> : RouteJourney
        {
            public DictionarRouteJourney(Route route, IList<Route> routes)
                : base(route, routes)
            {
            }
        }
    }
}