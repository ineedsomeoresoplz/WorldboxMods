// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehPlantCrops
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehPlantCrops : BehCityActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_tile_target.Type != TopTileLibrary.field || pActor.beh_tile_target.hasBuilding())
      return BehResult.Stop;
    BehaviourActionBase<Actor>.world.buildings.addBuilding("wheat", pActor.beh_tile_target);
    pActor.addLoot(SimGlobals.m.coins_for_planting);
    MusicBox.playSound("event:/SFX/CIVILIZATIONS/PlantCrops", pActor.beh_tile_target, true);
    return BehResult.Continue;
  }
}
