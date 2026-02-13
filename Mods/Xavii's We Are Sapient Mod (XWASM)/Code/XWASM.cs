using NeoModLoader.api;
using XWASM.Code.Content;
using XWASM.Code.Patches;

namespace XWASM.Code
{
    public class XWASM : BasicMod<XWASM>
    {
        protected override void OnModLoad()
        {
            TraitRegistry.Register();
            SapiencePatches.Apply();
        }
    }
}
