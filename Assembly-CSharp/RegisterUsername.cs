// Decompiled with JetBrains decompiler
// Type: RegisterUsername
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RegisterUsername : MonoBehaviour
{
  private static bool usernameOK;
  private static bool termsOK;

  private void OnEnable()
  {
    RegisterUsername.usernameOK = false;
    RegisterUsername.termsOK = false;
  }

  public void usernameCheck(InputField pUsername) => RegisterUsername.runUsernameCheck(pUsername);

  public static async void runUsernameCheck(InputField pUsername)
  {
    RegisterUsername.clearStatus();
    RegisterUsername.blockRegisterButton();
    RegisterUsername.usernameOK = false;
    string text = pUsername.text;
    Debug.Log((object) ("Name: " + text));
    if (!Username.isValid(text))
    {
      RegisterUsername.newStatus("InvalidUsernameLong");
      RegisterUsername.blockRegisterButton();
      Debug.Log((object) "Not valid");
    }
    else
    {
      Debug.Log((object) ("Check if taken : " + text));
      try
      {
        if (await Username.isTaken(text))
        {
          RegisterUsername.newStatus("UsernameTaken");
          RegisterUsername.blockRegisterButton();
          return;
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
        RegisterUsername.newStatus(ex.Message.ToString());
        RegisterUsername.blockRegisterButton();
        return;
      }
      Debug.Log((object) "not taken?");
      RegisterUsername.usernameOK = true;
      RegisterUsername.unblockRegisterButton();
    }
  }

  public void termsCheck(bool pTermsEnabled)
  {
    RegisterUsername.termsOK = pTermsEnabled;
    RegisterUsername.unblockRegisterButton();
  }

  private static void blockRegisterButton()
  {
    if (!RegisterUsername.registerWindowExists())
      return;
    ((Component) ScrollWindow.get("register")).GetComponent<UserRegisterWindow>().blockRegister1Button();
  }

  private static void unblockRegisterButton()
  {
    if (!RegisterUsername.usernameOK || !RegisterUsername.termsOK)
    {
      RegisterUsername.blockRegisterButton();
    }
    else
    {
      if (!RegisterUsername.registerWindowExists())
        return;
      ((Component) ScrollWindow.get("register")).GetComponent<UserRegisterWindow>().unblockRegister1Button();
    }
  }

  private static void newStatus(string pMessage)
  {
    if (!RegisterUsername.registerWindowExists())
      return;
    ((Component) ScrollWindow.get("register")).GetComponent<UserRegisterWindow>().newStatus(pMessage);
  }

  private static bool registerWindowExists()
  {
    return Object.op_Inequality((Object) ScrollWindow.get("register"), (Object) null) && Object.op_Inequality((Object) ((Component) ScrollWindow.get("register")).GetComponent<UserRegisterWindow>(), (Object) null);
  }

  private static void clearStatus() => RegisterUsername.newStatus("");
}
