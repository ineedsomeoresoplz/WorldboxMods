// Decompiled with JetBrains decompiler
// Type: BuildingBiomeFoodProducer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BuildingBiomeFoodProducer : BaseBuildingComponent
{
  private const float FOOD_INTERVAL = 90f;
  private float timer = 90f;

  public override void update(float pElapsed)
  {
    if (this.building.city == null || !this.building.isUsable())
      return;
    if ((double) this.timer > 0.0)
    {
      this.timer -= pElapsed;
    }
    else
    {
      this.timer = 90f;
      WorldTile random = this.building.tiles.GetRandom<WorldTile>();
      string foodResource = random.Type.food_resource;
      if (string.IsNullOrEmpty(foodResource))
        foodResource = random.main_type.food_resource;
      if (string.IsNullOrEmpty(foodResource) || this.building.city.getResourcesAmount(foodResource) >= 10)
        return;
      this.building.city.addResourcesToRandomStockpile(foodResource);
    }
  }
}
