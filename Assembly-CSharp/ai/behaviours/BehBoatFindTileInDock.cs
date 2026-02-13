// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatFindTileInDock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoatFindTileInDock : BehBoat
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.check_building_target_non_usable = true;
    this.null_check_building_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (!pActor.beh_building_target.isCiv())
      return BehResult.Stop;
    WorldTile oceanTileInSameOcean = pActor.beh_building_target.component_docks.getOceanTileInSameOcean(pActor.current_tile);
    if (oceanTileInSameOcean == null)
      return BehResult.Stop;
    pActor.beh_tile_target = oceanTileInSameOcean;
    return BehResult.Continue;
  }
}
