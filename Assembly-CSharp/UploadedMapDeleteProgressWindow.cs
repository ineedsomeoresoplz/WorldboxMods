// Decompiled with JetBrains decompiler
// Type: UploadedMapDeleteProgressWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UploadedMapDeleteProgressWindow : MonoBehaviour
{
  public GameObject deletingOverlay;

  private void OnEnable() => this.deletingOverlay.SetActive(false);

  public void confirmDeletion() => this.deletingOverlay.SetActive(true);
}
