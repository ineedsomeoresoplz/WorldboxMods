// Decompiled with JetBrains decompiler
// Type: WorkshopUploadingWorldWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using RSG;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorkshopUploadingWorldWindow : MonoBehaviour
{
  public Button doneButton;
  public Image loadingImage;
  public Image doneImage;
  public Image errorImage;
  public GameObject barParent;
  public Text statusMessage;
  public Text percents;
  public Image bar;
  public Image mask;
  public static bool uploading;
  public static bool needsWorkshopAgreement;
  public GameObject workshopAgreementButton;

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    WorkshopUploadingWorldWindow.needsWorkshopAgreement = false;
    ((Component) this.errorImage).gameObject.SetActive(false);
    ((Component) this.doneButton).gameObject.SetActive(false);
    this.workshopAgreementButton.gameObject.SetActive(false);
    this.statusMessage.text = LocalizedTextManager.getText("uploading_your_world");
    ((Component) this.loadingImage).gameObject.SetActive(true);
    ((Component) this.doneImage).gameObject.SetActive(false);
    ((Component) this.bar).gameObject.SetActive(true);
    ((Component) this.percents).gameObject.SetActive(true);
    ((Component) this.mask).gameObject.SetActive(true);
    this.barParent.SetActive(true);
    ((Component) this.bar).transform.localScale = new Vector3(0.0f, 1f, 1f);
    WorkshopUploadingWorldWindow.uploading = true;
    SteamSDK.steamInitialized.Then((Func<IPromise>) (() => (IPromise) WorkshopMaps.uploadMap())).Then((Action) (() =>
    {
      this.progressBarUpdate();
      WorkshopUploadingWorldWindow.uploading = false;
      ((Component) this.doneButton).gameObject.SetActive(true);
      this.statusMessage.text = LocalizedTextManager.getText("world_uploaded");
      ((Component) this.loadingImage).gameObject.SetActive(false);
      ((Component) this.doneImage).gameObject.SetActive(true);
      if (WorkshopUploadingWorldWindow.needsWorkshopAgreement)
      {
        this.statusMessage.text = LocalizedTextManager.getText("workshop_agreement");
        this.workshopAgreementButton.SetActive(true);
      }
      else
        Application.OpenURL("steam://url/CommunityFilePage/" + WorkshopMaps.uploaded_file_id.ToString());
      this.barParent.SetActive(false);
      ((Component) this.bar).gameObject.SetActive(false);
      ((Component) this.percents).gameObject.SetActive(false);
      ((Component) this.mask).gameObject.SetActive(false);
    })).Catch((Action<Exception>) (e =>
    {
      this.statusMessage.text = $"{LocalizedTextManager.getText("upload_error")}\n( {e.Message.ToString()} )";
      WorkshopUploadingWorldWindow.uploading = false;
      Debug.LogError((object) e.Message.ToString());
      ((Component) this.doneButton).gameObject.SetActive(true);
      ((Component) this.doneImage).gameObject.SetActive(false);
      ((Component) this.loadingImage).gameObject.SetActive(false);
      ((Component) this.errorImage).gameObject.SetActive(true);
    }));
  }

  private void Update()
  {
    if (!WorkshopUploadingWorldWindow.uploading && !((Behaviour) this.percents).isActiveAndEnabled)
      return;
    this.progressBarUpdate();
  }

  private void progressBarUpdate()
  {
    float uploadProgress = WorkshopMaps.uploadProgress;
    float x = ((Component) this.bar).transform.localScale.x;
    if ((double) ((Component) this.bar).transform.localScale.x < (double) uploadProgress)
    {
      float num = ((Component) this.bar).transform.localScale.x + Time.deltaTime;
      if ((double) num > (double) uploadProgress || (double) uploadProgress > 0.75)
        num = uploadProgress;
      ((Component) this.bar).transform.localScale = new Vector3(num, 1f, 1f);
      this.percents.text = Mathf.CeilToInt(num * 100f).ToString() + " %";
    }
    else
      this.percents.text = Mathf.CeilToInt(uploadProgress * 100f).ToString() + " %";
  }
}
