// Decompiled with JetBrains decompiler
// Type: RegisterButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RegisterButton : MonoBehaviour
{
  public UserRegisterWindow userRegisterWindow;

  public void usernameCheck()
  {
    Debug.Log((object) ("Name:  " + this.userRegisterWindow.inputTextUsername.text));
    this.userRegisterWindow.setPage2();
  }

  public void tryRegister()
  {
    this.clearStatus();
    Debug.Log((object) ("Name:  " + this.userRegisterWindow.inputTextUsername.text));
    Debug.Log((object) ("Email: " + this.userRegisterWindow.inputTextEmail.text));
    string text1 = this.userRegisterWindow.inputTextUsername.text;
    string text2 = this.userRegisterWindow.inputTextEmail.text;
    string text3 = this.userRegisterWindow.inputTextPassword.text;
    if (text2 == "" || text3 == "")
      this.newStatus("EmailPasswordEmpty");
    else if (!Auth.isValidEmail(text2))
      this.newStatus("InvalidEmail");
    else if (text3.Length < 6)
      this.newStatus("ShortPassword");
    else
      this.userRegisterWindow.RegisterNewAccount(text1, text3, text2);
  }

  private void sendVerification() => Debug.Log((object) "send verification");

  private void newStatus(string pMessage)
  {
    Debug.Log((object) ("new status " + pMessage));
    if (LocalizedTextManager.stringExists(pMessage))
    {
      ((Component) this.userRegisterWindow.textMessage).GetComponent<LocalizedText>().key = pMessage;
      ((Component) this.userRegisterWindow.textMessage).GetComponent<LocalizedText>().updateText();
    }
    else
      this.userRegisterWindow.textMessage.text = pMessage;
  }

  private void clearStatus() => this.newStatus("");
}
