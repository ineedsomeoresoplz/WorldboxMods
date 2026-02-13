using NeoModLoader.api;
using UnityEngine;

namespace XaviiWindowsMod
{
    public class XWMMod : BasicMod<XWMMod>
    {
        protected override void OnModLoad()
        {
            if (!TryGetComponent<WindowService>(out _))
            {
                gameObject.AddComponent<WindowService>();
            }
        }
    }
}
