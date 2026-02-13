// Decompiled with JetBrains decompiler
// Type: LoadingScreenOut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LoadingScreenOut : MonoBehaviour
{
  public CanvasGroup canvasGroup;

  private void Update()
  {
    this.canvasGroup.alpha -= Time.deltaTime * 2f;
    if ((double) this.canvasGroup.alpha > 0.0)
      return;
    ((Component) this).gameObject.SetActive(false);
  }
}
