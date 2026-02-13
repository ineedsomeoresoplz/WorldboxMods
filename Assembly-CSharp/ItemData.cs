// Decompiled with JetBrains decompiler
// Type: ItemData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class ItemData : BaseSystemData
{
  public int durability = 100;
  public bool created_by_player;
  [DefaultValue("")]
  public string by = string.Empty;
  internal string byColor = string.Empty;
  [DefaultValue("")]
  public string from = string.Empty;
  internal string fromColor = string.Empty;
  [DefaultValue(0)]
  public int kills;
  public string asset_id = string.Empty;
  public string material = string.Empty;
  public readonly ListPool<string> modifiers = new ListPool<string>();

  [DefaultValue(-1)]
  public long creator_id { get; set; } = -1;

  [DefaultValue(-1)]
  public long creator_kingdom_id { get; set; } = -1;

  public bool ShouldSerializemodifiers() => this.modifiers.Count > 0;

  public override void Dispose()
  {
    this.modifiers.Dispose();
    base.Dispose();
  }
}
