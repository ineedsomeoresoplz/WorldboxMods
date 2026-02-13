// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActionSwampAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class WorldBehaviourActionSwampAnimation
{
  public static void updateSwampTiles()
  {
    if (TopTileLibrary.swamp_low.hashset.Count < 10)
      return;
    World.world.redrawRenderedTile(TopTileLibrary.swamp_low.getCurrentTiles().GetRandom<WorldTile>());
  }
}
