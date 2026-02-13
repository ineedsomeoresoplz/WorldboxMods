using HarmonyLib;
using UnityEngine.UI;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(WorldLogMessageExtensions), nameof(WorldLogMessageExtensions.getFormatedText))]
    public static class WorldLogMessageFormatPatch
    {
        private static bool Prefix(WorldLogMessage pMessage, Text pTextField, ref string __result)
        {
            NationTypeManager.RegisterTraits();
            WorldLogAsset asset = pMessage.getAsset();
            if (asset == null)
                return true;

            string localeId = NationTypeLogHelper.ResolveLocaleId(pMessage, asset);
            string text = LocalizedTextManager.getText(localeId);
            asset.text_replacer?.Invoke(pMessage, ref text);
            __result = NationTypeLogHelper.ReplaceTokens(pMessage, text);
            if (pTextField != null)
                ((Graphic)pTextField).color = asset.color;
            return false;
        }
    }
}
