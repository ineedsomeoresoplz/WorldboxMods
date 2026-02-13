// Decompiled with JetBrains decompiler
// Type: AdButtonTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AdButtonTimer : MonoBehaviour
{
  internal static AdButtonTimer instance;
  public Text timer;
  public Button button;
  public Image icon;
  private double adTimer;
  private Color transparent = new Color(1f, 1f, 1f, 0.3f);
  private int tRecalc;

  private void Awake()
  {
    AdButtonTimer.instance = this;
    this.adTimer = 10.0;
  }

  internal static void setAdTimer()
  {
    if (PlayerConfig.instance == null)
      return;
    double num = PlayerConfig.instance.data.nextAdTimestamp - Epoch.Current();
    AdButtonTimer.instance.adTimer = num;
    if (AdButtonTimer.instance.adTimer >= 0.0 && PlayerConfig.instance.data.nextAdTimestamp != -1.0)
      return;
    AdButtonTimer.instance.adTimer = -1.0;
  }

  private void OnEnable()
  {
    AdButtonTimer.setAdTimer();
    this.updateButton();
  }

  private void Update()
  {
    if (Config.hasPremium)
    {
      ((Component) this).gameObject.SetActive(false);
    }
    else
    {
      if (this.adTimer > 0.0)
        this.adTimer -= (double) Time.deltaTime;
      this.updateButton();
    }
  }

  private void updateButton()
  {
    if (this.tRecalc > 0)
    {
      --this.tRecalc;
    }
    else
    {
      this.tRecalc = 10;
      AdButtonTimer.setAdTimer();
    }
    if (this.adTimer > 0.0)
    {
      ((Component) this.timer).gameObject.SetActive(true);
      this.timer.text = Toolbox.formatTimer((float) this.adTimer);
      ((Graphic) this.icon).color = this.transparent;
    }
    else
    {
      ((Component) this.timer).gameObject.SetActive(false);
      ((Graphic) this.icon).color = Color.white;
    }
  }
}
