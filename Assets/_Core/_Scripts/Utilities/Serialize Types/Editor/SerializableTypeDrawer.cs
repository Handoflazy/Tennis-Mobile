using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Platformer.Extension
{
    [CustomPropertyDrawer(typeof(SerializableType))]
    public class SerializableTypeDrawer: PropertyDrawer
    {
        private TypeFilterAttribute typeFiler;
        string[] typeNames, typeFullNames;

        void Initialize()
        {
            if (typeFullNames != null) return;
            typeFiler = (TypeFilterAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));
            var fulteredTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t=>typeFiler == null ? DefaultFiler(t):typeFiler.Filter(t))
                .ToArray();
            typeNames = fulteredTypes.Select(t => t.ReflectedType == null?t.Name: $"t.ReflectedType.Name + \".\" + t.Name").ToArray();
            typeFullNames = fulteredTypes.Select(t => t.AssemblyQualifiedName).ToArray();
        }

        static bool DefaultFiler(Type type)
        {
            return !type.IsAbstract && !type.IsInterface && !type.IsGenericType;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize();
            var type = property.FindPropertyRelative("assemblyQualifiedName");
            if(string.IsNullOrEmpty(type.stringValue))
            {
                type.stringValue = typeFullNames.First();
                property.serializedObject.ApplyModifiedProperties();
            }
            var currentIndex = Array.IndexOf(typeFullNames, type.stringValue);
            var selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, typeNames);
            if (selectedIndex > 0 && selectedIndex != currentIndex)
            {
                type.stringValue = typeFullNames[selectedIndex];
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}