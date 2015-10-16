using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HandlerResult = System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Falcor.Router.PathValue>>;

namespace Falcor.Router.Extensions
{
    public static class TypedRouteListExtensions
    {
        public static IList<Route> MapRoute<TItem1>(this IList<Route> list,
                    TItem1 item1,
                    Func<RouteHandlerContext, TItem1, HandlerResult> handler)
                    where TItem1 : IPathItem
        {
            var route = new Route(
                ctx => handler(ctx, (TItem1)ctx.Path.Items[0]), 
                item1
            );

            list.Add(route);
            return list;
        }

        public static IList<Route> MapRoute<TItem1, TItem2>(this IList<Route> list, 
            TItem1 item1, 
            TItem2 item2,
            Func<RouteHandlerContext, TItem1, TItem2, HandlerResult> handler) 
            where TItem1 : IPathItem 
            where TItem2: IPathItem
        {
            var route = new Route(
                ctx => handler(ctx, 
                    (TItem1)ctx.Path.Items[0], 
                    (TItem2)ctx.Path.Items[1]), 
                item1,
                item2
            );

            list.Add(route);
            return list;
        }

        public static IList<Route> MapRoute<TItem1, TItem2, TItem3>(this IList<Route> list,
            TItem1 item1,
            TItem2 item2,
            TItem3 item3,
            Func<RouteHandlerContext, TItem1, TItem2, TItem3, HandlerResult> handler)
            where TItem1 : IPathItem
            where TItem2 : IPathItem
            where TItem3 : IPathItem
        {
            var route = new Route(
                ctx => handler(ctx,
                    (TItem1)ctx.Path.Items[0],
                    (TItem2)ctx.Path.Items[1],
                    (TItem3)ctx.Path.Items[2]),
                item1,
                item2,
                item3
            );

            list.Add(route);
            return list;
        }

        public static IList<Route> MapRoute<TItem1, TItem2, TItem3, TItem4>(this IList<Route> list,
            TItem1 item1,
            TItem2 item2,
            TItem3 item3,
            TItem4 item4,
            Func<RouteHandlerContext, TItem1, TItem2, TItem3, TItem4, HandlerResult> handler)
            where TItem1 : IPathItem
            where TItem2 : IPathItem
            where TItem3 : IPathItem
            where TItem4 : IPathItem
        {
            var route = new Route(
                ctx => handler(ctx,
                    (TItem1)ctx.Path.Items[0],
                    (TItem2)ctx.Path.Items[1],
                    (TItem3)ctx.Path.Items[2],
                    (TItem4)ctx.Path.Items[3]),
                item1,
                item2,
                item3,
                item4
            );

            list.Add(route);
            return list;
        }
    }
}