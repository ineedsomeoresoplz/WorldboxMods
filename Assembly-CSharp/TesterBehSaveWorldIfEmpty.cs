// Decompiled with JetBrains decompiler
// Type: TesterBehSaveWorldIfEmpty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehSaveWorldIfEmpty : BehaviourActionTester
{
  private int slot;

  public TesterBehSaveWorldIfEmpty(int pSlot) => this.slot = pSlot;

  public override BehResult execute(AutoTesterBot pObject)
  {
    SaveManager.setCurrentSlot(this.slot);
    if (!SaveManager.currentSlotExists())
      SaveManager.saveWorldToDirectory(SaveManager.currentSavePath);
    return BehResult.Continue;
  }
}
