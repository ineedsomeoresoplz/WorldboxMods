// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehAntSwitchGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehAntSwitchGround : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    int pResult1;
    pActor.data.get("ant_steps", out pResult1);
    int pResult2;
    pActor.data.get("direction", out pResult2);
    string pResult3;
    pActor.data.get("tile_type1", out pResult3);
    string pResult4;
    pActor.data.get("tile_type2", out pResult4);
    int pData;
    if (pActor.beh_tile_target.Type.IsType(pResult4))
    {
      Ant.antUseOnTile(pActor.beh_tile_target, pResult3);
      int num = pResult1;
      pData = num + 1;
      if (num > 3)
      {
        ++pResult2;
        if (pResult2 > Toolbox.directions.Length - 1)
          pResult2 = 0;
        pData = 0;
      }
    }
    else
    {
      Ant.antUseOnTile(pActor.beh_tile_target, pResult4);
      int num = pResult1;
      pData = num + 1;
      if (num > 3)
      {
        --pResult2;
        if (pResult2 < 0)
          pResult2 = Toolbox.directions.Length - 1;
        pData = 0;
      }
    }
    pActor.data.set("ant_steps", pData);
    pActor.data.set("direction", pResult2);
    return BehResult.Continue;
  }
}
