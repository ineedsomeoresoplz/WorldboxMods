// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehPrinterStep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehPrinterStep : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    string pResult1;
    pActor.data.get("template", out pResult1);
    int pResult2;
    pActor.data.get("step", out pResult2, -1);
    int pResult3;
    pActor.data.get("origin_x", out pResult3);
    int pResult4;
    pActor.data.get("origin_y", out pResult4);
    PrintTemplate template = PrintLibrary.getTemplate(pResult1);
    for (int index = 0; index < template.steps_per_tick; ++index)
    {
      if (pResult2 >= template.steps.Length)
      {
        pActor.data.set("step", pResult2);
        return BehResult.Stop;
      }
      PrintStep step = template.steps[pResult2];
      WorldTile tile = BehaviourActionBase<Actor>.world.GetTile(pResult3 + step.x, pResult4 + step.y);
      if (tile != null)
      {
        pActor.spawnOn(tile);
        BehPrinterStep.printTile(pActor);
      }
      ++pResult2;
    }
    pActor.data.set("step", pResult2);
    return BehResult.RestartTask;
  }

  private static void printTile(Actor pActor)
  {
    MusicBox.playSound("event:/SFX/UNIQUE/PrinterStep", pActor.current_tile);
    if (pActor.current_tile.top_type != null)
      MapAction.decreaseTile(pActor.current_tile, false);
    if (pActor.current_tile.Type.increase_to != null)
    {
      MapAction.terraformMain(pActor.current_tile, pActor.current_tile.Type.increase_to, AssetManager.terraform.get("destroy"));
      BehaviourActionBase<Actor>.world.setTileDirty(pActor.current_tile);
    }
    BehaviourActionBase<Actor>.world.conway_layer.remove(pActor.current_tile);
  }
}
