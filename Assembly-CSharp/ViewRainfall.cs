// Decompiled with JetBrains decompiler
// Type: ViewRainfall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ViewRainfall : MapLayer
{
  internal override void create()
  {
    this.colorValues = new Color(0.0f, 0.0f, 1f);
    this.colors_amount = 10;
    base.create();
    this.sprRnd.color = new Color(1f, 1f, 1f, 0.6f);
  }

  public void setTileDirty(WorldTile pTile)
  {
  }

  protected override void UpdateDirty(float pElapsed)
  {
  }
}
