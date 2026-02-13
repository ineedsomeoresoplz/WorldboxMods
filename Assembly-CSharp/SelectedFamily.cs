// Decompiled with JetBrains decompiler
// Type: SelectedFamily
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class SelectedFamily : SelectedMetaWithUnit<Family, FamilyData>
{
  protected override MetaType meta_type => MetaType.Family;

  public override string unit_title_locale_key => "titled_alpha";

  public override bool hasUnit() => !this.nano_object.getAlpha().isRekt();

  public override Actor getUnit() => this.nano_object.getAlpha();

  protected override string getPowerTabAssetID() => "selected_family";

  public void openPeopleTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("People");
  }
}
