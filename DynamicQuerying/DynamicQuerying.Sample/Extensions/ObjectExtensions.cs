using System;

namespace DynamicQuerying.Sample.Extensions
{
    public static class ObjectExtensions
    {
        public static void AssignValue(this object theObject, string propertyName, object value)
        {
            var property = theObject.GetType().GetProperty(propertyName);
            if (property == null) throw new NullReferenceException(nameof(property));

            // Try casting automatically
            var converted = Convert.ChangeType(value.ToString(), property.PropertyType);
            property.SetValue(theObject, converted);
        }

        public static object RetrieveValue(this object theObject, string propertyName)
        {
            var property = theObject.GetType().GetProperty(propertyName);
            if (property == null) throw new NullReferenceException(nameof(property));
            return property.GetValue(theObject);
        }
    }
}