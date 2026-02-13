// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehJoinCity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehJoinCity : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    City city = pActor.current_zone.city;
    if (city == null || !city.isPossibleToJoin(pActor))
      return BehResult.Stop;
    if (city.isNeutral())
    {
      if (pActor.kingdom.isCiv())
      {
        city.setKingdom(pActor.kingdom);
      }
      else
      {
        Kingdom pKingdom = BehaviourActionBase<Actor>.world.kingdoms.makeNewCivKingdom(pActor);
        pActor.createDefaultCultureAndLanguageAndClan();
        city.setKingdom(pKingdom);
        city.setUnitMetas(pActor);
        pKingdom.setUnitMetas(pActor);
      }
    }
    if (city.kingdom != pActor.kingdom)
      pActor.removeFromPreviousFaction();
    pActor.joinCity(city);
    pActor.setMetasFromCity(city);
    return BehResult.Continue;
  }
}
