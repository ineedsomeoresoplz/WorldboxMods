// Decompiled with JetBrains decompiler
// Type: WindowBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WindowBackground : MonoBehaviour
{
  private CanvasGroup group;

  private void Start() => this.group = ((Component) this).GetComponent<CanvasGroup>();

  private void Update()
  {
    if (ScrollWindow.isWindowActive() && (double) this.group.alpha < 1.0)
    {
      this.group.alpha += Time.deltaTime * 5f;
    }
    else
    {
      if (ScrollWindow.isWindowActive() || (double) this.group.alpha <= 0.0)
        return;
      this.group.alpha -= Time.deltaTime * 5f;
    }
  }
}
