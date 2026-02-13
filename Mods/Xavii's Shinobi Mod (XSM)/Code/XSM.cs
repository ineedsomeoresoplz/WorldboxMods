using NeoModLoader.api;
using NeoModLoader.General;
using Narutobox;
using XSM.Legacy;

namespace XSM;

public class XSMMod : BasicMod<XSMMod>
{
    protected override void OnModLoad()
    {
        NarutoBoxModule.Init();
        LegacyTraitGroups.Init();
        LegacyDojutsu.Init();
        LegacyRanks.Init();
        LegacyBeasts.Init();
    }
}
