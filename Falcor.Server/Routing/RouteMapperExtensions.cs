using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Falcor.Server.Routing
{
    public static class RouteMapperExtensions
    {
        public static PropertyRouteJourney<T> MapRoute<T>(this IList<Route> routes)
        {
            return new PropertyRouteJourney<T>(new Route(), routes);
        }

        public static PropertyRouteJourney<TProperty> Property<T, TProperty>(this PropertyRouteJourney<T> route, Expression<Func<T, TProperty>> func)
        {
            var propertyInfo = ExpressionHelper.GetProperty(func);
            if (typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType))
            {
                throw new ArgumentException("For properties implementing ICollection you have to use 'List' method");
            }
            
            route.Route.Path.Add(new PropertyPathFragment(propertyInfo.Name));
            return new PropertyRouteJourney<TProperty>(route.Route, route.Routes);
        }

        public static RouteJourney Properties<T>(this PropertyRouteJourney<T> route, params Expression<Func<T, object>>[] funcs)
        {
            var properties = funcs
                .Select(ExpressionHelper.GetProperty)
                .ToList();

            if (properties.Any(i => typeof(ICollection).IsAssignableFrom(i.PropertyType)))
            {
                throw new ArgumentException("For properties implementing ICollection you have to use 'List' method");
            }
            route.Route.Path.Add(new PropertiesPathFragment{ Keys = properties.Select(i => i.Name).ToList() });
            return new RouteJourney(route.Route, route.Routes);
        }

        public static ListRouteJourney<TProperty> List<T, TProperty>(this PropertyRouteJourney<T> route, Expression<Func<T, IList<TProperty>>> func)
        {
            var propertyInfo = ExpressionHelper.GetProperty(func);
            if (!typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType))
            {
                throw new ArgumentException("For simple properties you have to use 'Property' method");
            }
            
            route.Route.Path.Add(new ListPathFragment(propertyInfo.Name));
            return new ListRouteJourney<TProperty>(route.Route, route.Routes);
        }

        public static PropertyRouteJourney<T> AsRange<T>(this ListRouteJourney<T> journey)
        {
            return new PropertyRouteJourney<T>(journey.Route, journey.Routes);
        }

        public static void To(this RouteJourney journey, Func<Task> handler)
        {
            journey.Route.Handler = handler;
            journey.Routes.Add(journey.Route);
        }
    }
}