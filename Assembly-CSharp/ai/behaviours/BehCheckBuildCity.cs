// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCheckBuildCity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCheckBuildCity : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.current_tile.zone.hasCity() || !WorldLawLibrary.world_law_kingdom_expansion.isEnabled() || !pActor.current_tile.zone.isGoodForNewCity(pActor))
      return BehResult.Stop;
    City pCity = BehaviourActionBase<Actor>.world.cities.buildNewCity(pActor, pActor.current_zone);
    pActor.joinCity(pCity);
    return BehResult.Continue;
  }
}
