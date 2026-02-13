// Decompiled with JetBrains decompiler
// Type: BackButtonHider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
internal class BackButtonHider : MonoBehaviour
{
  private void OnEnable()
  {
    if (WindowHistory.hasHistory())
      ((Component) this).gameObject.SetActive(true);
    else
      ((Component) this).gameObject.SetActive(false);
  }
}
