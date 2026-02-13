// Decompiled with JetBrains decompiler
// Type: BehWarriorCaptainWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehWarriorCaptainWait : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.isArmyGroupLeader())
      return BehResult.Stop;
    Army army = pActor.army;
    WorldTile currentTile = pActor.current_tile;
    int num1 = 0;
    foreach (Actor unit in army.units)
    {
      if ((double) Toolbox.SquaredDist(currentTile.posV3.x, currentTile.posV3.y, (float) unit.current_tile.x, (float) unit.current_tile.y) < 100.0)
        ++num1;
    }
    float pMinInclusive = 2f;
    float num2 = (float) num1 / (float) army.units.Count;
    if ((double) num2 < 0.20000000298023224)
      pMinInclusive = 13f;
    else if ((double) num2 < 0.40000000596046448)
      pMinInclusive = 7f;
    else if ((double) num2 < 0.60000002384185791)
      pMinInclusive = 4f;
    pActor.timer_action = Randy.randomFloat(pMinInclusive, pMinInclusive * 2f);
    return BehResult.Continue;
  }
}
