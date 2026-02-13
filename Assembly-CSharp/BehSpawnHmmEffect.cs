// Decompiled with JetBrains decompiler
// Type: BehSpawnHmmEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehSpawnHmmEffect : BehaviourActionActor
{
  private int _amount;

  public BehSpawnHmmEffect(int pAmount = 1) => this._amount = pAmount;

  public override BehResult execute(Actor pActor)
  {
    TileZone zone = pActor.current_tile.zone;
    for (int index = 0; index < this._amount; ++index)
      EffectsLibrary.spawnAt("fx_hmm", pActor.current_tile.neighbours.GetRandom<WorldTile>().posV, pActor.actor_scale);
    return BehResult.Continue;
  }
}
