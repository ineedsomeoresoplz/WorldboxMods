using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(StatsWindow), "tryToShowActor", new[] { typeof(string), typeof(long), typeof(string), typeof(Actor), typeof(string) })]
    public static class StatsWindowTryToShowActorCouncilPatch
    {
        private static readonly MethodInfo ShowStatRowMethod = AccessTools.Method(
            typeof(StatsWindow),
            "showStatRow",
            new[]
            {
                typeof(string),
                typeof(object),
                typeof(string),
                typeof(MetaType),
                typeof(long),
                typeof(bool),
                typeof(string),
                typeof(string),
                typeof(TooltipDataGetter),
                typeof(bool)
            });

        private static bool Prefix(StatsWindow __instance, string pTitle, long pID, string pName, Actor pObject, string pIconPath)
        {
            if (!string.Equals(pTitle, "king", StringComparison.Ordinal))
                return true;
            if (!(__instance is KingdomWindow))
                return true;
            if (ShowStatRowMethod == null)
                return true;

            Kingdom kingdom = SelectedMetas.selected_kingdom;
            if (kingdom == null || !CouncilManager.IsCouncilNation(kingdom))
                return true;

            List<Actor> councilors = CouncilManager.GetCouncilorsBySeat(kingdom);
            int slotCount = Mathf.Max(CouncilManager.GetCouncilSlotCount(kingdom), councilors.Count);
            if (slotCount <= 0)
                slotCount = 1;

            string color = kingdom.getColor()?.color_text;
            for (int i = 0; i < slotCount; i++)
            {
                Actor councilor = i < councilors.Count ? councilors[i] : null;
                string rowId = $"Councilor #{i + 1}";
                object rowValue = BuildCouncilorRowValue(councilor, color);
                MetaType metaType = MetaType.None;
                long metaId = -1L;

                if (councilor != null && !councilor.isRekt())
                {
                    metaType = MetaType.Unit;
                    metaId = councilor.getID();
                }

                ShowStatRowMethod.Invoke(
                    __instance,
                    new object[] { rowId, rowValue, color, metaType, metaId, true, pIconPath, null, null, false });
            }

            return false;
        }

        private static string BuildCouncilorRowValue(Actor councilor, string fallbackColor)
        {
            if (councilor == null || councilor.isRekt())
                return "???";

            string color = councilor.kingdom?.getColor()?.color_text ?? fallbackColor;
            return councilor.getName() + Toolbox.coloredGreyPart((object)councilor.getAge(), color, true);
        }
    }
}
