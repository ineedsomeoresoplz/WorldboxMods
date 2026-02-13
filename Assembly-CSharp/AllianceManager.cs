// Decompiled with JetBrains decompiler
// Type: AllianceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AllianceManager : MetaSystemManager<Alliance, AllianceData>
{
  public Sprite[] _cached_banner_backgrounds;
  public Sprite[] _cached_banner_icons;
  private List<Alliance> _to_dissolve = new List<Alliance>();

  public AllianceManager() => this.type_id = "alliance";

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    foreach (Alliance alliance in (CoreSystemManager<Alliance, AllianceData>) this)
    {
      alliance.clearCursorOver();
      if (!alliance.checkActive())
        this._to_dissolve.Add(alliance);
      else
        alliance.update();
    }
    foreach (Alliance pAlliance in this._to_dissolve)
      this.dissolveAlliance(pAlliance);
    this._to_dissolve.Clear();
  }

  public void dissolveAlliance(Alliance pAlliance)
  {
    ++World.world.game_stats.data.alliancesDissolved;
    ++World.world.map_stats.alliancesDissolved;
    WorldLog.logAllianceDisolved(pAlliance);
    pAlliance.dissolve();
    this.removeObject(pAlliance);
  }

  private void addTest()
  {
  }

  public bool forceAlliance(Kingdom pKingdom1, Kingdom pKingdom2)
  {
    Alliance alliance = pKingdom1.getAlliance() ?? pKingdom2.getAlliance();
    bool flag = false;
    if (alliance == null)
    {
      alliance = this.newAlliance(pKingdom1, pKingdom2);
      flag = true;
    }
    else
    {
      alliance.join(pKingdom1, pForce: true);
      alliance.join(pKingdom2, pForce: true);
    }
    alliance.setType(AllianceType.Forced);
    return flag;
  }

  public void useDiscordPower(Alliance pAlliance, City pCity)
  {
    Kingdom kingdom = pCity.kingdom;
    pAlliance.leave(kingdom);
    EffectsLibrary.highlightKingdomZones(kingdom, Color.white);
    if (pAlliance.kingdoms_hashset.Count != 0)
      return;
    this.dissolveAlliance(pAlliance);
  }

  public Alliance newAlliance(Kingdom pKingdom, Kingdom pKingdom2)
  {
    ++World.world.game_stats.data.alliancesMade;
    ++World.world.map_stats.alliancesMade;
    Alliance pAlliance = this.newObject();
    pAlliance.createNewAlliance();
    pAlliance.addFounders(pKingdom, pKingdom2);
    WorldLog.logAllianceCreated(pAlliance);
    return pAlliance;
  }

  public Sprite[] getBackgroundsList()
  {
    if (this._cached_banner_backgrounds == null)
      this._cached_banner_backgrounds = SpriteTextureLoader.getSpriteList("alliances/backgrounds/");
    return this._cached_banner_backgrounds;
  }

  public Sprite[] getIconsList()
  {
    if (this._cached_banner_icons == null)
      this._cached_banner_icons = SpriteTextureLoader.getSpriteList("alliances/icons/");
    return this._cached_banner_icons;
  }

  public bool anyAlliances() => this.Count > 0;

  public override void clear() => base.clear();

  protected override void updateDirtyUnits()
  {
  }
}
