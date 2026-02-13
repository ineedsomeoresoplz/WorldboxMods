// Decompiled with JetBrains decompiler
// Type: TileLibraryMain`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TileLibraryMain<T> : AssetLibrary<T> where T : TileTypeBase
{
  public override void linkAssets()
  {
    base.linkAssets();
    foreach (T obj in this.list)
    {
      TileTypeBase tileTypeBase1 = (TileTypeBase) obj;
      TileTypeBase tileTypeBase2 = tileTypeBase1;
      HashSet<BiomeTag> biomeTags = tileTypeBase1.biome_tags;
      // ISSUE: explicit non-virtual call
      int num = biomeTags != null ? (__nonvirtual (biomeTags.Count) > 0 ? 1 : 0) : 0;
      tileTypeBase2.has_biome_tags = num != 0;
      if (tileTypeBase1.color_hex != null)
        tileTypeBase1.color = Color32.op_Implicit(Toolbox.makeColor(tileTypeBase1.color_hex));
      if (tileTypeBase1.edge_color_hex != null)
        tileTypeBase1.edge_color = Color32.op_Implicit(Toolbox.makeColor(tileTypeBase1.edge_color_hex));
    }
  }
}
