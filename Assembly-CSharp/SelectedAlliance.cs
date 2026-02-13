// Decompiled with JetBrains decompiler
// Type: SelectedAlliance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedAlliance : SelectedMeta<Alliance, AllianceData>
{
  [SerializeField]
  private AllianceSelectedKingdomsContainer _kingdoms_container;

  protected override MetaType meta_type => MetaType.Alliance;

  protected override string getPowerTabAssetID() => "selected_alliance";

  protected override void updateElementsOnChange(Alliance pNano)
  {
    base.updateElementsOnChange(pNano);
    this._kingdoms_container.update(pNano);
  }

  protected override void showStatsGeneral(Alliance pAlliance)
  {
    base.showStatsGeneral(pAlliance);
    this.setIconValue("i_army", (float) pAlliance.countWarriors());
    this.setIconValue("i_kingdoms", (float) pAlliance.countKingdoms());
    this.setIconValue("i_zones", (float) pAlliance.countZones());
    this.setIconValue("i_cities", (float) pAlliance.countCities());
    this.setIconValue("i_money", (float) pAlliance.countTotalMoney());
    this.setIconValue("i_buildings", (float) pAlliance.countBuildings());
    this.setIconValue("i_territory", (float) pAlliance.countZones());
  }

  protected override void setTitleIcons(Alliance pAlliance)
  {
  }
}
