// Decompiled with JetBrains decompiler
// Type: SelectedCulture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedCulture : SelectedMeta<Culture, CultureData>
{
  [SerializeField]
  private CultureSelectedOnomasticsNames _onomastics_names;
  [SerializeField]
  private CitiesKingdomsContainersController _banners_cities_kingdoms;

  protected override MetaType meta_type => MetaType.Culture;

  protected override string getPowerTabAssetID() => "selected_culture";

  protected override void showStatsGeneral(Culture pCulture)
  {
    base.showStatsGeneral(pCulture);
    this.setIconValue("i_kingdoms", (float) pCulture.countKingdoms());
    this.setIconValue("i_cities", (float) pCulture.countCities());
    this.setIconValue("i_books", (float) pCulture.books.count());
  }

  protected override void updateElementsOnChange(Culture pNano)
  {
    base.updateElementsOnChange(pNano);
    this._onomastics_names.load(pNano);
    this._banners_cities_kingdoms.update((NanoObject) pNano);
  }

  protected override void updateElementsAlways(Culture pNano)
  {
    base.updateElementsAlways(pNano);
    this._onomastics_names.update();
  }

  public void openOnomasticsTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Onomastics");
  }
}
