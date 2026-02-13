using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace AIBox.Patches
{
    public static class ModerBoxPatches
    {
        public static bool Prefix_BlockAutoAction()
        {
            if (ModerBoxHelper.AIBlocksActions)
            {
                return false;
            }
            return true;
        }
    }
}
