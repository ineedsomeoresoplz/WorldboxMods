// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehCheckDestruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class CityBehCheckDestruction : BehaviourActionCity
{
  public override BehResult execute(City pCity)
  {
    if (!pCity.isGettingCaptured())
      return BehResult.Continue;
    Kingdom capturingKingdom = pCity.getCapturingKingdom();
    if (capturingKingdom.isRekt())
      return BehResult.Continue;
    Actor king = capturingKingdom.king;
    if ((king != null ? (!king.hasXenophobic() ? 1 : 0) : 1) != 0 || capturingKingdom.getSpecies() == pCity.kingdom.getSpecies())
      return BehResult.Continue;
    foreach (BaseSimObject unit in pCity.getUnits())
    {
      if (unit.current_zone.city == pCity)
        return BehResult.Continue;
    }
    pCity.kingdom.decreaseHappinessFromRazedCity(pCity);
    capturingKingdom.increaseHappinessFromDestroyingCity();
    foreach (Actor unit in pCity.getUnits())
    {
      unit.stopBeingWarrior();
      unit.joinCity((City) null);
    }
    if (pCity.hasLeader())
      pCity.removeLeader();
    return BehResult.Continue;
  }
}
