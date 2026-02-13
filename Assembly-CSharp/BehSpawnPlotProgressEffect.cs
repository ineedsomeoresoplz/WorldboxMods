// Decompiled with JetBrains decompiler
// Type: BehSpawnPlotProgressEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class BehSpawnPlotProgressEffect : BehaviourActionActor
{
  private int _amount;

  public BehSpawnPlotProgressEffect(int pAmount = 1) => this._amount = pAmount;

  public override BehResult execute(Actor pActor)
  {
    TileZone zone = pActor.current_tile.zone;
    for (int index = 0; index < this._amount; ++index)
    {
      Vector3 pPos = Vector2.op_Implicit(pActor.current_position);
      pPos.y += 5f * pActor.actor_scale;
      pPos.y += Randy.randomFloat((float) (-(double) pActor.actor_scale * 3.0), pActor.actor_scale * 3f);
      pPos.x += Randy.randomFloat((float) (-(double) pActor.actor_scale * 2.0), pActor.actor_scale * 2f);
      Object.op_Equality((Object) EffectsLibrary.spawnAt("fx_plot_progress", pPos, pActor.actor_scale * 0.8f), (Object) null);
    }
    return BehResult.Continue;
  }
}
