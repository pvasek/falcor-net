using System.Linq.Expressions;
using System.Reflection;

namespace Falcor.Server.Routing.Builder
{
    public static class ExpressionHelper
    {
        public static PropertyInfo GetProperty(Expression expression)
        {
            var lambdaExpression = (LambdaExpression) expression;
            if (lambdaExpression != null)
            {
                var memberExpression = lambdaExpression.Body as MemberExpression;
                if (memberExpression != null)
                {
                    return memberExpression.Member as PropertyInfo;
                }
            }

            return null;
        }    
    }
}