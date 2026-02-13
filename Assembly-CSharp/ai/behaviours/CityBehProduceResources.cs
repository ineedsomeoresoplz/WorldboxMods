// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehProduceResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class CityBehProduceResources : BehaviourActionCity
{
  public override bool shouldRetry(City pCity) => false;

  public override BehResult execute(City pCity)
  {
    ActorAsset actorAsset = pCity.getActorAsset();
    if (actorAsset.production == null)
      return BehResult.Stop;
    foreach (string str in actorAsset.production.LoopRandom<string>())
    {
      ResourceAsset pAsset = AssetManager.resources.get(str);
      int resourcesAmount = pCity.getResourcesAmount(str);
      if (resourcesAmount <= pAsset.maximum)
      {
        int pAmount = pCity.status.population / 10 + 1;
        if (resourcesAmount < pAsset.produce_min)
          pAmount = pAsset.produce_min;
        this.tryToProduce(pAsset, pCity, pAmount);
      }
    }
    return BehResult.Continue;
  }

  private bool tryToProduce(ResourceAsset pAsset, City pCity, int pAmount = 1)
  {
    for (int index = 0; index < pAmount; ++index)
    {
      if (pCity.getResourcesAmount(pAsset.id) == pAsset.maximum)
        return false;
      foreach (string ingredient in pAsset.ingredients)
      {
        if (pCity.getResourcesAmount(ingredient) < pAsset.ingredients_amount)
          return false;
      }
      foreach (string ingredient in pAsset.ingredients)
        pCity.takeResource(ingredient, pAsset.ingredients_amount);
      if (pCity.addResourcesToRandomStockpile(pAsset.id) <= 0)
        return false;
    }
    return true;
  }
}
