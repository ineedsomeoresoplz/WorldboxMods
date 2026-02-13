// Decompiled with JetBrains decompiler
// Type: TesterBehSetResolution
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehSetResolution : BehaviourActionTester
{
  private int width;
  private int height;
  private string name;

  public TesterBehSetResolution(int pWidth, int pHeight, string pName = null)
  {
    this.width = pWidth;
    this.height = pHeight;
    this.name = pName;
  }

  public override BehResult execute(AutoTesterBot pObject) => BehResult.Continue;
}
