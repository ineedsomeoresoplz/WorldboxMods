// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehVerifierAttackZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehVerifierAttackZone : BehCitizenActionCity
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.city == null)
      return BehResult.Stop;
    TileZone targetAttackZone = pActor.city.target_attack_zone;
    if (!pActor.city.hasAttackZoneOrder())
      return BehResult.Stop;
    City city = pActor.city.target_attack_zone.city;
    return city == null || targetAttackZone == null || !pActor.kingdom.isEnemy(city.kingdom) ? BehResult.Stop : BehResult.Continue;
  }
}
