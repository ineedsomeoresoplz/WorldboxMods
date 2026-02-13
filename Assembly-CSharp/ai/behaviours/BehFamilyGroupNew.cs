// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFamilyGroupNew
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFamilyGroupNew : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.hasFamily())
      return BehResult.Stop;
    Actor nearbySameSpecies = this.getNearbySameSpecies(pActor, pActor.asset, pActor.current_tile);
    if (nearbySameSpecies == null)
      return BehResult.Stop;
    BehaviourActionBase<Actor>.world.families.newFamily(pActor, pActor.current_tile, nearbySameSpecies);
    return BehResult.Continue;
  }

  public Actor getNearbySameSpecies(Actor pActor, ActorAsset pUnitAsset, WorldTile pTile)
  {
    foreach (Actor nearbySameSpecies in Finder.getUnitsFromChunk(pTile, 4, (float) pUnitAsset.family_spawn_radius, true))
    {
      if (nearbySameSpecies != pActor && nearbySameSpecies.current_tile.isSameIsland(pTile) && !nearbySameSpecies.hasFamily() && nearbySameSpecies.isSameSpecies(pUnitAsset.id) && nearbySameSpecies.isSameSubspecies(pActor.subspecies))
        return nearbySameSpecies;
    }
    return (Actor) null;
  }
}
