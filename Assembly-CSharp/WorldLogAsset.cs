// Decompiled with JetBrains decompiler
// Type: WorldLogAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class WorldLogAsset : Asset, IMultiLocalesAsset
{
  public string group;
  public string locale_id;
  public string path_icon;
  public Color color = Toolbox.color_log_neutral;
  public int random_ids;
  public WorldLogTextFormatter text_replacer;

  public string getLocaleID() => this.locale_id ?? this.id;

  public string getLocaleID(int pIndex) => $"{this.getLocaleID()}_{pIndex}";

  public IEnumerable<string> getLocaleIDs()
  {
    if (this.random_ids == 0)
    {
      yield return this.getLocaleID();
    }
    else
    {
      for (int i = 1; i <= this.random_ids; ++i)
        yield return this.getLocaleID(i);
    }
  }
}
