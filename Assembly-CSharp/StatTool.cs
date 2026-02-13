// Decompiled with JetBrains decompiler
// Type: StatTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class StatTool
{
  public static float getDPS(Actor pActor)
  {
    ActorAsset asset = pActor.asset;
    return asset.base_stats["damage"] * (1f / asset.base_stats["attack_speed"]);
  }

  public static float getSecondsLife(Actor pActor) => pActor.asset.base_stats["lifespan"] * 60f;

  public static string getStringSecondsLife(Actor pActor)
  {
    float pValue = pActor.asset.base_stats["lifespan"] * 60f;
    return pValue.ToString("0") + StatTool.toMinutes(pValue);
  }

  public static string getAmountFood(Actor pActor)
  {
    float nutritionMax = (float) pActor.asset.nutrition_max;
    float intervalNutritionDecay = SimGlobals.m.interval_nutrition_decay;
    return (StatTool.getSecondsLife(pActor) / (intervalNutritionDecay * nutritionMax)).ToString("0.0");
  }

  public static string getStringAmountBreeding(Actor pActor)
  {
    ActorAsset asset = pActor.asset;
    if (!pActor.hasSubspecies())
      return "0.0";
    float num = (float) asset.months_breeding_timeout * 5f;
    float pValue = StatTool.getSecondsLife(pActor) - pActor.subspecies.age_breeding * 60f;
    return (pValue / num).ToString("0.0") + StatTool.toMinutes(pValue);
  }

  private static string toMinutes(float pValue)
  {
    float num1 = pValue / 60f;
    float num2 = pValue / 60f;
    return $" ({num1.ToString("0.0")}m) {num2.ToString("0.0")}y";
  }
}
