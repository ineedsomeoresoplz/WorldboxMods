using System;
using System.Reflection;
using HarmonyLib;

namespace XaviiHistorybookMod.Code.Compatibility
{
    internal static class XNTMCompatibility
    {
        private const string NationTypeManagerTypeName = "XNTM.Code.Utils.NationTypeManager";

        private static readonly Type NationTypeManagerType = AccessTools.TypeByName(NationTypeManagerTypeName);
        private static readonly MethodInfo GetDefinitionMethod = NationTypeManagerType != null
            ? AccessTools.Method(NationTypeManagerType, "GetDefinition", new[] { typeof(Kingdom) })
            : null;

        public static bool TryGetTitles(Kingdom kingdom, Actor actor, out string rulerTitle, out string nationTypeName)
        {
            rulerTitle = null;
            nationTypeName = null;

            if (kingdom == null || GetDefinitionMethod == null)
                return false;

            var definition = GetDefinitionMethod.Invoke(null, new object[] { kingdom });
            if (definition == null)
                return false;

            var defType = definition.GetType();
            rulerTitle = InvokeLocalized(defType, definition, "GetLocalizedRulerTitle", actor);
            nationTypeName = InvokeLocalized(defType, definition, "GetLocalizedName");

            return !string.IsNullOrEmpty(rulerTitle) || !string.IsNullOrEmpty(nationTypeName);
        }

        private static string InvokeLocalized(Type definitionType, object definition, string methodName, params object[] args)
        {
            if (definition == null)
                return null;

            var method = definitionType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            if (method == null)
                return null;

            try
            {
                var result = method.Invoke(definition, args ?? Array.Empty<object>());
                return result?.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
