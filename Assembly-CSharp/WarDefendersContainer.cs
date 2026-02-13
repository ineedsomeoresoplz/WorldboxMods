// Decompiled with JetBrains decompiler
// Type: WarDefendersContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class WarDefendersContainer : WarBannersContainer
{
  protected override IEnumerator showContent()
  {
    WarDefendersContainer defendersContainer = this;
    bool tHasWon = false;
    bool tHasLost = false;
    switch (defendersContainer.war.data.winner)
    {
      case WarWinner.Attackers:
        tHasLost = true;
        break;
      case WarWinner.Defenders:
        tHasWon = true;
        break;
    }
    foreach (Kingdom defender in defendersContainer.war.getDefenders())
      yield return (object) defendersContainer.showBanner(defender, pWinner: tHasWon, pLoser: tHasLost);
    foreach (Kingdom diedDefender in defendersContainer.war.getDiedDefenders())
      yield return (object) defendersContainer.showBanner(diedDefender);
    foreach (Kingdom pastDefender in defendersContainer.war.getPastDefenders())
      yield return (object) defendersContainer.showBanner(pastDefender, true);
  }
}
