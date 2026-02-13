// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehConsumeGrass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehConsumeGrass : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    WorldTile behTileTarget = pActor.beh_tile_target;
    if (!behTileTarget.Type.grass)
      return BehResult.Stop;
    pActor.punchTargetAnimation(behTileTarget.posV3, false);
    pActor.consumeTopTile(behTileTarget);
    return BehResult.Continue;
  }
}
