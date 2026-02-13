// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatFishing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehBoatFishing : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    this.spawnFishnet(pActor);
    return BehResult.Continue;
  }

  public void spawnFishnet(Actor pActor)
  {
    if (!MapBox.isRenderGameplay())
      return;
    Vector2 vector2 = Randy.randomPointOnCircle(3f, 4f);
    MapBox world = BehaviourActionBase<Actor>.world;
    Vector2Int pos1 = pActor.current_tile.pos;
    int pX = ((Vector2Int) ref pos1).x + (int) vector2.x;
    Vector2Int pos2 = pActor.current_tile.pos;
    int pY = ((Vector2Int) ref pos2).y + (int) vector2.y;
    WorldTile tile = world.GetTile(pX, pY);
    if (tile == null || !tile.Type.ocean)
      return;
    EffectsLibrary.spawnAtTile("fx_fishnet", tile, pActor.asset.base_stats["scale"]);
  }
}
