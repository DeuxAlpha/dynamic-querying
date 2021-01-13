using System;

namespace DynamicQuerying.Sample.Extensions
{
    public static class ObjectExtensions
    {
        public static void AssignValue(this object theObject, string propertyName, object value)
        {
            var property = theObject.GetType().GetProperty(propertyName);
            if (property == null) throw new NullReferenceException(nameof(property));

            if (value != null)
            {
                // Handling nullable.
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    propertyType = propertyType.GetGenericArguments()[0];

                // Try casting automatically
                var converted = Convert.ChangeType(value.ToString(), propertyType);
                property.SetValue(theObject, converted);
            }
            else
            {
                property.SetValue(theObject, null);
            }
        }

        public static object RetrieveValue(this object theObject, string propertyName)
        {
            var property = theObject.GetType().GetProperty(propertyName);
            if (property == null) throw new NullReferenceException(nameof(property));
            return property.GetValue(theObject);
        }
    }
}