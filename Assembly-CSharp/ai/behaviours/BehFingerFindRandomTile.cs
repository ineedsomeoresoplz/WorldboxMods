// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFingerFindRandomTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFingerFindRandomTile : BehFinger
{
  private int _range;

  public BehFingerFindRandomTile(int pRange = 75) => this._range = pRange;

  public override BehResult execute(Actor pActor)
  {
    pActor.findCurrentTile(false);
    pActor.beh_tile_target = Toolbox.getRandomTileWithinDistance(pActor.current_tile, this._range);
    return BehResult.Continue;
  }
}
