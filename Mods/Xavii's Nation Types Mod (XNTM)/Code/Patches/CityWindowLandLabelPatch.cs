using HarmonyLib;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(CityWindow), "showStatsRows")]
    public static class CityWindowLandLabelPatch
    {
        private static void Postfix(CityWindow __instance)
        {
            City city = Traverse.Create(__instance).Property("meta_object").GetValue<City>();
            if (city == null)
                return;

            var land = LandTypeManager.EnsureLandType(city);
            string landName = land?.GetLocalizedName();
            if (string.IsNullOrEmpty(landName))
                return;

            var label = __instance.village_title;
            if (label == null)
                return;

            var textField = Traverse.Create(label).Field("text").GetValue<UnityEngine.UI.Text>();
            if (textField != null)
                textField.text = landName;
        }
    }
}
