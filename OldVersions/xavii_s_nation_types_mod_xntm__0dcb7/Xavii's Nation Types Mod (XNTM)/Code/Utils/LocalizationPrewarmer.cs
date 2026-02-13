using System;

namespace XNTM.Code.Utils
{
    public static class LocalizationPrewarmer
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered)
                return;
            MapBox.on_world_loaded += Prewarm;
            _registered = true;
        }

        public static void Prewarm()
        {
            try
            {
                if (LocalizedTextManager.instance == null)
                    return;
                LocalizedTextManager.updateTexts();
            }
            catch
            {
            }
        }
    }
}
