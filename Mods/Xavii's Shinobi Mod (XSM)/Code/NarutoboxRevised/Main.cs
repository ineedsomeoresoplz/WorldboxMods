using NarutoboxRevised.Content.Effects;
using NarutoboxRevised.Content.Items;
using NarutoboxRevised.Content.StatusEffects;
using NarutoboxRevised.Content.Traits;

namespace Narutobox;

public static class NarutoBoxModule
{
    public const string Identifier = "darkie";

    public static void Init()
    {
        Config.isEditor = true;
        CustomTraits.Init();
        CustomItems.Init();
        CustomEffects.Init();
        CustomStatusEffects.Init();
    }

    public static void Reload()
    {
        CustomTraits.Init();
        CustomStatusEffects.Init();
        CustomEffects.Init();
    }
}
