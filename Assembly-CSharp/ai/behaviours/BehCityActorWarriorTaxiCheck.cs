// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorWarriorTaxiCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using life.taxi;

#nullable disable
namespace ai.behaviours;

public class BehCityActorWarriorTaxiCheck : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.current_tile.hasCity() && pActor.current_tile.zone_city.kingdom.isEnemy(pActor.kingdom) || !pActor.city.hasAttackZoneOrder() || pActor.city.target_attack_zone.centerTile.isSameIsland(pActor.current_tile))
      return BehResult.Stop;
    TaxiManager.newRequest(pActor, pActor.city.target_attack_zone.centerTile);
    return BehResult.Continue;
  }
}
