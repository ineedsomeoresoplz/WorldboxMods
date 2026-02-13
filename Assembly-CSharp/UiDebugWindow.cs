// Decompiled with JetBrains decompiler
// Type: UiDebugWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UiDebugWindow : TabbedWindow
{
  public Mask mask;
  public ActorDebugAssetsComponent assets_actor;
  public BuildingDebugAssetsComponent assets_building;

  private void OnEnable() => this.showInfo();

  public void showInfo()
  {
    ((Behaviour) this.mask).enabled = true;
    AchievementLibrary.god_mode.check();
    this.clear();
  }

  private void clear()
  {
  }

  public void clickConsole()
  {
    if (!Config.game_loaded)
      return;
    World.world.console.Show();
  }

  public void clickNewDebugWindow() => DebugConfig.createTool("Game Info");

  public void clickNewDebugWindowBench() => DebugConfig.createTool("Benchmark All");

  public void clickRandomKingdomColor()
  {
    AssetsDebugManager.newKingdomColors();
    if (((Component) this.assets_actor).gameObject.activeSelf)
      this.assets_actor.refresh();
    if (!((Component) this.assets_building).gameObject.activeSelf)
      return;
    this.assets_building.refresh();
  }

  public void clickRandomSkinColor()
  {
    AssetsDebugManager.newSkinColors();
    this.assets_actor.refresh();
  }

  public void clickChangeSex()
  {
    AssetsDebugManager.changeSex();
    this.assets_actor.refresh();
  }

  public void showDebugAvatarsWindow() => ScrollWindow.showWindow("debug_avatars");
}
