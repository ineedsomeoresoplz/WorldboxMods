// Decompiled with JetBrains decompiler
// Type: RegisterDetails
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RegisterDetails : MonoBehaviour
{
  private static bool emailValid;
  private static bool passwordValid;

  private void OnEnable() => RegisterDetails.checkButton();

  public void emailCheck(InputField pEmail) => RegisterDetails.runEmailCheck(pEmail);

  public static void runEmailCheck(InputField pEmail)
  {
    string text = pEmail.text;
    RegisterDetails.emailValid = false;
    Debug.Log((object) ("Name: " + text));
    if (!Auth.isValidEmail(text))
    {
      RegisterDetails.newStatus("InvalidEmail");
      RegisterDetails.checkButton();
      Debug.Log((object) "Not valid");
    }
    else
    {
      RegisterDetails.clearStatus();
      RegisterDetails.emailValid = true;
      RegisterDetails.checkButton();
    }
  }

  public void passwordCheck(InputField pEmail) => RegisterDetails.runPasswordCheck(pEmail);

  public static void runPasswordCheck(InputField pPassword)
  {
    string text = pPassword.text;
    RegisterDetails.passwordValid = false;
    Debug.Log((object) ("Pass: " + text));
    if (text.Length < 6)
    {
      RegisterDetails.newStatus("ShortPassword");
      RegisterDetails.checkButton();
      Debug.Log((object) "Not valid");
    }
    else
    {
      RegisterDetails.clearStatus();
      RegisterDetails.passwordValid = true;
      RegisterDetails.checkButton();
    }
  }

  private static void checkButton()
  {
    if (RegisterDetails.emailValid && RegisterDetails.passwordValid)
      RegisterDetails.unblockRegisterButton();
    else
      RegisterDetails.blockRegisterButton();
  }

  private static void blockRegisterButton()
  {
    if (!RegisterDetails.registerWindowExists())
      return;
    ((Component) ScrollWindow.get("register")).GetComponent<UserRegisterWindow>().blockRegister2Button();
  }

  private static void unblockRegisterButton()
  {
    if (!RegisterDetails.registerWindowExists())
      return;
    ((Component) ScrollWindow.get("register")).GetComponent<UserRegisterWindow>().unblockRegister2Button();
  }

  private static void newStatus(string pMessage)
  {
    if (!RegisterDetails.registerWindowExists())
      return;
    ((Component) ScrollWindow.get("register")).GetComponent<UserRegisterWindow>().newStatus(pMessage);
  }

  private static bool registerWindowExists()
  {
    return Object.op_Inequality((Object) ScrollWindow.get("register"), (Object) null) && Object.op_Inequality((Object) ((Component) ScrollWindow.get("register")).GetComponent<UserRegisterWindow>(), (Object) null);
  }

  private static void clearStatus() => RegisterDetails.newStatus("");
}
