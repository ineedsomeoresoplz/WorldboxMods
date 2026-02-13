// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatFindWaterTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoatFindWaterTile : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    WorldTile randomTileForBoat = ActorTool.getRandomTileForBoat(pActor);
    pActor.beh_tile_target = randomTileForBoat;
    return BehResult.Continue;
  }
}
