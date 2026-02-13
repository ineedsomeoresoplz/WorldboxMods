// Decompiled with JetBrains decompiler
// Type: WarAttackersContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class WarAttackersContainer : WarBannersContainer
{
  protected override IEnumerator showContent()
  {
    WarAttackersContainer attackersContainer = this;
    bool tHasWon = false;
    bool tHasLost = false;
    switch (attackersContainer.war.data.winner)
    {
      case WarWinner.Attackers:
        tHasWon = true;
        break;
      case WarWinner.Defenders:
        tHasLost = true;
        break;
    }
    foreach (Kingdom attacker in attackersContainer.war.getAttackers())
      yield return (object) attackersContainer.showBanner(attacker, pWinner: tHasWon, pLoser: tHasLost);
    foreach (Kingdom diedAttacker in attackersContainer.war.getDiedAttackers())
      yield return (object) attackersContainer.showBanner(diedAttacker);
    foreach (Kingdom pastAttacker in attackersContainer.war.getPastAttackers())
      yield return (object) attackersContainer.showBanner(pastAttacker, true);
  }
}
