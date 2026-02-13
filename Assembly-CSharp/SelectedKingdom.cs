// Decompiled with JetBrains decompiler
// Type: SelectedKingdom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedKingdom : SelectedMetaWithUnit<Kingdom, KingdomData>
{
  [SerializeField]
  private KingdomSelectedAlliesContainer _allies_container;
  [SerializeField]
  private KingdomSelectedWarsContainer _wars_container;

  protected override MetaType meta_type => MetaType.Kingdom;

  public override string unit_title_locale_key => "titled_king";

  public override bool hasUnit() => !this.nano_object.king.isRekt();

  public override Actor getUnit() => this.nano_object.king;

  protected override string getPowerTabAssetID() => "selected_kingdom";

  protected override void updateElementsOnChange(Kingdom pNano)
  {
    base.updateElementsOnChange(pNano);
    this._allies_container.update(pNano);
    this._wars_container.update(pNano);
  }

  protected override void showStatsGeneral(Kingdom pKingdom)
  {
    base.showStatsGeneral(pKingdom);
    if (pKingdom.countCities() > pKingdom.getMaxCities())
      this.setIconValue("i_cities", (float) pKingdom.countCities(), new float?((float) pKingdom.getMaxCities()), "#FB2C21");
    else
      this.setIconValue("i_cities", (float) pKingdom.countCities(), new float?((float) pKingdom.getMaxCities()));
    this.setIconValue("i_food", (float) pKingdom.countTotalFood());
    this.setIconValue("i_money", (float) pKingdom.countTotalMoney());
    this.setIconValue("i_buildings", (float) pKingdom.countBuildings());
    this.setIconValue("i_army", (float) pKingdom.countTotalWarriors(), new float?((float) pKingdom.countWarriorsMax()));
    this.setIconValue("i_territory", (float) pKingdom.countZones());
  }

  public void openVillagesTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Villages");
  }
}
