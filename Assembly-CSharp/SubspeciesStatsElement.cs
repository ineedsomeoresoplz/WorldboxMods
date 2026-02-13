// Decompiled with JetBrains decompiler
// Type: SubspeciesStatsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class SubspeciesStatsElement : SubspeciesElement, IStatsElement, IRefreshElement
{
  private StatsIconContainer _stats_icons;

  public void setIconValue(
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/')
  {
    this._stats_icons.setIconValue(pName, pMainVal, pMax, pColor, pFloat, pEnding, pSeparator);
  }

  protected override void Awake()
  {
    this._stats_icons = ((Component) this).gameObject.AddOrGetComponent<StatsIconContainer>();
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    SubspeciesStatsElement subspeciesStatsElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (subspeciesStatsElement.subspecies == null || !subspeciesStatsElement.subspecies.isAlive())
      return false;
    subspeciesStatsElement._stats_icons.showGeneralIcons<Subspecies, SubspeciesData>(subspeciesStatsElement.subspecies);
    // ISSUE: explicit non-virtual call
    __nonvirtual (subspeciesStatsElement.setIconValue("i_offspring", subspeciesStatsElement.subspecies.base_stats["offspring"], new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (subspeciesStatsElement.setIconValue("i_mutation_rate", subspeciesStatsElement.subspecies.base_stats_meta["mutation"], new float?(), "", false, "", '/'));
    double baseStat = (double) subspeciesStatsElement.subspecies.base_stats["lifespan"];
    float pMainVal1 = (float) baseStat + subspeciesStatsElement.subspecies.base_stats_male["lifespan"];
    float pMainVal2 = (float) baseStat + subspeciesStatsElement.subspecies.base_stats_female["lifespan"];
    int pMainVal3 = (int) ((double) (int) subspeciesStatsElement.subspecies.base_stats["intelligence"] * (double) SimGlobals.m.MANA_PER_INTELLIGENCE);
    // ISSUE: explicit non-virtual call
    __nonvirtual (subspeciesStatsElement.setIconValue("i_lifespan_male", pMainVal1, new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (subspeciesStatsElement.setIconValue("i_lifespan_female", pMainVal2, new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (subspeciesStatsElement.setIconValue("i_maturation", subspeciesStatsElement.subspecies.getMaturationTimeMonths(), new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (subspeciesStatsElement.setIconValue("i_mana", (float) pMainVal3, new float?(), "", false, "", '/'));
    subspeciesStatsElement.showIconSubspecies("i_birth_rate", "birth_rate");
    subspeciesStatsElement.showIconSubspecies("i_health", "health");
    subspeciesStatsElement.showIconSubspecies("i_armor", "armor");
    subspeciesStatsElement.showIconSubspecies("i_speed", "speed");
    subspeciesStatsElement.showIconSubspecies("i_damage", "damage");
    subspeciesStatsElement.showIconSubspecies("i_critical_chance", "critical_chance");
    subspeciesStatsElement.showIconSubspecies("i_attack_speed", "attack_speed");
    subspeciesStatsElement.showIconSubspecies("i_diplomacy", "diplomacy");
    subspeciesStatsElement.showIconSubspecies("i_warfare", "warfare");
    subspeciesStatsElement.showIconSubspecies("i_stewardship", "stewardship");
    subspeciesStatsElement.showIconSubspecies("i_intelligence", "intelligence");
    subspeciesStatsElement.showIconSubspecies("i_stamina", "stamina");
    int pMainVal4 = subspeciesStatsElement.subspecies.countMainKingdoms();
    int pMainVal5 = subspeciesStatsElement.subspecies.countMainCities();
    // ISSUE: explicit non-virtual call
    __nonvirtual (subspeciesStatsElement.setIconValue("i_kingdoms", (float) pMainVal4, new float?(), "", false, "", '/'));
    // ISSUE: explicit non-virtual call
    __nonvirtual (subspeciesStatsElement.setIconValue("i_villages", (float) pMainVal5, new float?(), "", false, "", '/'));
    return false;
  }

  private void showIconSubspecies(string pFieldID, string pStatID)
  {
    ActorAsset actorAsset = this.subspecies.getActorAsset();
    int num = (int) this.subspecies.nucleus.getStats().get(pStatID);
    int baseStat1 = (int) actorAsset.base_stats[pStatID];
    foreach (GenomePart genomePart in actorAsset.genome_parts)
    {
      if (genomePart.id == pStatID)
      {
        baseStat1 += (int) genomePart.value;
        break;
      }
    }
    string str1 = num <= baseStat1 ? (num >= baseStat1 ? string.Empty : "#FB2C21") : "#43FF43";
    string pName = pFieldID;
    double baseStat2 = (double) this.subspecies.base_stats[pStatID];
    string str2 = str1;
    float? pMax = new float?();
    string pColor = str2;
    this.setIconValue(pName, (float) baseStat2, pMax, pColor, false, "", '/');
  }

  GameObject IStatsElement.get_gameObject() => ((Component) this).gameObject;
}
