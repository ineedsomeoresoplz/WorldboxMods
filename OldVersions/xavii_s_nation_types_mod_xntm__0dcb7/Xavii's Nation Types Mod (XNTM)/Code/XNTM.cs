using NeoModLoader.api;
using XNTM.Code.Features;
using XNTM.Code.Utils;

namespace XNTM.Code
{
    public class XNTM : BasicMod<XNTM>
    {
        protected override void OnModLoad()
        {
            Utils.NationTypeManager.RegisterTraits();
            Utils.PlotSafetyFix.Apply();
            NationTypeTask.Register();
            NationTypePlot.Register();
            LocalizationPrewarmer.Register();
            LocalizationPrewarmer.Prewarm();
        }
    }
}
