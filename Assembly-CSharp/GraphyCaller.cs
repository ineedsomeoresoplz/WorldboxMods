// Decompiled with JetBrains decompiler
// Type: GraphyCaller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GraphyCaller : MonoBehaviour
{
  private int clicked;

  public void click()
  {
    ++this.clicked;
    if (this.clicked <= 10)
      return;
    bool flag = DebugConfig.isOn(DebugOption.DebugButton);
    DebugConfig.setOption(DebugOption.DebugButton, !flag);
    DebugConfig.instance.debugButton.SetActive(!flag);
  }

  public void clickConsole()
  {
    ++this.clicked;
    if (this.clicked <= 10)
      return;
    World.world.console.Show();
  }

  private void OnEnable() => this.clicked = 0;
}
