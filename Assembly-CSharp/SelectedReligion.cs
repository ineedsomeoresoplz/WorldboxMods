// Decompiled with JetBrains decompiler
// Type: SelectedReligion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedReligion : SelectedMeta<Religion, ReligionData>
{
  [SerializeField]
  private CitiesKingdomsContainersController _banners_cities_kingdoms;

  protected override MetaType meta_type => MetaType.Religion;

  protected override string getPowerTabAssetID() => "selected_religion";

  protected override void showStatsGeneral(Religion pReligion)
  {
    base.showStatsGeneral(pReligion);
    this.setIconValue("i_kingdoms", (float) pReligion.countKingdoms());
    this.setIconValue("i_cities", (float) pReligion.countCities());
    this.setIconValue("i_books", (float) pReligion.books.count());
  }

  protected override void updateElementsOnChange(Religion pNano)
  {
    base.updateElementsOnChange(pNano);
    this._banners_cities_kingdoms.update((NanoObject) pNano);
  }

  protected override void checkAchievements(Religion pNano)
  {
    AchievementLibrary.not_just_a_cult.checkBySignal((object) pNano);
  }
}
