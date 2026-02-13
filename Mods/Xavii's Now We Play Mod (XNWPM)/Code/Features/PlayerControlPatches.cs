using HarmonyLib;
using UnityEngine;

namespace XaviiNowWePlayMod.Code.Features
{
    [HarmonyPatch(typeof(PlayerControl))]
    [HarmonyPatch("clickedFinal")]
    internal static class PlayerControlClickedFinalPatch
    {
        private static void Postfix(Vector2Int pPos, GodPower pPower, bool pTrack)
        {
            if (!pTrack)
            {
                return;
            }

            GodPower power = pPower ?? World.world.selected_buttons.selectedButton?.godPower;
            if (power == null)
            {
                return;
            }

            XNWPMManager.Instance?.ReportLocalCommand(pPos, power.id);
        }
    }
}
