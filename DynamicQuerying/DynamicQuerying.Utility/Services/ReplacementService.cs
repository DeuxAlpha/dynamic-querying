using System.Reflection;
using DynamicQuerying.Utility.Extensions;

namespace DynamicQuerying.Utility.Services
{
    public static class ReplacementService
    {
        public static void ReplaceProperties(object original, object transformed)
        {
            var properties = original.GetType().GetProperties(BindingFlags.Public);
            foreach (var property in properties)
            {
                original.AssignValue(property.Name, transformed.RetrieveValue(property.Name));
            }
        }

        public static void UpdateProperties(object original, object transformed)
        {
            var properties = original.GetType().GetProperties(BindingFlags.Public);
            foreach (var property in properties)
            {
                var newValue = transformed.RetrieveValue(property.Name);
                if (newValue == null) continue;
                original.AssignValue(property.Name, newValue);
            }
        }
    }
}