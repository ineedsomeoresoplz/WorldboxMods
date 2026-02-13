using NeoModLoader;
using NeoModLoader.api;
using NeoModLoader.General;
using NeoModLoader.services;
using HarmonyLib;

namespace NoRaceRestrictions;

class Main : BasicMod<Main>
{
    private Harmony _harmony;

    protected override void OnModLoad()
    {
        LogService.LogInfo("Мод загружен \nПОН");
        WorldLaws.Init();
        _harmony = new Harmony("NoRaceRestrictions.Patch");
        _harmony.PatchAll(typeof(Main).Assembly);
    }
}