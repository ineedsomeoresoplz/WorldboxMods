// Decompiled with JetBrains decompiler
// Type: UploadedMapReportWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UploadedMapReportWindow : MonoBehaviour
{
  public GameObject reportOverlay;
  public GameObject reportButtons;
  public GameObject reportConfirmation;
  private string reportReason = "";

  private void OnEnable()
  {
    this.reportOverlay.SetActive(false);
    this.reportButtons.SetActive(true);
    this.reportConfirmation.SetActive(false);
  }

  public void reportNSFW()
  {
    this.reportReason = "nsfw";
    this.confirmReport();
  }

  public void reportCrash()
  {
    this.reportReason = "crash";
    this.confirmReport();
  }

  public void reportBroken()
  {
    this.reportReason = "broken";
    this.confirmReport();
  }

  public void confirmReport()
  {
    this.reportButtons.SetActive(false);
    this.reportOverlay.SetActive(true);
    string reportReason = this.reportReason;
  }
}
