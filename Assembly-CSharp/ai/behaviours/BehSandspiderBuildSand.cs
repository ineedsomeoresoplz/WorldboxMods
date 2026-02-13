// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehSandspiderBuildSand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehSandspiderBuildSand : BehGoToTileTarget
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (!pActor.beh_tile_target.Type.IsType("sand"))
    {
      int pResult;
      pActor.data.get("ant_steps", out pResult);
      Ant.antUseOnTile(pActor.beh_tile_target, "sand");
      int num;
      pActor.data.set("ant_steps", num = pResult + 1);
      pActor.data.removeBool("changed_direction");
    }
    else if (Randy.randomChance(0.1f))
    {
      int pResult;
      pActor.data.get("ant_steps", out pResult);
      int num;
      pActor.data.set("ant_steps", num = pResult + 1);
      pActor.data.removeBool("changed_direction");
    }
    return BehResult.Continue;
  }
}
