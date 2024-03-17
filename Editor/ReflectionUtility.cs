using System;
using System.Collections;
using System.Reflection;

namespace ShalicoUtils.Editor
{
    public static class ReflectionUtility
    {
        public const BindingFlags FindAllBindingFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
            BindingFlags.Static;

        public static bool TryFindFieldValue(object source, string name, out object value,
            BindingFlags bindingFlags = FindAllBindingFlags, bool includeAllBaseTypes = true)
        {
            value = null;
            if (source == null) return false;
            var type = source.GetType();

            while (type != null && includeAllBaseTypes)
            {
                var field = type.GetField(name, bindingFlags);
                if (field != null)
                {
                    value = field.GetValue(source);
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        public static bool TryFindFieldValueWithIndex(object source, string name, int index, out object value,
            BindingFlags bindingFlags = FindAllBindingFlags, bool includeAllBaseTypes = true)
        {
            if (TryFindFieldValue(source, name, out var enumerableObject, bindingFlags, includeAllBaseTypes))
                throw new ArgumentException(name + " is not a field of " + source.GetType(), nameof(name));

            if (enumerableObject == null)
            {
                value = null;
                return false;
            }

            var enumerable = enumerableObject as IEnumerable;
            if (enumerable == null)
            {
                value = null;
                return false;
            }

            var enumerator = enumerable.GetEnumerator();
            for (var i = 0; i <= index; i++)
                if (!enumerator.MoveNext())
                {
                    value = null;
                    return false;
                }

            value = enumerator.Current;
            return true;
        }

        public static bool TryFindFieldInfo(object source, string name, out FieldInfo field,
            BindingFlags bindingFlags = FindAllBindingFlags, bool includeAllBaseTypes = true)
        {
            field = null;
            if (source == null) return false;
            var sourceType = source.GetType();

            while (sourceType != null && includeAllBaseTypes)
            {
                field = sourceType.GetField(name, bindingFlags);
                if (field != null) return true;

                sourceType = sourceType.BaseType;
            }

            return false;
        }

        public static bool TryFindFieldOrPropertyValue(object source, string name, out object value,
            BindingFlags bindingFlags = FindAllBindingFlags, bool includeAllBaseTypes = true)
        {
            value = null;
            if (source == null) return false;
            var type = source.GetType();

            while (type != null && includeAllBaseTypes)
            {
                var field = type.GetField(name, bindingFlags);
                if (field != null)
                {
                    value = field.GetValue(source);
                    return true;
                }

                var property = type.GetProperty(name, bindingFlags);
                if (property != null)
                {
                    value = property.GetValue(source);
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        public static Type GetFieldElementType(FieldInfo info)
        {
            if (info.FieldType.IsArray)
                return info.FieldType.GetElementType();

            if (info.FieldType.IsGenericType)
                return info.FieldType.GetGenericArguments()[0];

            return info.FieldType;
        }
    }
}