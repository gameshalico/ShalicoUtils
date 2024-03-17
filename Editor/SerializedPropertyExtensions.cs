using System;
using System.Collections.Generic;
using System.Reflection;
using ShalicoUtils.Editor;
using UnityEditor;

namespace Packages.ShalicoUtils.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static object GetPropertyValue(this SerializedProperty property)
        {
            return GetPropertyValue(property.serializedObject.targetObject, property.propertyPath);
        }

        public static void SetPropertyValue(this SerializedProperty property, object value)
        {
            SetPropertyValue(property.serializedObject.targetObject, property.propertyPath, value);
        }

        private static object GetPropertyValue(object source, string path)
        {
            var pathParts = Parse(path);
            var currentObject = source;

            foreach (var part in pathParts) currentObject = GetNestedObject(currentObject, part);

            return currentObject;
        }

        private static object SetPropertyValue(object source, string path, object value)
        {
            var pathParts = Parse(path);
            var currentObject = source;

            for (var i = 0; i < pathParts.Count - 1; i++)
            {
                var part = pathParts[i];
                currentObject = GetNestedObject(currentObject, part);
            }

            var lastPart = pathParts[^1];
            var fieldInfo = GetNestedFieldInfo(currentObject, lastPart);
            fieldInfo.SetValue(currentObject, value);

            return source;
        }


        private static List<PathPart> Parse(string path)
        {
            var pathParts = path.Split('.');
            var resultList = new List<PathPart>();

            for (var i = 0; i < pathParts.Length; i++)
            {
                var part = pathParts[i];
                if (part.Contains("["))
                {
                    var arrayPart = pathParts[i - 1];
                    var indexPart = part.Substring(part.IndexOf('[')).Replace("[", "").Replace("]", "");
                    var index = int.Parse(indexPart);

                    resultList.Add(new PathPart(arrayPart, index, true));
                }
                else
                {
                    resultList.Add(new PathPart(part, 0, false));
                }
            }

            return resultList;
        }

        private static object GetNestedObject(object currentObject, PathPart part)
        {
            if (part.IsArray)
            {
                if (ReflectionUtility.TryFindFieldValueWithIndex(currentObject, part.Name, part.Index, out var value))
                    return value;

                throw new ArgumentException(part.Name + " is not a field of " + currentObject.GetType(),
                    nameof(part.Name));
            }

            if (ReflectionUtility.TryFindFieldValue(currentObject, part.Name, out var nestedValue)) return nestedValue;

            throw new ArgumentException(part.Name + " is not a field of " + currentObject.GetType(), nameof(part.Name));
        }

        private static FieldInfo GetNestedFieldInfo(object currentObject, PathPart part)
        {
            if (ReflectionUtility.TryFindFieldInfo(currentObject, part.Name, out var fieldInfo)) return fieldInfo;
            throw new ArgumentException(part.Name + " is not a field of " + currentObject.GetType(), nameof(part.Name));
        }

        private readonly struct PathPart
        {
            public readonly string Name;
            public readonly int Index;
            public readonly bool IsArray;

            public PathPart(string name, int index, bool isArray)
            {
                Name = name;
                Index = index;
                IsArray = isArray;
            }
        }
    }
}