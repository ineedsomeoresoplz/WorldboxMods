// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBlackAntBuildIsland
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBlackAntBuildIsland : BehaviourActionActor
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
    if (pActor.beh_tile_target.Type.liquid)
      pResult1 = 20;
    if (pResult1 > 0)
    {
      string pType;
      if (!pActor.beh_tile_target.Type.IsType("mountains"))
      {
        pType = "mountains";
        ++pResult2;
        if (pResult2 > Toolbox.directions.Length - 1)
          pResult2 = 0;
      }
      else
      {
        pType = "hills";
        --pResult2;
        if (pResult2 < 0)
          pResult2 = Toolbox.directions.Length - 1;
      }
      Ant.antUseOnTile(pActor.beh_tile_target, pType);
      --pResult1;
    }
    if (pResult1 == 0)
    {
      pActor.data.set("ant_steps", 40);
      pActor.data.set("direction", BehBlackAntBuildIsland.getRandomDirection());
      pActor.setTask("ant_black_sand");
      return BehResult.Stop;
    }
    pActor.data.set("ant_steps", pResult1);
    pActor.data.set("direction", pResult2);
    return BehResult.Continue;
  }

  private static int getRandomDirection()
  {
    ActorDirection random = Randy.getRandom<ActorDirection>(Toolbox.directions);
    return Toolbox.directions.IndexOf<ActorDirection>(random);
  }
}
