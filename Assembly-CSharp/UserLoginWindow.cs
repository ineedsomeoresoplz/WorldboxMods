// Decompiled with JetBrains decompiler
// Type: UserLoginWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UserLoginWindow : MonoBehaviour
{
  public GameObject groupLogged;
  public GameObject groupLogin;
  public GameObject groupLoading;
  public Text usernameText;
  public Text windowTitle;
  public InputField inputTextUser;
  public InputField inputTextPassword;
  public Text textMessage;
  private bool isLoggedIn;

  public void Start()
  {
    this.checkState();
    if (!(PlayerConfig.dict["username"].stringVal != ""))
      return;
    this.inputTextUser.text = PlayerConfig.dict["username"].stringVal;
  }

  public void checkState()
  {
    Debug.Log((object) "Check Login Window State");
    if (Auth.isLoggedIn)
    {
      if (Auth.displayName != "" && Auth.displayName != null)
      {
        Debug.Log((object) "displayName found");
        this.usernameText.text = Auth.displayName;
      }
      else if (Auth.userName != "" && Auth.userName != null)
      {
        Debug.Log((object) "userName found");
        this.usernameText.text = Auth.userName;
      }
      else
      {
        Debug.Log((object) "emailAddress found");
        this.usernameText.text = Auth.emailAddress;
      }
      this.setLogout();
    }
    else
      this.setLogin();
    this.isLoggedIn = Auth.isLoggedIn;
  }

  public void Update()
  {
    if (this.isLoggedIn == Auth.isLoggedIn)
      return;
    this.checkState();
  }

  public void setLoading()
  {
    ((Component) this.windowTitle).GetComponent<LocalizedText>().key = "logging_in";
    ((Component) this.windowTitle).GetComponent<LocalizedText>().updateText();
    this.groupLogin.SetActive(false);
    this.groupLogged.SetActive(false);
    this.groupLoading.SetActive(true);
  }

  public void setLogin()
  {
    ((Component) this.windowTitle).GetComponent<LocalizedText>().key = "Login";
    ((Component) this.windowTitle).GetComponent<LocalizedText>().updateText();
    this.groupLogged.SetActive(false);
    this.groupLoading.SetActive(false);
    this.groupLogin.SetActive(true);
  }

  public void setLogout()
  {
    ((Component) this.windowTitle).GetComponent<LocalizedText>().key = "welcome_worldnet";
    ((Component) this.windowTitle).GetComponent<LocalizedText>().updateText();
    this.groupLogin.SetActive(false);
    this.groupLoading.SetActive(false);
    this.groupLogged.SetActive(true);
  }

  public void clearWindow(string pMessage = "...")
  {
    this.textMessage.text = pMessage;
    this.inputTextPassword.text = "";
    this.inputTextUser.text = "";
  }

  public void clearCredentials()
  {
    this.inputTextPassword.text = "";
    this.inputTextUser.text = "";
    if (!(PlayerConfig.dict["username"].stringVal != ""))
      return;
    this.inputTextUser.text = PlayerConfig.dict["username"].stringVal;
  }
}
