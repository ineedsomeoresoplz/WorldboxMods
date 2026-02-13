// Decompiled with JetBrains decompiler
// Type: HistoryGroupLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class HistoryGroupLibrary : AssetLibrary<HistoryGroupAsset>
{
  public override void init()
  {
    base.init();
    HistoryGroupAsset pAsset1 = new HistoryGroupAsset();
    pAsset1.id = "kings";
    pAsset1.icon_path = "ui/Icons/history_group/icon_kings";
    this.add(pAsset1);
    HistoryGroupAsset pAsset2 = new HistoryGroupAsset();
    pAsset2.id = "favorite_units";
    pAsset2.icon_path = "ui/Icons/iconFavoriteStar";
    this.add(pAsset2);
    HistoryGroupAsset pAsset3 = new HistoryGroupAsset();
    pAsset3.id = "cities";
    pAsset3.icon_path = "ui/Icons/iconCityList";
    this.add(pAsset3);
    HistoryGroupAsset pAsset4 = new HistoryGroupAsset();
    pAsset4.id = "kingdoms";
    pAsset4.icon_path = "ui/Icons/iconKingdomList";
    this.add(pAsset4);
    HistoryGroupAsset pAsset5 = new HistoryGroupAsset();
    pAsset5.id = "wars";
    pAsset5.icon_path = "ui/Icons/iconWarList";
    this.add(pAsset5);
    HistoryGroupAsset pAsset6 = new HistoryGroupAsset();
    pAsset6.id = "clans";
    pAsset6.icon_path = "ui/Icons/iconClanList";
    this.add(pAsset6);
    HistoryGroupAsset pAsset7 = new HistoryGroupAsset();
    pAsset7.id = "disasters";
    pAsset7.icon_path = "ui/Icons/worldrules/icon_disasters";
    this.add(pAsset7);
  }

  public override void post_init()
  {
    foreach (HistoryGroupAsset historyGroupAsset in this.list)
    {
      if (string.IsNullOrEmpty(historyGroupAsset.icon_path))
        historyGroupAsset.icon_path = "ui/Icons/history_group/icon_" + historyGroupAsset.id;
    }
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (HistoryGroupAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }
}
