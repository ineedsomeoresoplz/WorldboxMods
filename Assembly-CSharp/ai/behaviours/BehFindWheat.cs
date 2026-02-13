// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindWheat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindWheat : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    int num1 = int.MaxValue;
    WorldTile worldTile = (WorldTile) null;
    Building building = (Building) null;
    foreach (WorldTile pT2 in (ObjectContainer<WorldTile>) pActor.city.calculated_grown_wheat)
    {
      if (pT2.building != null && pT2.building.asset.wheat)
      {
        int num2 = Toolbox.SquaredDistTile(pActor.current_tile, pT2);
        if (num2 < num1 && pT2.building.isUsable() && pT2.building.component_wheat.isMaxLevel() && pT2.isSameIsland(pActor.current_tile) && !pT2.isTargeted())
        {
          num1 = num2;
          worldTile = pT2;
          building = pT2.building;
        }
      }
    }
    if (worldTile == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile;
    pActor.beh_building_target = building;
    return BehResult.Continue;
  }
}
