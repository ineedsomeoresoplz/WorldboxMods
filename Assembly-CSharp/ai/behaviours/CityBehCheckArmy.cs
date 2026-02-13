// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehCheckArmy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class CityBehCheckArmy : BehaviourActionCity
{
  public override BehResult execute(City pCity)
  {
    pCity.checkArmyExistence();
    if (pCity.hasArmy() || !pCity.hasAnyWarriors())
      return BehResult.Continue;
    Actor randomWarrior = pCity.getRandomWarrior();
    if (randomWarrior == null)
      return BehResult.Continue;
    BehaviourActionBase<City>.world.armies.newArmy(randomWarrior, pCity);
    return BehResult.Continue;
  }
}
