using System;
using System.Linq;
using System.Reflection;
using DynamicQuerying.Sample.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DynamicQuerying.Sample.Mapping.Base
{
    public class JsonHeaderContractResolver<T> : DefaultContractResolver
    {
        private readonly HeaderMapping<T> _headerMapping;

        public JsonHeaderContractResolver(HeaderMapping<T> headerMapping)
        {
            _headerMapping = headerMapping;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (property.DeclaringType != typeof(T)) return property;

            var assignedHeader = _headerMapping.MemberMaps
                .FirstOrDefault(map => map.GetOriginalName() == property.PropertyName);
            property.PropertyName = assignedHeader.GetAssignedName();
            return property;
        }
    }
}