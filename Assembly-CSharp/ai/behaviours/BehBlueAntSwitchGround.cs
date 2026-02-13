// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBlueAntSwitchGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBlueAntSwitchGround : BehaviourActionActor
{
  private const string tileType1 = "sand";
  private const string tileType2 = "shallow_waters";

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    int pResult;
    pActor.data.get("direction", out pResult);
    int pData;
    if (pActor.beh_tile_target.Type.liquid)
    {
      pData = pResult + 1;
      if (pData > Toolbox.directions.Length - 1)
        pData = 0;
      Ant.antUseOnTile(pActor.beh_tile_target, "sand");
    }
    else
    {
      pData = pResult - 1;
      if (pData < 0)
        pData = Toolbox.directions.Length - 1;
      Ant.antUseOnTile(pActor.beh_tile_target, "shallow_waters");
    }
    pActor.data.set("direction", pData);
    return BehResult.Continue;
  }
}
