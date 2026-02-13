// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActionCreepBiomass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldBehaviourActionCreepBiomass
{
  public static void updateBiomassTiles()
  {
    int num1 = 0;
    while (num1 < 3 && WorldBehaviourActionCreepBiomass.tryActionOn(TopTileLibrary.biomass_low))
      ++num1;
    int num2 = 0;
    while (num2 < 3 && WorldBehaviourActionCreepBiomass.tryActionOn(TopTileLibrary.biomass_high))
      ++num2;
  }

  private static bool tryActionOn(TopTileType pType)
  {
    if (pType.hashset.Count < 10)
      return false;
    World.world.redrawRenderedTile(pType.getCurrentTiles().GetRandom<WorldTile>());
    return true;
  }
}
