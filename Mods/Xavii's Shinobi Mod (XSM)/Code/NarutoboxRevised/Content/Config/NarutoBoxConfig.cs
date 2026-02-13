using Narutobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NarutoboxRevised.Content.Config
{
    public class NarutoBoxConfig
    {
        private int NoChance = 0;
        private int Rare = 0;
        private int LowChance = 15;
        private int MediumChance = 30;
        private int ExtraChance = 45;
        private int HighChance = 75;

        public static bool EnableClanFamilyName { get; set; } = false;
        public static bool EnableAutoFavorite { get; set; } = false;
        public static bool EnableForceRename { get; set; } = false;
        public static bool UnlockLegendTraits { get; set; } = false;
        public static bool EnableLegendaryAscension { get; set; } = true;


        //// This method will be called when config value set. ATTENTION: It might be called when game start.
        public static void ClanNameEnableSwitchConfigCallBack(bool pCurrentValue)
        {
            EnableClanFamilyName = pCurrentValue;
            Debug.Log($"Set enable clan family name to '{EnableClanFamilyName}'");
        }

        public static void UnlockLegendTraitsSwitchConfigCallBack(bool pCurrentValue)
        {
            UnlockLegendTraits = pCurrentValue;
            Debug.Log($"Set unlock legend traits to '{UnlockLegendTraits}'");
        }
        public static void AutoFavoriteEnableSwitchConfigCallBack(bool pCurrentValue)
        {
            EnableAutoFavorite = pCurrentValue;
            Debug.Log($"Set Enable Auto Favorite unit to '{EnableAutoFavorite}'");
        }
        public static void ForceRenameSwitchConfigCallBack(bool pCurrentValue)
        {
            EnableForceRename = pCurrentValue;
            Debug.Log($"Set force rename to '{EnableForceRename}'");
        }

        public static void LegendaryAscensionSwitchConfigCallBack(bool pCurrentValue)
        {
            EnableLegendaryAscension = pCurrentValue;
            Debug.Log($"Set legendary ascension to '{EnableLegendaryAscension}'");
        }

    }
}
