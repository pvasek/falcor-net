using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Falcor.Server.Builder
{
    public static class RequestExtensions
    {
        public static Ref CreateRef<TModel, TProperty>(this FalcorRequest<TModel> req,
            Expression<Func<TModel, IDictionary<string, TProperty>>> func, string key)
        {
            var property = ExpressionHelper.GetProperty(func).Name;
            return new Ref(
                new KeysPathComponent(property),
                new KeysPathComponent(key));
        }        
    }
}