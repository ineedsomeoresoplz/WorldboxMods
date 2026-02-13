// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehSandspiderCheckSand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehSandspiderCheckSand : BehaviourActionActor
{
  private static List<int> _list_directions = new List<int>(3);

  public override BehResult execute(Actor pActor)
  {
    WorldTile behTileTarget = pActor.beh_tile_target;
    if (behTileTarget == null)
      return BehResult.Continue;
    bool pResult1;
    pActor.data.get("changed_direction", out pResult1);
    if (pResult1 || !behTileTarget.Type.IsType("sand"))
      return BehResult.Continue;
    int pResult2;
    pActor.data.get("direction", out pResult2);
    int newDirectionIndex = BehSandspiderCheckSand.getNewDirectionIndex(pResult2);
    pActor.data.set("direction", newDirectionIndex);
    pActor.data.set("changed_direction", true);
    return BehResult.RestartTask;
  }

  private static int getNewDirectionIndex(int pOldIndex)
  {
    BehSandspiderCheckSand._list_directions.Clear();
    for (int index = 0; index < Toolbox.directions.Length; ++index)
    {
      if (index != pOldIndex)
        BehSandspiderCheckSand._list_directions.Add(index);
    }
    return BehSandspiderCheckSand._list_directions.GetRandom<int>();
  }
}
