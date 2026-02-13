// Decompiled with JetBrains decompiler
// Type: MetaTextReportAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class MetaTextReportAsset : Asset, IMultiLocalesAsset
{
  public MetaTextReportAction report_action;
  public string color;
  public int amount = 5;

  internal string get_locale_id => $"meta_report_{this.id}_";

  internal string get_random_text
  {
    get => LocalizedTextManager.getText($"{this.get_locale_id}{Randy.randomInt(0, this.amount)}");
  }

  public IEnumerable<string> getLocaleIDs()
  {
    for (int i = 0; i < this.amount; ++i)
      yield return $"{this.get_locale_id}{i}";
  }
}
