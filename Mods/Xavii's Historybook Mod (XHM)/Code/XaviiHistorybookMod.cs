using NeoModLoader.api;
using UnityEngine;
using XaviiHistorybookMod.Code.Managers;

namespace XaviiHistorybookMod.Code
{
    public class XaviiHistorybookMod : BasicMod<XaviiHistorybookMod>
    {
        protected override void OnModLoad()
        {
            // Ensure there is only a single history manager running.
            if (TryGetComponent(out HistorybookManager existingManager))
            {
                existingManager.enabled = true;
                return;
            }

            gameObject.AddComponent<HistorybookManager>();
        }
    }
}
