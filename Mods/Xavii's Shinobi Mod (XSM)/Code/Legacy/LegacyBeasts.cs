using System;
using UnityEngine;

namespace XSM.Legacy;

internal static class LegacyBeasts
{
    private static readonly BeastDef[] Beasts =
    {
        new("Shukaku", 250, 0.6f, "Baryon"),
        new("Matatabi", 300, 0.7f, "Baryon"),
        new("Isobu", 350, 0.8f, "Baryon"),
        new("Son Goku", 400, 0.9f, "Baryon"),
        new("Kokuo", 450, 1f, "Baryon"),
        new("Saiken", 500, 1.1f, "Baryon"),
        new("Chomei", 550, 1.2f, "Baryon"),
        new("Gyuki", 650, 1.3f, "Baryon"),
        new("Kurama", 800, 1.5f, "Baryon")
    };

    public static void Init()
    {
        foreach (var beast in Beasts)
            Register(beast);
    }

    private static void Register(BeastDef beast)
    {
        if (AssetManager.traits.get(beast.Id) != null)
            return;
        var trait = new ActorTrait
        {
            id = beast.Id,
            path_icon = $"ui/icons/{IconName(beast.Id)}",
            inherit = 0f,
            birth = 0f,
            group_id = LegacyTraitGroups.Beast,
            can_be_given = true,
            base_stats = new BaseStats()
        };
        trait.base_stats[S.health] = beast.Health;
        trait.base_stats[S.mod_damage] = beast.DamageBonus;
        trait.base_stats[S.attack_speed] = 40f;
        trait.base_stats[S.speed] = 20f;
        trait.action_special_effect = (WorldAction)Delegate.Combine(trait.action_special_effect, new WorldAction((p, t) =>
        {
            if (p?.a == null)
                return false;
            if (Toolbox.randomChance(0.1f))
                p.a.restoreHealth(10);
            if (Toolbox.randomChance(0.05f))
                p.a.addStatusEffect("invincible", 1f);
            return true;
        }));
        AssetManager.traits.add(trait);
        trait.unlock(true);
    }

    private static string IconName(string id)
    {
        return id switch
        {
            "Shukaku" => "Rampage",
            "Matatabi" => "Rampage1",
            "Isobu" => "Rampage2",
            "Son Goku" => "Rampage3",
            "Kokuo" => "Rampage4",
            "Saiken" => "Rampage5",
            "Chomei" => "Rampage6",
            "Gyuki" => "Rampage7",
            _ => "Rampage8"
        };
    }

    private class BeastDef
    {
        public BeastDef(string id, float health, float damageBonus, string aura)
        {
            Id = id;
            Health = health;
            DamageBonus = damageBonus;
            Aura = aura;
        }

        public string Id { get; }
        public float Health { get; }
        public float DamageBonus { get; }
        public string Aura { get; }
    }
}
