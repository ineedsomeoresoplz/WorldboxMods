// Decompiled with JetBrains decompiler
// Type: DebugHighlightContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DebugHighlightContainer
{
  public Color color;
  public float timer = 0.2f;
  public float interval = 0.2f;
  public TileZone zone;
  public MapChunk chunk;
  public WorldTile tile;

  public void setTimer(float pVal)
  {
    this.interval = pVal;
    this.timer = pVal;
  }
}
