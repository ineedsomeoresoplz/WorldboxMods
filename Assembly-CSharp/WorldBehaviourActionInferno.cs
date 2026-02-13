// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActionInferno
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class WorldBehaviourActionInferno
{
  public static void updateInfernalLowAnimations()
  {
    if (TopTileLibrary.infernal_low.hashset.Count < 10 || !Randy.randomChance(0.4f))
      return;
    World.world.redrawRenderedTile(TopTileLibrary.infernal_low.getCurrentTiles().GetRandom<WorldTile>());
  }

  public static void updateInfernoFireAction()
  {
    WorldBehaviourActionInferno.tryFireAction(TopTileLibrary.infernal_high);
    WorldBehaviourActionInferno.tryFireAction(TopTileLibrary.infernal_low);
  }

  private static void tryFireAction(TopTileType pType)
  {
    if (pType.hashset.Count < 10 || !Randy.randomChance(0.1f))
      return;
    WorldTile random = pType.getCurrentTiles().GetRandom<WorldTile>();
    random.startFire(true);
    World.world.particles_fire.spawn(random.posV3);
  }
}
