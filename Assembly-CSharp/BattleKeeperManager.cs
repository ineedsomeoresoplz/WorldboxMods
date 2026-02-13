// Decompiled with JetBrains decompiler
// Type: BattleKeeperManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BattleKeeperManager
{
  private const int MAX_FRAMES = 8;
  private static HashSet<BattleContainer> _hashset;
  private static readonly List<BattleContainer> _to_remove = new List<BattleContainer>();

  public static void clear()
  {
    if (BattleKeeperManager._hashset == null)
      BattleKeeperManager._hashset = new HashSet<BattleContainer>();
    BattleKeeperManager._hashset.Clear();
    BattleKeeperManager._to_remove.Clear();
  }

  public static HashSet<BattleContainer> get() => BattleKeeperManager._hashset;

  public static void update(float pElapsed)
  {
    if (BattleKeeperManager._hashset.Count == 0)
      return;
    foreach (BattleContainer battleContainer in BattleKeeperManager._hashset)
    {
      if ((double) battleContainer.timer > 1.0)
      {
        battleContainer.timer -= pElapsed;
        battleContainer.timer = Mathf.Clamp(battleContainer.timer, 1f, battleContainer.timer);
      }
      if (battleContainer.isRendered())
      {
        if ((double) battleContainer.timer_animation > 0.0)
        {
          battleContainer.timer_animation -= pElapsed;
        }
        else
        {
          battleContainer.timer_animation = 0.04f;
          ++battleContainer.frame;
          if (battleContainer.frame >= 8)
            battleContainer.frame = 7;
        }
      }
      if ((double) battleContainer.timeout > 0.0)
      {
        battleContainer.timeout -= pElapsed;
      }
      else
      {
        battleContainer.timer -= pElapsed;
        if ((double) battleContainer.timer <= 0.0)
          BattleKeeperManager._to_remove.Add(battleContainer);
      }
    }
    if (BattleKeeperManager._to_remove.Count <= 0)
      return;
    foreach (BattleContainer battleContainer in BattleKeeperManager._to_remove)
      BattleKeeperManager._hashset.Remove(battleContainer);
    BattleKeeperManager._to_remove.Clear();
  }

  public static void addUnitKilled(Actor pActor)
  {
    BattleContainer battleContainer1 = (BattleContainer) null;
    foreach (BattleContainer battleContainer2 in BattleKeeperManager._hashset)
    {
      if ((double) Toolbox.SquaredDistTile(battleContainer2.tile, pActor.current_tile) < 1600.0)
      {
        battleContainer1 = battleContainer2;
        break;
      }
    }
    if (battleContainer1 == null && !pActor.isSapient())
      return;
    if (battleContainer1 == null)
    {
      battleContainer1 = new BattleContainer();
      battleContainer1.tile = pActor.current_tile;
      BattleKeeperManager._hashset.Add(battleContainer1);
    }
    battleContainer1.increaseDeaths(pActor);
    if (battleContainer1.tile != pActor.current_tile && ((double) Toolbox.SquaredDistTile(battleContainer1.tile, pActor.current_tile) < 25.0 || battleContainer1.getDeathsTotal() < 3))
      battleContainer1.tile = pActor.current_tile;
    battleContainer1.timer = 1.2f;
    battleContainer1.timeout = 1f;
    if (battleContainer1.frame < 7)
      return;
    battleContainer1.frame = 0;
  }
}
