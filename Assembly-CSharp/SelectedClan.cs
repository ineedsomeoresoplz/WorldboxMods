// Decompiled with JetBrains decompiler
// Type: SelectedClan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedClan : SelectedMetaWithUnit<Clan, ClanData>
{
  [SerializeField]
  private CitiesKingdomsContainersController _banners_cities_kingdoms;

  protected override MetaType meta_type => MetaType.Clan;

  public override string unit_title_locale_key => "titled_chief";

  public override bool hasUnit() => !this.nano_object.getChief().isRekt();

  public override Actor getUnit() => this.nano_object.getChief();

  protected override string getPowerTabAssetID() => "selected_clan";

  protected override void showStatsGeneral(Clan pClan)
  {
    base.showStatsGeneral(pClan);
    this.setIconValue("i_books_written", (float) pClan.data.books_written);
  }

  public void openPeopleTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("People");
  }

  protected override void updateElementsOnChange(Clan pNano)
  {
    base.updateElementsOnChange(pNano);
    this._banners_cities_kingdoms.update((NanoObject) pNano);
  }
}
