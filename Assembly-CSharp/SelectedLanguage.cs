// Decompiled with JetBrains decompiler
// Type: SelectedLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedLanguage : SelectedMeta<Language, LanguageData>
{
  [SerializeField]
  private CitiesKingdomsContainersController _banners_cities_kingdoms;

  protected override MetaType meta_type => MetaType.Language;

  protected override string getPowerTabAssetID() => "selected_language";

  protected override void showStatsGeneral(Language pLanguage)
  {
    base.showStatsGeneral(pLanguage);
    this.setIconValue("i_books", (float) pLanguage.books.count());
    this.setIconValue("i_kingdoms", (float) pLanguage.countKingdoms());
    this.setIconValue("i_cities", (float) pLanguage.countCities());
    this.setIconValue("i_books_written", (float) pLanguage.data.books_written);
  }

  protected override void updateElementsOnChange(Language pNano)
  {
    base.updateElementsOnChange(pNano);
    this._banners_cities_kingdoms.update((NanoObject) pNano);
  }

  protected override void checkAchievements(Language pNano)
  {
    AchievementLibrary.multiply_spoken.checkBySignal((object) pNano);
  }
}
