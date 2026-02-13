// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehSupplyKingdomCities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class CityBehSupplyKingdomCities : BehaviourActionCity
{
  public override BehResult execute(City pCity)
  {
    if (!pCity.hasCulture() || pCity.kingdom.countCities() == 1 || (double) pCity.data.timer_supply > 0.0 || !pCity.hasStockpiles())
      return BehResult.Stop;
    Building randomStockpile = pCity.getRandomStockpile();
    if (randomStockpile == null)
      return BehResult.Stop;
    using (ListPool<CityStorageSlot> list = new ListPool<CityStorageSlot>())
    {
      foreach (CityStorageSlot slot in randomStockpile.resources.getSlots())
      {
        if (slot.amount > slot.asset.supply_bound_give)
          list.Add(slot);
      }
      if (list.Count == 0)
        return BehResult.Stop;
      foreach (City pTargetCity in pCity.kingdom.getCities().LoopRandom<City>())
      {
        if (pTargetCity != pCity)
        {
          foreach (CityStorageSlot pSlot in list.LoopRandom<CityStorageSlot>())
          {
            if (pTargetCity.getResourcesAmount(pSlot.id) < pSlot.asset.supply_bound_take)
            {
              this.shareResource(pCity, pTargetCity, pSlot);
              this.updateSupplyTimer(pCity);
              return BehResult.Continue;
            }
          }
        }
      }
      return BehResult.Continue;
    }
  }

  private void updateSupplyTimer(City pCity)
  {
    float num = 30f;
    if (pCity.hasLeader())
      num *= pCity.leader.stats["multiplier_supply_timer"];
    if ((double) num < 10.0)
      num = 10f;
    pCity.data.timer_supply = num;
  }

  private void shareResource(City pCity, City pTargetCity, CityStorageSlot pSlot)
  {
    pCity.takeResource(pSlot.id, pSlot.asset.supply_give);
    pTargetCity.addResourcesToRandomStockpile(pSlot.id, pSlot.asset.supply_give);
  }
}
