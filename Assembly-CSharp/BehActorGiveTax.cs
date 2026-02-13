// Decompiled with JetBrains decompiler
// Type: BehActorGiveTax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehActorGiveTax : BehCitizenActionCity
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.isKing())
    {
      pActor.takeAllOwnLoot();
      return BehResult.Continue;
    }
    if (!pActor.city.hasLeader())
    {
      pActor.takeAllOwnLoot();
      return BehResult.Continue;
    }
    Actor leader = pActor.city.leader;
    if (pActor.isCityLeader())
    {
      if (!pActor.kingdom.hasKing())
        pActor.takeAllOwnLoot();
      else
        this.payTributeToKing(pActor, pActor.kingdom.king, pActor.kingdom.getTaxRateTribute());
    }
    else
      this.payTaxToLeader(pActor, leader, pActor.kingdom.getTaxRateLocal());
    return BehResult.Continue;
  }

  private void payTributeToKing(Actor pActor, Actor pKing, float pTaxRate)
  {
    if (pActor.loot <= 0)
      return;
    int loot;
    int pLootValue = (int) ((double) (loot = pActor.loot) * (double) pTaxRate);
    int num1 = pLootValue;
    int num2 = loot - num1;
    int pAmount = (int) ((double) num2 * 0.5);
    int pValue = num2 - pAmount;
    pActor.city.addResourcesToRandomStockpile("gold", pAmount);
    pActor.addMoney(pValue);
    pKing.addLoot(pLootValue);
    pActor.paidTax(pTaxRate, "fx_money_paid_tribute");
  }

  private void payTaxToLeader(Actor pActor, Actor pTarget, float pTaxRate)
  {
    if (pActor.loot <= 0)
      return;
    int loot;
    int pLootValue = (int) ((double) (loot = pActor.loot) * (double) pTaxRate);
    int num = pLootValue;
    int pValue = loot - num;
    pActor.addMoney(pValue);
    pTarget.addLoot(pLootValue);
    pActor.paidTax(pTaxRate, "fx_money_paid_tax");
  }
}
