// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCheckNeeds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCheckNeeds : BehCityActor
{
  private int _max_restarts;

  public BehCheckNeeds(int pRestarts) => this._max_restarts = pRestarts;

  public override BehResult execute(Actor pActor)
  {
    if (this._max_restarts > 0 && pActor.ai.restarts >= this._max_restarts)
      return BehResult.Stop;
    if (!pActor.isStarving() || !pActor.city.hasAnyFood())
      return BehResult.Continue;
    pActor.endJob();
    return BehResult.Stop;
  }
}
