// Decompiled with JetBrains decompiler
// Type: SignButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SignButton : MonoBehaviour
{
  public UserLoginWindow userLoginWindow;
  public InputField textName;
  public InputField textPassword;
  public Text textStatusMessage;
  private string loginEmail;
  private string loginPassword;
  private string loginUsername;

  public void tryLogin()
  {
    this.clearStatus();
    this.loginEmail = this.textName.text;
    this.loginPassword = this.textPassword.text;
    this.loginUsername = "";
    if (this.loginEmail == "" || this.loginPassword == "")
      this.errorStatus("EmailPasswordEmpty");
    else if (!Auth.isValidEmail(this.loginEmail))
    {
      this.loginUsername = this.textName.text;
      this.loginEmail = "";
      if (!Username.isValid(this.loginUsername))
      {
        this.errorStatus("InvalidUsername");
      }
      else
      {
        PlayerConfig.dict["username"].stringVal = this.loginUsername;
        PlayerConfig.saveData();
        Login.GetEmailForUsername(this.loginUsername, this.loginPassword, new Action<string, string>(this.emailLoginCallback));
        this.userLoginWindow.setLoading();
      }
    }
    else
    {
      this.userLoginWindow.setLoading();
      this.continueLogin();
    }
  }

  public void continueLogin()
  {
  }

  public void emailLoginCallback(string returnedEmail, string errorReason)
  {
    if (errorReason != "")
    {
      this.userLoginWindow.setLogin();
      this.errorStatus(errorReason);
    }
    else
    {
      this.loginEmail = returnedEmail;
      this.continueLogin();
    }
  }

  private void errorStatus(string pMessage)
  {
    if (LocalizedTextManager.stringExists(pMessage))
    {
      ((Component) this.textStatusMessage).GetComponent<LocalizedText>().key = pMessage;
      ((Component) this.textStatusMessage).GetComponent<LocalizedText>().updateText();
    }
    else
      this.textStatusMessage.text = pMessage;
    ((Graphic) this.textStatusMessage).color = Toolbox.makeColor("#FF8686");
  }

  private void goodStatus(string pMessage)
  {
    if (LocalizedTextManager.stringExists(pMessage))
    {
      ((Component) this.textStatusMessage).GetComponent<LocalizedText>().key = pMessage;
      ((Component) this.textStatusMessage).GetComponent<LocalizedText>().updateText();
    }
    else
      this.textStatusMessage.text = pMessage;
    ((Graphic) this.textStatusMessage).color = Toolbox.makeColor("#95DD5D");
  }

  private void clearStatus() => this.textStatusMessage.text = "";
}
