using System;
using System.Collections.Generic;
using UnityEngine;

namespace XSM.Legacy;

internal static class LegacyDojutsu
{
    private static readonly System.Random Random = new();

    public static void Init()
    {
        RegisterClanTraits();
        RegisterDojutsuTraits();
    }

    private static void RegisterClanTraits()
    {
        AddTrait(new ActorTrait
        {
            id = "Uchiha",
            path_icon = "ui/icons/clans/uchiha",
            inherit = 100f,
            birth = 0f,
            group_id = LegacyTraitGroups.Clan,
            can_be_given = true,
            base_stats = new BaseStats { [S.health] = 150, [S.intelligence] = 10 }
        }, a =>
        {
            if (a.data.getAge() < 10)
                return false;
            if (HasAnySharingan(a))
                return false;
            if (Toolbox.randomChance(0.012f))
            {
                a.addTrait("1 Tomoe Sharingan");
                return true;
            }
            return false;
        });

        AddTrait(new ActorTrait
        {
            id = "Hyuga",
            path_icon = "ui/icons/clans/hyuga",
            inherit = 100f,
            birth = 0f,
            group_id = LegacyTraitGroups.Clan,
            can_be_given = true,
            base_stats = new BaseStats { [S.health] = 200 }
        }, a =>
        {
            if (a.data.getAge() < 10 || a.hasTrait("Byakugan"))
                return false;
            if (Toolbox.randomChance(0.006f))
            {
                a.addTrait("Byakugan");
                return true;
            }
            return false;
        });

        AddTrait(new ActorTrait
        {
            id = "Uzumaki",
            path_icon = "ui/icons/clans/Uzumaki",
            inherit = 100f,
            birth = 0f,
            group_id = LegacyTraitGroups.Clan,
            can_be_given = true,
            base_stats = new BaseStats { [S.health] = 400 }
        });

        AddTrait(new ActorTrait
        {
            id = "Senju",
            path_icon = "ui/icons/clans/Senju",
            inherit = 100f,
            birth = 0f,
            group_id = LegacyTraitGroups.Clan,
            can_be_given = true,
            base_stats = new BaseStats { [S.health] = 250, [S.intelligence] = 25f, [S.diplomacy] = 25f }
        });
    }

    private static void RegisterDojutsuTraits()
    {
        AddTrait(new ActorTrait
        {
            id = "1 Tomoe Sharingan",
            path_icon = "ui/icons/dojutsu/Sharingan1",
            inherit = 0f,
            birth = 0f,
            group_id = LegacyTraitGroups.Dojutsu,
            can_be_given = true,
            base_stats = new BaseStats { [S.mod_damage] = 0.1f, [S.attack_speed] = 5f }
        }, a =>
        {
            if (a.hasTrait("2 Tomoe Sharingan") || a.hasTrait("3 Tomoe Sharingan"))
                return false;
            if (Toolbox.randomChance(0.005f))
            {
                a.removeTrait("1 Tomoe Sharingan");
                a.addTrait("2 Tomoe Sharingan");
                return true;
            }
            return false;
        });

        AddTrait(new ActorTrait
        {
            id = "2 Tomoe Sharingan",
            path_icon = "ui/icons/dojutsu/Sharingan2",
            inherit = 0f,
            birth = 0f,
            group_id = LegacyTraitGroups.Dojutsu,
            can_be_given = true,
            base_stats = new BaseStats { [S.mod_damage] = 0.25f, [S.attack_speed] = 5f }
        }, a =>
        {
            if (a.hasTrait("3 Tomoe Sharingan"))
                return false;
            if (Toolbox.randomChance(0.003f))
            {
                a.removeTrait("2 Tomoe Sharingan");
                a.addTrait("3 Tomoe Sharingan");
                return true;
            }
            return false;
        });

        AddTrait(new ActorTrait
        {
            id = "3 Tomoe Sharingan",
            path_icon = "ui/icons/dojutsu/Sharingan3",
            inherit = 0f,
            birth = 0f,
            group_id = LegacyTraitGroups.Dojutsu,
            can_be_given = true,
            base_stats = new BaseStats { [S.mod_damage] = 0.3f, [S.attack_speed] = 10f }
        }, a =>
        {
            if (HasAnyMangekyo(a))
                return false;
            if (!Toolbox.randomChance(0.0008f))
                return false;
            var index = Random.Next(0, MangekyoIds.Length);
            a.addTrait(MangekyoIds[index]);
            return true;
        });

        foreach (var mangekyo in MangekyoIds)
        {
            AddTrait(new ActorTrait
            {
                id = mangekyo,
                path_icon = $"ui/icons/dojutsu/{MangekyoIcon(mangekyo)}",
                inherit = 0f,
                birth = 0f,
                group_id = LegacyTraitGroups.Dojutsu,
                can_be_given = true,
                base_stats = new BaseStats { [S.mod_damage] = 0.5f, [S.attack_speed] = 20f }
            }, a =>
            {
                if (!NeedsEternal(mangekyo))
                    return false;
                if (!Toolbox.randomChance(0.0002f))
                    return false;
                a.removeTrait(mangekyo);
                a.addTrait(EternalId(mangekyo));
                return true;
            });
        }

        foreach (var eternal in EternalMangekyoIds)
        {
            AddTrait(new ActorTrait
            {
                id = eternal,
                path_icon = $"ui/icons/dojutsu/{MangekyoIcon(eternal)}",
                inherit = 0f,
                birth = 0f,
                group_id = LegacyTraitGroups.Dojutsu,
                can_be_given = true,
                base_stats = new BaseStats { [S.mod_damage] = 1f, [S.attack_speed] = 40f }
            }, a =>
            {
                if (a.hasTrait("Rinnegan"))
                    return false;
                if (!Toolbox.randomChance(0.0001f))
                    return false;
                a.removeTrait(eternal);
                a.addTrait("Rinnegan");
                return true;
            });
        }

        AddTrait(new ActorTrait
        {
            id = "Byakugan",
            path_icon = "ui/icons/dojutsu/Byakugan",
            inherit = 0f,
            birth = 0f,
            group_id = LegacyTraitGroups.Dojutsu,
            can_be_given = true,
            base_stats = new BaseStats { [S.mod_damage] = 0.3f, [S.attack_speed] = 10f }
        }, a =>
        {
            if (a.hasTrait("Tenseigan"))
                return false;
            if (!Toolbox.randomChance(0.00001f))
                return false;
            a.removeTrait("Byakugan");
            a.addTrait("Tenseigan");
            return true;
        });

        AddTrait(new ActorTrait
        {
            id = "Tenseigan",
            path_icon = "ui/icons/dojutsu/Tenseigan",
            inherit = 0f,
            birth = 0f,
            group_id = LegacyTraitGroups.Dojutsu,
            can_be_given = true,
            base_stats = new BaseStats { [S.mod_damage] = 1.5f, [S.health] = 600, [S.attack_speed] = 60f }
        });

        AddTrait(new ActorTrait
        {
            id = "Rinnegan",
            path_icon = "ui/icons/dojutsu/Rinnegan",
            inherit = 0f,
            birth = 0f,
            group_id = LegacyTraitGroups.Dojutsu,
            can_be_given = true,
            base_stats = new BaseStats { [S.mod_damage] = 2f, [S.health] = 800, [S.attack_speed] = 80f }
        });
    }

    private static void AddTrait(ActorTrait trait, Func<Actor, bool>? specialEffect = null)
    {
        if (AssetManager.traits.get(trait.id) != null)
            return;
        if (specialEffect != null)
            trait.action_special_effect = (WorldAction)Delegate.Combine(trait.action_special_effect, new WorldAction((p, t) =>
            {
                if (p?.a == null)
                    return false;
                return specialEffect(p.a);
            }));
        AssetManager.traits.add(trait);
        trait.unlock(true);
    }

    private static bool HasAnySharingan(Actor a) => a.hasTrait("1 Tomoe Sharingan") || a.hasTrait("2 Tomoe Sharingan") || a.hasTrait("3 Tomoe Sharingan");

    private static bool HasAnyMangekyo(Actor a)
    {
        foreach (var id in MangekyoIds)
            if (a.hasTrait(id))
                return true;
        foreach (var id in EternalMangekyoIds)
            if (a.hasTrait(id))
                return true;
        return false;
    }

    private static bool NeedsEternal(string mangekyo) => mangekyo == "Madara's Mangekyo Sharingan" || mangekyo == "Sasuke's Mangekyo Sharingan";

    private static string EternalId(string mangekyo) => mangekyo.StartsWith("Madara") ? "Madara's Eternal Mangekyo Sharingan" : "Sasuke's Eternal Mangekyo Sharingan";

    private static string MangekyoIcon(string id)
    {
        if (id.StartsWith("Shin"))
            return "Mangekyo1";
        if (id.StartsWith("Izuna"))
            return "Mangekyo2";
        if (id.StartsWith("Shisui"))
            return "Mangekyo3";
        if (id.StartsWith("Itachi"))
            return "Mangekyo4";
        if (id.StartsWith("Obito"))
            return "Mangekyo5";
        if (id.StartsWith("Indra"))
            return "Mangekyo6";
        if (id.StartsWith("Madara"))
            return "Mangekyo7";
        if (id.StartsWith("Sasuke"))
            return "Mangekyo8";
        if (id.StartsWith("Madara's Eternal"))
            return "EternalMangekyo1";
        if (id.StartsWith("Sasuke's Eternal"))
            return "EternalMangekyo2";
        return "Mangekyo1";
    }

    private static readonly string[] MangekyoIds =
    {
        "Shin's Mangekyo Sharingan",
        "Izuna's Mangekyo Sharingan",
        "Shisui's Mangekyo Sharingan",
        "Itachi's Mangekyo Sharingan",
        "Obito's Mangekyo Sharingan",
        "Indra's Mangekyo Sharingan",
        "Madara's Mangekyo Sharingan",
        "Sasuke's Mangekyo Sharingan"
    };

    private static readonly string[] EternalMangekyoIds =
    {
        "Madara's Eternal Mangekyo Sharingan",
        "Sasuke's Eternal Mangekyo Sharingan"
    };
}
