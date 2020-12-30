using System;

namespace Crude.Models
{
    internal static class TypeExtensions
    {
        public static bool IsGenericBaseType(this object instance, Type generic)
        {
            var baseType = instance.GetType().BaseType;

            if (baseType?.IsGenericType != true)
            {
                return false;
            }

            return baseType.GetGenericTypeDefinition().IsAssignableFrom(generic);
        }
    }
}