using System.Linq.Expressions;
using System.Reflection;

namespace Falcor.Router.Builder
{
    public static class ExpressionHelper
    {
        public static PropertyInfo GetProperty(Expression expression)
        {
            var lambdaExpression = (LambdaExpression) expression;
            MemberExpression memberExpression;
            if (lambdaExpression?.Body.NodeType == ExpressionType.Convert)
            {
                memberExpression = ((UnaryExpression) lambdaExpression.Body).Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambdaExpression?.Body as MemberExpression;
            }            

            return memberExpression?.Member as PropertyInfo;
        }    
    }
}