// Decompiled with JetBrains decompiler
// Type: BehRepairEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehRepairEquipment : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    bool flag = false;
    foreach (ActorEquipmentSlot actorEquipmentSlot in pActor.equipment)
    {
      if (!actorEquipmentSlot.isEmpty())
      {
        Item obj = actorEquipmentSlot.getItem();
        if (obj.needRepair())
        {
          int pCost = (int) ((double) actorEquipmentSlot.getItem().getAsset().cost_gold * (double) SimGlobals.m.item_repair_cost_multiplier);
          if (pActor.hasEnoughMoney(pCost))
          {
            pActor.spendMoney(pCost);
            obj.fullRepair();
            flag = true;
          }
        }
      }
    }
    if (flag)
      pActor.setStatsDirty();
    return BehResult.Continue;
  }
}
