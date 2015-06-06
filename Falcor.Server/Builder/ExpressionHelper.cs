using System.Linq.Expressions;
using System.Reflection;

namespace Falcor.Server.Builder
{
    public static class ExpressionHelper
    {
        public static PropertyInfo GetProperty(Expression expression)
        {
            var lambdaExpression = (LambdaExpression) expression;
            var memberExpression = lambdaExpression?.Body as MemberExpression;
            return memberExpression?.Member as PropertyInfo;
        }    
    }
}