using System;
using UnityEngine;

namespace Platformer.Extension
{
    public class TypeFilterAttribute : PropertyAttribute
    {
        public Func<Type, bool> Filter { get; }

        public TypeFilterAttribute(Type filerType)
        {
            //Concrete class that inherits or implements the filerType
            Filter = type =>!type.IsAbstract &&
                            !type.IsInterface &&
                            !type.IsGenericType &&
                            type.InheritOrImplement(filerType);
        }
    }
}