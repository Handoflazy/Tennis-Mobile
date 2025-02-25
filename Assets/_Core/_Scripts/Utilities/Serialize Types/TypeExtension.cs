using System;
using System.Linq;

namespace Platformer.Extension
{
    /// <summary>
    /// Provides extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Resolves the generic type definition of the specified type if it is a generic type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>The generic type definition if the specified type is a generic type; otherwise, the original type.</returns>
        static Type ResolveGenericType(Type type)
        {
            if (type is not { IsGenericType: true }) return type;
            var genericType = type.GetGenericTypeDefinition();
            return genericType != type ? genericType : type;
        }

        /// <summary>
        /// Determines whether the specified type implements any interfaces that match the specified interface type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="interfaceType">The interface type to match.</param>
        /// <returns><c>true</c> if the specified type implements any interfaces that match the specified interface type; otherwise, <c>false</c>.</returns>
        static bool hasAnyIntefaces(Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(i => ResolveGenericType(i) == interfaceType);
        }

        /// <summary>
        /// Determines whether the specified type inherits from or implements the specified base type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="baseType">The base type to match.</param>
        /// <returns><c>true</c> if the specified type inherits from or implements the specified base type; otherwise, <c>false</c>.</returns>
        public static bool InheritOrImplement(this Type type, Type baseType)
        {
            type = ResolveGenericType(type);
            baseType = ResolveGenericType(baseType);
            while (type != typeof(object))
            {
                if (baseType == type || hasAnyIntefaces(type, baseType)) return true;
                type = ResolveGenericType(type.BaseType);
                if (type == null) return false;
            }

            return false;
        }

        
    }
}