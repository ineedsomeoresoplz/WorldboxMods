// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBlackAntBuildSand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBlackAntBuildSand : BehaviourActionActor
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
    if (pResult1 > 0)
    {
      --pResult1;
      if (!pActor.beh_tile_target.Type.IsType("mountains") && !pActor.beh_tile_target.Type.IsType("hills"))
        Ant.antUseOnTile(pActor.beh_tile_target, "sand");
      pResult2 = BehBlackAntBuildSand.getRandomDirection();
    }
    pActor.data.set("ant_steps", pResult1);
    pActor.data.set("direction", pResult2);
    if (pResult1 != 0)
      return BehResult.Continue;
    pActor.setTask("ant_black_island");
    return BehResult.Stop;
  }

  private static int getRandomDirection()
  {
    ActorDirection random = Randy.getRandom<ActorDirection>(Toolbox.directions);
    return Toolbox.directions.IndexOf<ActorDirection>(random);
  }
}
