// Decompiled with JetBrains decompiler
// Type: DebugHighlight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class DebugHighlight
{
  public static HashSet<DebugHighlightContainer> hashset = new HashSet<DebugHighlightContainer>();
  private static List<DebugHighlightContainer> to_remove = new List<DebugHighlightContainer>();

  public static void updateDebugHighlights()
  {
    if (DebugHighlight.hashset.Count == 0)
      return;
    DebugHighlight.to_remove.Clear();
    foreach (DebugHighlightContainer highlightContainer in DebugHighlight.hashset)
    {
      highlightContainer.timer -= World.world.delta_time;
      if ((double) highlightContainer.timer < 0.0)
        DebugHighlight.to_remove.Add(highlightContainer);
    }
    foreach (DebugHighlightContainer highlightContainer in DebugHighlight.to_remove)
      DebugHighlight.hashset.Remove(highlightContainer);
  }

  public static void newHighlightList(Color pColor, List<TileZone> pZones, float pTime = 3f)
  {
    foreach (TileZone pZone in pZones)
      DebugHighlight.newHighlight(pColor, pZone, pTime);
  }

  public static void newHighlightList(Color pColor, List<MapChunk> pChunks, float pTime = 3f)
  {
    foreach (MapChunk pChunk in pChunks)
      DebugHighlight.newHighlight(pColor, pChunk, pTime);
  }

  public static void clear() => DebugHighlight.hashset.Clear();

  public static void newHighlight(Color pColor, MapChunk pChunk, float pTime = 3f)
  {
    DebugHighlightContainer highlightContainer = new DebugHighlightContainer();
    highlightContainer.chunk = pChunk;
    highlightContainer.color = pColor;
    highlightContainer.setTimer(pTime);
    DebugHighlight.hashset.Add(highlightContainer);
  }

  public static void newHighlight(Color pColor, TileZone pZone, float pTime = 3f)
  {
    DebugHighlightContainer highlightContainer = new DebugHighlightContainer();
    highlightContainer.zone = pZone;
    highlightContainer.color = pColor;
    highlightContainer.setTimer(pTime);
    DebugHighlight.hashset.Add(highlightContainer);
  }
}
