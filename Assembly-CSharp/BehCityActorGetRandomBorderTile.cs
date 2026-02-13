// Decompiled with JetBrains decompiler
// Type: BehCityActorGetRandomBorderTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCityActorGetRandomBorderTile : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.city.hasZones() || pActor.city.border_zones.Count == 0)
      return BehResult.Stop;
    WorldTile random = pActor.city.border_zones.GetRandom<TileZone>().tiles.GetRandom<WorldTile>();
    if (!random.Type.ground)
      return BehResult.Stop;
    pActor.beh_tile_target = random;
    return BehResult.Continue;
  }
}
