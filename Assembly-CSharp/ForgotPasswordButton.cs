// Decompiled with JetBrains decompiler
// Type: ForgotPasswordButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ForgotPasswordButton : MonoBehaviour
{
  public GameObject emailBG;
  public InputField emailInput;
  public Text statusMessage;
  public Button continueButton;
  private Button forgotPasswordButton;
  private bool checking;

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    this.newStatus("");
    ((Component) this.emailInput).gameObject.SetActive(true);
    this.emailBG.gameObject.SetActive(true);
    ((Component) this.continueButton).gameObject.SetActive(false);
    ((Component) this).gameObject.SetActive(true);
    this.forgotPasswordButton = ((Component) this).gameObject.GetComponent<Button>();
    this.checking = false;
  }

  public void resetPassword()
  {
    this.checking = true;
    this.clearStatus();
  }

  private void Update() => ((Selectable) this.forgotPasswordButton).interactable = !this.checking;

  private void newStatus(string pMessage)
  {
    Debug.Log((object) ("new status " + pMessage));
    if (LocalizedTextManager.stringExists(pMessage))
    {
      ((Component) this.statusMessage).GetComponent<LocalizedText>().key = pMessage;
      ((Component) this.statusMessage).GetComponent<LocalizedText>().updateText();
    }
    else
      this.statusMessage.text = pMessage;
  }

  private void clearStatus() => this.newStatus("");
}
