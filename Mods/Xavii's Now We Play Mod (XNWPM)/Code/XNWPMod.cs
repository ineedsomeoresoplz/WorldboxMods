using NeoModLoader.api;
using UnityEngine;
using XaviiNowWePlayMod.Code.Features;

namespace XaviiNowWePlayMod
{
    public class XNWPMMod : BasicMod<XNWPMMod>
    {
        private Code.XNWPMManager _manager;

        protected override void OnModLoad()
        {
            HarmonyPatches.Apply();

            if (TryGetComponent(out Code.XNWPMManager existing))
            {
                _manager = existing;
                return;
            }

            _manager = gameObject.AddComponent<Code.XNWPMManager>();
        }

        private void OnDestroy()
        {
            HarmonyPatches.Remove();

            if (_manager != null)
            {
                Destroy(_manager);
            }
        }
    }
}
