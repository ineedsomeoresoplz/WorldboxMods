// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFingerGoTowardsTileTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFingerGoTowardsTileTarget : BehFinger
{
  private int _tile_range;

  public BehFingerGoTowardsTileTarget(int pRadiusTileRange = 25)
  {
    this._tile_range = pRadiusTileRange;
  }

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    WorldTile tileWithinDistance1 = Toolbox.getRandomTileWithinDistance(pActor.current_tile, this._tile_range);
    WorldTile tileWithinDistance2 = Toolbox.getRandomTileWithinDistance(tileWithinDistance1, this._tile_range);
    WorldTile tileWithinDistance3 = Toolbox.getRandomTileWithinDistance(pActor.beh_tile_target, this._tile_range);
    WorldTile tileWithinDistance4 = Toolbox.getRandomTileWithinDistance(tileWithinDistance3, this._tile_range);
    return ActorMove.goToCurved(pActor, pActor.current_tile, tileWithinDistance1, tileWithinDistance2, tileWithinDistance4, tileWithinDistance3) == ExecuteEvent.False ? BehResult.Stop : BehResult.Continue;
  }
}
