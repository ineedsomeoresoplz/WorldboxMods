using NeoModLoader.api;
using UnityEngine;
using XaviiMagiaMod.Code.Data;
using XaviiMagiaMod.Code.Managers;

namespace XaviiMagiaMod.Code
{
    public class MagicMod : BasicMod<MagicMod>
    {
        protected override void OnModLoad()
        {
            MagiaConfig.Load();
            if (TryGetComponent(out MagicManager existing))
            {
                existing.enabled = true;
                return;
            }

            gameObject.AddComponent<MagicManager>();
        }
    }
}
