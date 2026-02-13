using System;
using System.Collections.Generic;
using UnityEngine;
using NCMS;

namespace AIBox
{
    public class ResourcePatches
    {
        public static void init()
        {
            if (AssetManager.resources == null || AssetManager.resources.list == null) return;

            int patchedCount = 0;
            foreach (ResourceAsset res in AssetManager.resources.list)
            {
                if (res.maximum == 999)
                {
                    res.maximum = 999999;
                    patchedCount++;
                }
            }
            
            ResourceAsset gold = AssetManager.resources.get("gold");
            if(gold != null)
            {
                gold.maximum = 999999;
            }

            Debug.Log($"EconomyBox: Patched {patchedCount} resources to limit 999,999.");
        }
    }
}
