// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehMakeFarm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehMakeFarm : BehCityActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (!pActor.beh_tile_target.Type.can_be_farm || pActor.beh_tile_target.hasBuilding() && !pActor.beh_tile_target.building.canRemoveForFarms())
      return BehResult.Stop;
    MapAction.terraformTop(pActor.beh_tile_target, TopTileLibrary.field);
    MusicBox.playSound("event:/SFX/CIVILIZATIONS/MakeFarmField", pActor.beh_tile_target, true, true);
    pActor.addLoot(SimGlobals.m.coins_for_field);
    return BehResult.Continue;
  }
}
