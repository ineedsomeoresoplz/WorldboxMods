using System;
using System.Reflection;
using UnityEngine;

namespace ReflectionUtility
{
    public static class Reflection
    {
        public static object GetField(Type type, object instance, string fieldName)
        {
            if (type == null && instance != null) type = instance.GetType();
            if (type == null) return null;
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null) return null;
            return field.GetValue(instance);
        }

        public static object GetField(object instance, string fieldName)
        {
            return GetField(null, instance, fieldName);
        }

        public static void SetField(Type type, object instance, string fieldName, object value)
        {
            if (type == null && instance != null) type = instance.GetType();
            if (type == null) return;
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(instance, value);
            }
        }

        public static void SetField(object instance, string fieldName, object value)
        {
            SetField(null, instance, fieldName, value);
        }
        
        public static object CallMethod(Type type, object instance, string methodName, params object[] args)
        {
            if (type == null && instance != null) type = instance.GetType();
            if (type == null) return null;
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if(method == null) return null;
            return method.Invoke(instance, args);
        }

        public static object CallMethod(object instance, string methodName, params object[] args)
        {
             return CallMethod(null, instance, methodName, args);
        }
    }
}
