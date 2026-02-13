using UnityEngine;

namespace XSM.Legacy;

internal static class LegacyTraitGroups
{
    public const string Clan = "legacy_clantraits";
    public const string Dojutsu = "legacy_dojutsu";
    public const string Rank = "legacy_ranks";
    public const string Beast = "legacy_beasts";

    public static void Init()
    {
        AddGroup(Clan, "trait_group_legacy_clantraits", "#7A97AB");
        AddGroup(Dojutsu, "trait_group_legacy_dojutsu", "#A50C0E");
        AddGroup(Rank, "trait_group_legacy_ranks", "#ffaa00");
        AddGroup(Beast, "trait_group_legacy_beasts", "#6c2bd1");
    }

    private static void AddGroup(string id, string name, string color)
    {
        if (AssetManager.trait_groups.get(id) != null)
            return;
        var group = new ActorTraitGroupAsset
        {
            id = id,
            name = name,
            color = color
        };
        AssetManager.trait_groups.add(group);
    }
}
