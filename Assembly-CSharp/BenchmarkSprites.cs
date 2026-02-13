// Decompiled with JetBrains decompiler
// Type: BenchmarkSprites
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BenchmarkSprites
{
  public static void start()
  {
    if (!Config.game_loaded || !SelectedUnit.isSet())
      return;
    Actor unit = SelectedUnit.unit;
    if (!unit.is_visible)
      return;
    int pCounter = 100;
    Bench.bench("sprites_old", "sprites_test");
    for (int index = 0; index < pCounter; ++index)
      DynamicSpriteCreator.createNewSpriteUnit(unit.frame_data, unit.calculateMainSprite(), unit.cached_sprite_head, unit.kingdom.getColor(), unit.asset, unit.data.phenotype_index, unit.data.phenotype_shade, UnitTextureAtlasID.Units);
    Bench.benchEnd("sprites_old", "sprites_test", true, (long) pCounter);
    Bench.bench("sprites_new", "sprites_test");
    for (int index = 0; index < pCounter; ++index)
      DynamicSpriteCreator.createNewSpriteUnit(unit.frame_data, unit.calculateMainSprite(), unit.cached_sprite_head, unit.kingdom.getColor(), unit.asset, unit.data.phenotype_index, unit.data.phenotype_shade, UnitTextureAtlasID.Units);
    Bench.benchEnd("sprites_new", "sprites_test", true, (long) pCounter);
  }
}
