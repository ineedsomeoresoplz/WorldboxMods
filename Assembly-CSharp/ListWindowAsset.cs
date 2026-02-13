// Decompiled with JetBrains decompiler
// Type: ListWindowAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ListWindowAsset : Asset, IMultiLocalesAsset
{
  public string no_items_locale;
  public string no_dead_items_locale;
  public string art_path;
  public string icon_path;
  public MetaType meta_type;
  public ListComponentSetter set_list_component;

  public IEnumerable<string> getLocaleIDs()
  {
    yield return this.no_items_locale;
    if (!string.IsNullOrEmpty(this.no_dead_items_locale))
      yield return this.no_dead_items_locale;
  }
}
