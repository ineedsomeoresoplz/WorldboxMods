using HarmonyLib;
using XNTM.Code.Utils;
using XNTM.Code.Data;
using System.Collections.Generic;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(WorldLogMessageExtensions), nameof(WorldLogMessageExtensions.add))]
    public static class WorldLogMessageAddPatch
    {
        private static readonly HashSet<string> RulerLogIds = new HashSet<string>
        {
            "king_new",
            "king_left",
            "king_fled_city",
            "king_fled_capital",
            "king_dead",
            "king_killed",
            "kingdom_royal_clan_new",
            "kingdom_royal_clan_changed",
            "kingdom_royal_clan_dead"
        };

        private static bool Prefix(WorldLogMessage pMessage)
        {
            NationTypeManager.RegisterTraits();
            if (ShouldSuppress(pMessage))
                return false;
            WorldLogMetadataHelper.AttachNationMetadata(pMessage);
            return true;
        }

        private static bool ShouldSuppress(WorldLogMessage message)
        {
            if (message == null)
                return false;

            Kingdom kingdom = message.kingdom ?? message.unit?.kingdom;
            NationTypeDefinition def = NationTypeManager.GetDefinition(kingdom);
            if (def == null)
                return false;

            if (def.SuccessionMode == NationSuccessionMode.None && RulerLogIds.Contains(message.asset_id))
                return true;

            if (def.SuccessionMode == NationSuccessionMode.Council && RulerLogIds.Contains(message.asset_id))
                return true;

            return false;
        }
    }

    [HarmonyPatch(typeof(WorldLogMessageExtensions), nameof(WorldLogMessageExtensions.getSpecial))]
    public static class WorldLogMessageGetSpecialPatch
    {
        private static void Postfix(int pSpecialId, ref string __result)
        {
            if (pSpecialId != 3 || string.IsNullOrEmpty(__result))
                return;

            __result = WorldLogMetadataHelper.StripMetadataFromFormatted(__result);
        }
    }
}
