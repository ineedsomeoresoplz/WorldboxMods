// Decompiled with JetBrains decompiler
// Type: ClanManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ClanManager : MetaSystemManager<Clan, ClanData>
{
  public ClanManager() => this.type_id = "clan";

  public Clan newClan(Actor pFounder, bool pAddDefaultTraits)
  {
    ++World.world.game_stats.data.clansCreated;
    ++World.world.map_stats.clansCreated;
    Clan clan = this.newObject();
    clan.newClan(pFounder, pAddDefaultTraits);
    MetaHelper.addRandomTrait<ClanTrait>((ITraitsOwner<ClanTrait>) clan, (BaseTraitLibrary<ClanTrait>) AssetManager.clan_traits);
    pFounder.setClan(clan);
    if (pFounder.isKing())
      pFounder.kingdom.trySetRoyalClan();
    this.convertFamilyToClan(pFounder, clan);
    this.addRandomTraitFromBiomeToClan(clan, pFounder.current_tile);
    return clan;
  }

  private void convertFamilyToClan(Actor pFounder, Clan pNewClan)
  {
    if (!pFounder.hasFamily())
      return;
    foreach (Actor child in pFounder.getChildren())
    {
      if (!child.hasClan())
        child.setClan(pNewClan);
    }
  }

  public override void removeObject(Clan pClan)
  {
    foreach (Kingdom kingdom in World.world.kingdoms.list)
    {
      if (kingdom.data.royal_clan_id == pClan.getID() && pClan.getRenown() >= 10)
        kingdom.logRoyalClanLost(pClan);
    }
    ++World.world.game_stats.data.clansDestroyed;
    ++World.world.map_stats.clansDestroyed;
    base.removeObject(pClan);
  }

  public void addRandomTraitFromBiomeToClan(Clan pClan, WorldTile pTile)
  {
    pClan.addRandomTraitFromBiome<ClanTrait>(pTile, pTile.Type.biome_asset?.spawn_trait_clan, (AssetLibrary<ClanTrait>) AssetManager.clan_traits);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    foreach (Clan clan in this.list)
    {
      if (!clan.hasChief())
        clan.checkMembersForNewChief();
    }
  }

  protected override void updateDirtyUnits()
  {
    List<Actor> unitsOnlyAlive = World.world.units.units_only_alive;
    for (int index = 0; index < unitsOnlyAlive.Count; ++index)
    {
      Actor pActor = unitsOnlyAlive[index];
      Clan clan = pActor.clan;
      if (clan != null && clan.isDirtyUnits())
        clan.listUnit(pActor);
    }
  }
}
