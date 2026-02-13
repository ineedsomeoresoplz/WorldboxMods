// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehGoToBuildingTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehGoToBuildingTarget : BehActorBuildingTarget
{
  private bool _path_on_water;

  public BehGoToBuildingTarget(bool pPathOnWater = false) => this._path_on_water = pPathOnWater;

  public override BehResult execute(Actor pActor)
  {
    this.goToBuilding(pActor);
    return BehResult.Continue;
  }

  internal void goToBuilding(Actor pActor)
  {
    WorldTile currentTile = pActor.beh_building_target.current_tile;
    int num = (int) pActor.goTo(currentTile, this._path_on_water);
  }
}
