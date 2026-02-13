// Decompiled with JetBrains decompiler
// Type: BehClanChiefCheckMembersToBanish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehClanChiefCheckMembersToBanish : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    Clan clan = pActor.clan;
    for (int index = 0; index < clan.units.Count; ++index)
    {
      Actor unit = clan.units[index];
      if (unit != pActor && pActor.areFoes((BaseSimObject) unit))
        unit.setClan((Clan) null);
    }
    return BehResult.Continue;
  }
}
