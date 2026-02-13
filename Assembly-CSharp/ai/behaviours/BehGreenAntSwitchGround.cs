// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehGreenAntSwitchGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehGreenAntSwitchGround : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    int pResult;
    pActor.data.get("direction", out pResult);
    string pType;
    int pData;
    if (pActor.beh_tile_target.Type.liquid)
    {
      pType = "sand";
      pData = pResult - 1;
    }
    else if (pActor.beh_tile_target.Type.IsType("sand"))
    {
      pType = "soil_low";
      pData = pResult + 1;
    }
    else if (pActor.beh_tile_target.Type.IsType("soil_low"))
    {
      pType = "soil_high";
      pData = pResult - 1;
    }
    else if (pActor.beh_tile_target.Type.IsType("soil_high"))
    {
      pType = "soil_low";
      pData = pResult + 1;
    }
    else
    {
      pType = "sand";
      pData = pResult - 1;
    }
    if (pData > Toolbox.directions.Length - 1)
      pData = 0;
    if (pData < 0)
      pData = Toolbox.directions.Length - 1;
    Ant.antUseOnTile(pActor.beh_tile_target, pType);
    pActor.data.set("direction", pData);
    return BehResult.Continue;
  }
}
