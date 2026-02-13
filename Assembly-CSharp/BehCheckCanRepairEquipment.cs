// Decompiled with JetBrains decompiler
// Type: BehCheckCanRepairEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCheckCanRepairEquipment : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasEquipment())
      return BehResult.Stop;
    bool flag = false;
    foreach (ActorEquipmentSlot actorEquipmentSlot in pActor.equipment)
    {
      if (actorEquipmentSlot.getItem().needRepair())
      {
        int num = (int) ((double) actorEquipmentSlot.getItem().getAsset().cost_gold * (double) SimGlobals.m.item_repair_cost_multiplier);
        if (pActor.money >= num)
          flag = true;
      }
    }
    return !flag ? BehResult.Stop : BehResult.Continue;
  }
}
