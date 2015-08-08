using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Falcor.Server.Tests;

namespace Falcor.Server.Builder
{
    public class KeyPropertyFalcorRequest<TModel, TKeyModel> : FalcorRequest<TModel>
    {
        public KeyPropertyFalcorRequest(IPath path, IPath originalPath) : base(path)
        {
            var keysIndex = originalPath.Components.Count - 2;
            var propertiesIndex = originalPath.Components.Count - 1;
            Keys = Path.Components[keysIndex].AllKeys.Select(i => i.ToString()).ToList();
            Properties = ((KeysPathComponent)Path.Components[propertiesIndex]).Keys;
        }

        public IList<string> Keys { get; private set; }
        public IList<string> Properties { get; private set; }

        public bool HasProperty(string property)
        {
            return Properties.Any(i => string.Compare(i, property, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public bool HasProperty<TProperty>(Expression<Func<TKeyModel, TProperty>> func)
        {
            var property = ExpressionHelper.GetProperty(func).Name;
            return HasProperty(property);
        }

        public PathValue CreateResult<TProperty>(Expression<Func<TKeyModel, TProperty>> func, TProperty value, string key)
        {
            var property = ExpressionHelper.GetProperty(func);
            var propertyName = property.Name;
            var propertyKey = GetPropertyKey(property);
            if (!HasProperty(propertyName))
            {
                return null;
            }

            var pathComponents = Path.Components
                .Take(Path.Components.Count - 2)
                .ToList();

            pathComponents.Add(new KeysPathComponent(key));
            pathComponents.Add(new KeysPathComponent(propertyKey ?? propertyName));

            return PathValue.Create(value, pathComponents);
        }

        public PathValue CreateResult<TProperty>(string propertyName, TProperty value, string key)
        {
            if (!HasProperty(propertyName))
            {
                return null;
            }

            var pathComponents = Path.Components
                .Take(Path.Components.Count - 2)
                .ToList();

            pathComponents.Add(new KeysPathComponent(key));
            pathComponents.Add(new KeysPathComponent(propertyName));

            return PathValue.Create(value, pathComponents);
        }

        private string GetPropertyKey(PropertyInfo property)
        {
            return property
                .GetCustomAttributes(typeof(FalcorKeyAttribute), true)
                .Cast<FalcorKeyAttribute>()
                .FirstOrDefault()
                ?.Name;
        }
    }
}