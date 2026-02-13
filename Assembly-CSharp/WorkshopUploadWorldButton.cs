// Decompiled with JetBrains decompiler
// Type: WorkshopUploadWorldButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class WorkshopUploadWorldButton : MonoBehaviour
{
  public Text title;
  public Text description;
  public GameObject quickError;
  public Text errorMessage;

  private void Start()
  {
    Button button;
    if (!((Component) this).TryGetComponent<Button>(ref button))
      return;
    // ISSUE: method pointer
    ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(uploadWorldToWorkshop)));
  }

  private void OnEnable() => this.quickError.SetActive(false);

  private void uploadWorldToWorkshop()
  {
    this.quickError.SetActive(false);
    if (string.IsNullOrWhiteSpace(this.title.text))
    {
      this.errorMessage.text = "Give your world a name!";
      this.quickError.SetActive(true);
    }
    else if (string.IsNullOrWhiteSpace(this.description.text))
    {
      this.errorMessage.text = "Give your world a description!";
      this.quickError.SetActive(true);
    }
    else
      ScrollWindow.showWindow("steam_workshop_uploading");
  }

  public void closeError() => this.quickError.SetActive(false);
}
