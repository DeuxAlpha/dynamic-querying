using System;

namespace DynamicQuerying.Main.Extensions;

public static class EnumEx
{
    public static T ToEnum<T>(string val, bool ignoreCase = true) where T : struct
    {
        return Enum.TryParse<T>(val, ignoreCase, out var result) ? result : default(T);
    }
}