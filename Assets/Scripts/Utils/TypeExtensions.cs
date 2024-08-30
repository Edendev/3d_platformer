using System;
using System.Reflection;
using System.Collections.Generic;

namespace Utils
{    
    public static class TypeExtensions
    {
        public static bool IsGenericList(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}