using System.Linq;
using CsvHelper.Configuration;

namespace DynamicQuerying.Sample.Extensions
{
    public static class MemberMapExtensions
    {
        public static string GetAssignedName(this MemberMap memberMap)
        {
            var assignedName = memberMap.Data.Names.FirstOrDefault();
            return assignedName ?? memberMap.Data.Member.Name;
        }

        public static string GetOriginalName(this MemberMap memberMap)
        {
            return memberMap.Data.Member.Name;
        }
    }
}