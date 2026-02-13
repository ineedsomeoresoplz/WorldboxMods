using HarmonyLib;
using UnityEngine;
using XNTM.Code.Utils;

namespace XNTM.Code.Patches
{
    
    
    [HarmonyPatch(typeof(CommunicationTopicLibrary), nameof(CommunicationTopicLibrary.getTopicSprite))]
    public static class CommunicationTopicSpritePatch
    {
        private static void Postfix(ref Sprite __result)
        {
            __result = TopicSpriteScaler.ScaleIfNationType(__result);
        }
    }
}
