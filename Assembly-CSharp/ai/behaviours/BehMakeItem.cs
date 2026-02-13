// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehMakeItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehMakeItem : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    ItemCrafting.tryToCraftRandomWeapon(pActor, pActor.city);
    int num = 0;
    while (num < 5 && ItemCrafting.tryToCraftRandomEquipment(pActor, pActor.city))
      ++num;
    return BehResult.Continue;
  }
}
