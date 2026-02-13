// Decompiled with JetBrains decompiler
// Type: RewardWindowController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RewardWindowController : MonoBehaviour
{
  public GameObject watchVideoButton;
  public GameObject waitTimeElement;
  public Text textElement;

  private void Update()
  {
    double pTime = PlayerConfig.instance.data.nextAdTimestamp - Epoch.Current();
    if (Config.isEditor && Config.editor_test_rewards_from_ads)
    {
      PlayerConfig.instance.data.nextAdTimestamp = -1.0;
      pTime = 0.0;
    }
    if (pTime > 0.0)
    {
      this.watchVideoButton.SetActive(false);
      this.waitTimeElement.SetActive(true);
      this.textElement.text = Toolbox.formatTimer((float) pTime);
    }
    else
    {
      this.watchVideoButton.SetActive(true);
      this.waitTimeElement.SetActive(false);
    }
  }
}
