using HarmonyLib;
using ai.behaviours;
using XNTM.Code.Data;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    [HarmonyPatch(typeof(KingdomBehCheckKing), nameof(KingdomBehCheckKing.execute))]
    public static class KingdomBehCheckKingPatch
    {
        private static bool Prefix(Kingdom pKingdom, ref BehResult __result)
        {
            NationTypeManager.RegisterTraits();
            NationTypeDefinition def = NationTypeManager.GetDefinition(pKingdom);

            if (def.SuccessionMode == NationSuccessionMode.None)
            {
                if (pKingdom?.hasKing() == true)
                    pKingdom.kingLeftEvent();
                __result = BehResult.Continue;
                return false;
            }

            if (def.SuccessionMode == NationSuccessionMode.Council)
            {
                CouncilManager.TickCouncil(pKingdom);
                __result = BehResult.Continue;
                return false;
            }

            return true;
        }
    }
}
