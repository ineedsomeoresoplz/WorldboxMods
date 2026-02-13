// Decompiled with JetBrains decompiler
// Type: UserRegisterWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UserRegisterWindow : MonoBehaviour
{
  public GameObject page1;
  public GameObject page2;
  public GameObject successPage;
  public GameObject creationPage;
  public Button usernameCheckButton;
  public Button emailCheckButton;
  public InputField inputTextUsername;
  public InputField inputTextEmail;
  public InputField inputTextPassword;
  public Text textMessage;
  private static string _email = "";
  private static string _password = "";
  private static string _username = "";

  public void Start() => this.checkState();

  private void OnEnable() => this.checkState();

  public void RegisterNewAccount(string username, string password, string email)
  {
    UserRegisterWindow._username = username;
    UserRegisterWindow._password = password;
    UserRegisterWindow._email = email;
  }

  public void registerAccountCallback(string errorReason) => Config.lockGameControls = false;

  public void checkState()
  {
    Debug.Log((object) "Check Register Window State");
    if (Auth.isLoggedIn)
    {
      this.setSuccess();
    }
    else
    {
      this.setPage1();
      this.blockRegister1Button();
      this.blockRegister2Button();
    }
  }

  public void setSuccess()
  {
    Config.lockGameControls = false;
    this.page2.SetActive(false);
    this.page1.SetActive(false);
    this.creationPage.SetActive(false);
    this.successPage.SetActive(true);
  }

  public void setPage2()
  {
    Config.lockGameControls = false;
    this.page1.SetActive(false);
    this.successPage.SetActive(false);
    this.creationPage.SetActive(false);
    this.page2.SetActive(true);
  }

  public void setPage1()
  {
    Config.lockGameControls = false;
    this.page2.SetActive(false);
    this.successPage.SetActive(false);
    this.creationPage.SetActive(false);
    this.page1.SetActive(true);
    if (string.IsNullOrEmpty(this.inputTextUsername?.text))
      return;
    RegisterUsername.runUsernameCheck(this.inputTextUsername);
  }

  public void setCreation()
  {
    Config.lockGameControls = true;
    this.page1.SetActive(false);
    this.page2.SetActive(false);
    this.successPage.SetActive(false);
    this.creationPage.SetActive(true);
  }

  public void blockRegister1Button()
  {
    ((Component) this.usernameCheckButton).GetComponent<CanvasGroup>().alpha = 0.2f;
    ((Selectable) this.usernameCheckButton).interactable = false;
  }

  public void unblockRegister1Button()
  {
    ((Component) this.usernameCheckButton).GetComponent<CanvasGroup>().alpha = 1f;
    ((Selectable) this.usernameCheckButton).interactable = true;
  }

  public void blockRegister2Button()
  {
    ((Component) this.emailCheckButton).GetComponent<CanvasGroup>().alpha = 0.2f;
    ((Selectable) this.emailCheckButton).interactable = false;
  }

  public void unblockRegister2Button()
  {
    ((Component) this.emailCheckButton).GetComponent<CanvasGroup>().alpha = 1f;
    ((Selectable) this.emailCheckButton).interactable = true;
  }

  public void newStatus(string pMessage)
  {
    Debug.Log((object) ("new status " + pMessage));
    if (LocalizedTextManager.stringExists(pMessage))
    {
      ((Component) this.textMessage).GetComponent<LocalizedText>().key = pMessage;
      ((Component) this.textMessage).GetComponent<LocalizedText>().updateText();
    }
    else
      this.textMessage.text = pMessage;
  }

  public void clearStatus() => this.newStatus("");

  public void blockRegisterButton()
  {
  }
}
