// Decompiled with JetBrains decompiler
// Type: ErrorWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ErrorWindow : MonoBehaviour
{
  public Text errorText;
  public static string errorMessage;

  private void OnEnable() => this.errorText.text = ErrorWindow.errorMessage;
}
