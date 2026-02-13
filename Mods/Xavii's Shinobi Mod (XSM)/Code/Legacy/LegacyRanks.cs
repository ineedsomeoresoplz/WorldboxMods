using System;
using UnityEngine;

namespace XSM.Legacy;

internal static class LegacyRanks
{
    public static void Init()
    {
        Register("Rank Academy Student", "ui/icons/RankStudent", 0, 0, RankEvoStudent, 0);
        Register("Rank Genin", "ui/icons/RankGenin", 0, 0, RankEvoGenin, 3);
        Register("Rank Chunin", "ui/icons/RankChunin", 0, 0, RankEvoChunin, 8);
        Register("Rank Jonin", "ui/icons/RankJonin", 0, 0, RankEvoJonin, 15);
        Register("Anbu", "ui/icons/AnbuNinja", 3f, 10f, RankEvoAnbu, 40, 10, 50f);
        Register("Anbu Captain", "ui/icons/AnbuCaptain", 0f, 1f, RankEvoAnbuCaptain, 60, 20, 80f);
        Register("Kage", "ui/icons/Kage", 0f, 0f, RankMaintainKage, 80, 30, 120f);
        Register("Former-Kage", "ui/icons/FormerK", 0f, 0f, null, 50, 30, 110f);
    }

    private static void Register(string id, string icon, float inherit, float birth, Func<Actor, bool>? tick, float damage = 0, float intellect = 0, float speed = 0)
    {
        if (AssetManager.traits.get(id) != null)
            return;
        var trait = new ActorTrait
        {
            id = id,
            path_icon = icon,
            inherit = inherit,
            birth = birth,
            can_be_given = true,
            group_id = LegacyTraitGroups.Rank,
            base_stats = new BaseStats()
        };
        trait.base_stats[S.damage] += damage;
        trait.base_stats[S.intelligence] += intellect;
        trait.base_stats[S.speed] += speed;
        if (tick != null)
            trait.action_special_effect = (WorldAction)Delegate.Combine(trait.action_special_effect, new WorldAction((p, t) =>
            {
                if (p?.a == null)
                    return false;
                return tick(p.a);
            }));
        AssetManager.traits.add(trait);
        trait.unlock(true);
    }

    private static bool RankEvoStudent(Actor a)
    {
        if (a.data.level >= 2)
        {
            a.removeTrait("Rank Academy Student");
            a.addTrait("Rank Genin");
            return true;
        }
        return false;
    }

    private static bool RankEvoGenin(Actor a)
    {
        if (a.stats[S.kills] >= 5)
        {
            a.removeTrait("Rank Genin");
            a.addTrait("Rank Chunin");
            return true;
        }
        return false;
    }

    private static bool RankEvoChunin(Actor a)
    {
        if (a.stats[S.kills] >= 15)
        {
            a.removeTrait("Rank Chunin");
            a.addTrait("Rank Jonin");
            return true;
        }
        return false;
    }

    private static bool RankEvoJonin(Actor a)
    {
        if (a.stats[S.kills] >= 35)
        {
            a.removeTrait("Rank Jonin");
            a.addTrait("Anbu");
            return true;
        }
        return false;
    }

    private static bool RankEvoAnbu(Actor a)
    {
        if (a.stats[S.kills] >= 60)
        {
            a.removeTrait("Anbu");
            a.addTrait("Anbu Captain");
            return true;
        }
        return false;
    }

    private static bool RankEvoAnbuCaptain(Actor a)
    {
        if (a.stats[S.kills] >= 100 && a.data.level >= 10)
        {
            a.removeTrait("Anbu Captain");
            a.addTrait("Kage");
            return true;
        }
        return false;
    }

    private static bool RankMaintainKage(Actor a)
    {
        if (a.data.getAge() > 70)
        {
            a.removeTrait("Kage");
            a.addTrait("Former-Kage");
            return true;
        }
        return false;
    }
}
