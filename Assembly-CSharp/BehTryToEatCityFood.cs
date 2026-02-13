// Decompiled with JetBrains decompiler
// Type: BehTryToEatCityFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehTryToEatCityFood : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    City city = pActor.city;
    if (!city.hasSuitableFood(pActor.subspecies))
      return BehResult.Stop;
    ResourceAsset foodItem1 = city.getFoodItem(pActor.subspecies, pActor.data.favorite_food);
    bool pNeedToPay = !pActor.isFoodFreeForThisPerson();
    if (foodItem1 != null)
    {
      if (pNeedToPay && !pActor.hasEnoughMoney(foodItem1.money_cost))
        return BehResult.Stop;
      this.eatFood(pActor, city, foodItem1, pNeedToPay);
      if (pActor.hasTrait("gluttonous"))
      {
        ResourceAsset foodItem2 = city.getFoodItem(pActor.subspecies, pActor.data.favorite_food);
        if (foodItem2 != null && pNeedToPay && pActor.hasEnoughMoney(foodItem2.money_cost))
          this.eatFood(pActor, city, foodItem2, true);
      }
    }
    return BehResult.Continue;
  }

  private void eatFood(Actor pActor, City pCity, ResourceAsset pFoodItem, bool pNeedToPay)
  {
    if (pNeedToPay)
      pActor.spendMoney(pFoodItem.money_cost);
    pCity.eatFoodItem(pFoodItem.id);
    pActor.consumeFoodResource(pFoodItem);
  }
}
