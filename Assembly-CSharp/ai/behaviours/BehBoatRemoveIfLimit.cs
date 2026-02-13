// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatRemoveIfLimit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoatRemoveIfLimit : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    if (!this.boat.isHomeDockOverfilled())
      return BehResult.Continue;
    this.boat.destroyBecauseOverfilled();
    return BehResult.Stop;
  }
}
