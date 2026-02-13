// Decompiled with JetBrains decompiler
// Type: TesterBehLoadWorld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehLoadWorld : BehaviourActionTester
{
  private int slot;
  private string fallback;

  public TesterBehLoadWorld(int pSlot, string pFallback = null)
  {
    this.slot = pSlot;
    this.fallback = pFallback;
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    SaveManager.setCurrentSlot(this.slot);
    if (!SaveManager.currentSlotExists())
      SaveManager.loadMapFromResources(this.fallback);
    else
      BehaviourActionBase<AutoTesterBot>.world.save_manager.startLoadSlot();
    return BehResult.Continue;
  }
}
