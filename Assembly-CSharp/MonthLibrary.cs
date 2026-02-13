// Decompiled with JetBrains decompiler
// Type: MonthLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class MonthLibrary : AssetLibrary<MonthAsset>
{
  private Dictionary<int, MonthAsset> _month_indexes;

  public override void init()
  {
    base.init();
    MonthAsset pAsset1 = new MonthAsset();
    pAsset1.id = "month_1";
    pAsset1.english_name = "january";
    pAsset1.month_index = 1;
    this.add(pAsset1);
    MonthAsset pAsset2 = new MonthAsset();
    pAsset2.id = "month_2";
    pAsset2.english_name = "february";
    pAsset2.month_index = 2;
    this.add(pAsset2);
    MonthAsset pAsset3 = new MonthAsset();
    pAsset3.id = "month_3";
    pAsset3.english_name = "march";
    pAsset3.month_index = 3;
    this.add(pAsset3);
    MonthAsset pAsset4 = new MonthAsset();
    pAsset4.id = "month_4";
    pAsset4.english_name = "april";
    pAsset4.month_index = 4;
    this.add(pAsset4);
    MonthAsset pAsset5 = new MonthAsset();
    pAsset5.id = "month_5";
    pAsset5.english_name = "may";
    pAsset5.month_index = 5;
    this.add(pAsset5);
    MonthAsset pAsset6 = new MonthAsset();
    pAsset6.id = "month_6";
    pAsset6.english_name = "june";
    pAsset6.month_index = 6;
    this.add(pAsset6);
    MonthAsset pAsset7 = new MonthAsset();
    pAsset7.id = "month_7";
    pAsset7.english_name = "july";
    pAsset7.month_index = 7;
    this.add(pAsset7);
    MonthAsset pAsset8 = new MonthAsset();
    pAsset8.id = "month_8";
    pAsset8.english_name = "august";
    pAsset8.month_index = 8;
    this.add(pAsset8);
    MonthAsset pAsset9 = new MonthAsset();
    pAsset9.id = "month_9";
    pAsset9.english_name = "september";
    pAsset9.month_index = 9;
    this.add(pAsset9);
    MonthAsset pAsset10 = new MonthAsset();
    pAsset10.id = "month_10";
    pAsset10.english_name = "october";
    pAsset10.month_index = 10;
    this.add(pAsset10);
    MonthAsset pAsset11 = new MonthAsset();
    pAsset11.id = "month_11";
    pAsset11.english_name = "november";
    pAsset11.month_index = 11;
    this.add(pAsset11);
    MonthAsset pAsset12 = new MonthAsset();
    pAsset12.id = "month_12";
    pAsset12.english_name = "december";
    pAsset12.month_index = 12;
    this.add(pAsset12);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    this._month_indexes = new Dictionary<int, MonthAsset>();
    foreach (MonthAsset monthAsset in this.list)
      this._month_indexes.Add(monthAsset.month_index, monthAsset);
  }

  public MonthAsset getMonth(int pMonthIndex) => this._month_indexes[pMonthIndex];

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (MonthAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
  }
}
