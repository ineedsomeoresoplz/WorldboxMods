using NeoModLoader.api;

namespace XaviiPixelArtMod
{
    public class XPAMMod : BasicMod<XPAMMod>
    {
        protected override void OnModLoad()
        {
            if (!TryGetComponent<PixelArtService>(out _))
            {
                gameObject.AddComponent<PixelArtService>();
            }
        }
    }
}