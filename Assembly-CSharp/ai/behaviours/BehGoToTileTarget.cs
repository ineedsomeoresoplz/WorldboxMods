// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehGoToTileTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehGoToTileTarget : BehaviourActionActor
{
  public bool walk_on_water;
  public bool walk_on_blocks;
  public int limit_pathfinding_regions;

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
    this.walk_on_water = false;
  }

  public override BehResult execute(Actor pActor)
  {
    return pActor.goTo(pActor.beh_tile_target, this.walk_on_water, this.walk_on_blocks, pLimitPathfindingRegions: this.limit_pathfinding_regions) == ExecuteEvent.False ? BehResult.Stop : BehResult.Continue;
  }
}
