using System;

namespace Crude.Core
{
    internal static class TypeExtensions
    {
        public static bool IsGenericBaseType(this Type propertyType, Type generic)
        {
            var baseType = propertyType.BaseType;

            if (baseType?.IsGenericType != true)
            {
                return false;
            }

            return baseType.GetGenericTypeDefinition().IsAssignableFrom(generic);
        }

        public static Type UnwrapNullable(this Type type)
        {
            var nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType != null)
            {
                return nullableType;
            }

            return type;
        }
    }
}