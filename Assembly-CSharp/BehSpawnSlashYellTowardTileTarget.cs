// Decompiled with JetBrains decompiler
// Type: BehSpawnSlashYellTowardTileTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class BehSpawnSlashYellTowardTileTarget : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.spawnSlashYell(Vector2Int.op_Implicit(pActor.beh_tile_target.pos));
    return BehResult.Continue;
  }
}
