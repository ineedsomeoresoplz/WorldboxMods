using NeoModLoader.General.UI.Tab;

namespace AIBox
{
    class AIBoxTab
    {
        public static PowersTab EconomyTab;

        // Initialize AIBoxTab
        public static void init()
        {
            EconomyTab = TabManager.CreateTab(
                "Tab_AIBox",
                "AIBox",
                "economy_tab_desc",
                Sprites.LoadSprite($"{Mod.Info.Path}/EmbededResources/UI/Icons/icon.png")
            );
        }
    }
}

