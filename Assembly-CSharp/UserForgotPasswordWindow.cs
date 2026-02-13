// Decompiled with JetBrains decompiler
// Type: UserForgotPasswordWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UserForgotPasswordWindow : MonoBehaviour
{
  public Button forgotPasswordButton;

  public void Start()
  {
    if (!Config.game_loaded)
      return;
    ((Component) this.forgotPasswordButton).gameObject.SetActive(true);
  }

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    ((Component) this.forgotPasswordButton).gameObject.SetActive(true);
  }
}
