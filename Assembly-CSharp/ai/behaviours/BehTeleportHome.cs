// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehTeleportHome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehTeleportHome : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasCity())
      return BehResult.Stop;
    City city = pActor.getCity();
    if (!city.hasZones())
      return BehResult.Stop;
    TileZone random1 = city.zones.GetRandom<TileZone>();
    if (random1 == null)
      return BehResult.Stop;
    WorldTile random2 = random1.tiles.GetRandom<WorldTile>();
    ActionLibrary.teleportEffect(pActor, random2);
    pActor.cancelAllBeh();
    pActor.spawnOn(random2);
    pActor.doCastAnimation();
    return BehResult.Continue;
  }
}
