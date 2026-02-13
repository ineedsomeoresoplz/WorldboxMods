// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCheckSameCityActorLover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCheckSameCityActorLover : BehCitizenActionCity
{
  public override bool errorsFound(Actor pObject)
  {
    return base.errorsFound(pObject) || !pObject.hasLover() || pObject.lover.isRekt() || pObject.lover.city.isRekt();
  }

  public override BehResult execute(Actor pActor)
  {
    City city = pActor.lover.city;
    pActor.clearHomeBuilding();
    pActor.stopBeingWarrior();
    pActor.joinCity(city);
    return BehResult.Continue;
  }
}
