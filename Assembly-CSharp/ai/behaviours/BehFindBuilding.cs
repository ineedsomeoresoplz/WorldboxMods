// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindBuilding : BehaviourActionActor
{
  private readonly bool _only_non_targeted;
  private readonly bool _only_with_resources;
  private readonly string _type;

  public BehFindBuilding(string pType, bool pOnlyNonTargeted, bool pOnlyWithResources)
  {
    this._type = pType;
    this._only_non_targeted = pOnlyNonTargeted;
    this._only_with_resources = pOnlyWithResources;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.beh_building_target = this.findBuildingType(pActor, this._type);
    return pActor.beh_building_target != null ? BehResult.Continue : BehResult.Stop;
  }

  private Building findBuildingType(Actor pActor, string pType)
  {
    Building buildingType1 = (Building) null;
    (MapChunk[], int) allChunksFromTile = Toolbox.getAllChunksFromTile(pActor.current_tile);
    foreach (MapChunk pChunk in allChunksFromTile.Item1.LoopRandom<MapChunk>(allChunksFromTile.Item2))
    {
      foreach (Building buildingType2 in Toolbox.getBuildingsTypeFromChunk(pChunk, pType, this._only_non_targeted, this._only_with_resources))
      {
        if (buildingType2.current_tile.isSameIsland(pActor.current_tile))
          return buildingType2;
        buildingType1 = buildingType2;
      }
    }
    return buildingType1;
  }
}
