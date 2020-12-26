using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace DynamicQuerying.Main.Expressions
{
    internal static class BaseExpressions
    {
        public static Expression GetDotMember(Expression expression, string propertyName)
        {
            return propertyName.Split('.').Aggregate(expression, Expression.PropertyOrField);
        }

        public static Expression ConvertValue(Expression property, object value)
        {
            var propertyType = property.Type;
            var constant = Expression
                .Constant(TypeDescriptor.GetConverter(propertyType)
                    .ConvertFromInvariantString(value.ToString()));
            var converted = Expression.Convert(constant, propertyType);
            return converted;
        }
    }
}