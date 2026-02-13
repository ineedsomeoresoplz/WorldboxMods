// Decompiled with JetBrains decompiler
// Type: ai.behaviours.KingdomBehCheckCapital
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class KingdomBehCheckCapital : BehaviourActionKingdom
{
  public override BehResult execute(Kingdom pKingdom)
  {
    if (!pKingdom.hasCities() || pKingdom.hasCapital() && pKingdom.capital.isAlive())
      return BehResult.Continue;
    City pCity = (City) null;
    foreach (City city in pKingdom.cities)
    {
      if (pCity == null || city.buildings.Count > pCity.buildings.Count)
        pCity = city;
    }
    pKingdom.setCapital(pCity);
    return BehResult.Continue;
  }
}
