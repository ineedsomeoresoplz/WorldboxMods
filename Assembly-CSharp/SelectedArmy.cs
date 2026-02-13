// Decompiled with JetBrains decompiler
// Type: SelectedArmy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class SelectedArmy : SelectedMetaWithUnit<Army, ArmyData>
{
  protected override MetaType meta_type => MetaType.Army;

  public override string unit_title_locale_key => "titled_captain";

  public override bool hasUnit() => !this.nano_object.getCaptain().isRekt();

  public override Actor getUnit() => this.nano_object.getCaptain();

  protected override string getPowerTabAssetID() => "selected_army";

  protected override void showStatsGeneral(Army pArmy)
  {
    base.showStatsGeneral(pArmy);
    this.setIconValue("i_army_size", (float) pArmy.countUnits());
    this.setIconValue("i_money", (float) pArmy.countTotalMoney());
    this.setIconValue("i_melee", (float) pArmy.countMelee());
    this.setIconValue("i_range", (float) pArmy.countRange());
  }
}
