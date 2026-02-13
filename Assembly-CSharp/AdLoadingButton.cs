// Decompiled with JetBrains decompiler
// Type: AdLoadingButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AdLoadingButton : MonoBehaviour
{
  public Text button_text;
  public LocalizedText button_localized_text;
  public Button button;
  private Image button_image;
  public Sprite spriteOn;
  public Sprite spriteOff;
  private AdLoadingButtonState state;

  private void Awake()
  {
    this.button_image = ((Component) this.button).GetComponent<Image>();
    this.button_localized_text = ((Component) this.button_text).GetComponent<LocalizedText>();
    this.state = AdLoadingButtonState.None;
  }

  private void Update()
  {
    if (Config.isEditor && Config.editor_test_rewards_from_ads)
    {
      this.state = AdLoadingButtonState.AdReady;
      this.toggleState();
    }
    else
    {
      AdLoadingButtonState loadingButtonState;
      if (RewardedAds.isReady())
        loadingButtonState = AdLoadingButtonState.AdReady;
      else if (!Config.adsInitialized)
      {
        loadingButtonState = AdLoadingButtonState.Initializing;
      }
      else
      {
        loadingButtonState = AdLoadingButtonState.AdLoading;
        RewardedAds.trimTimeout();
      }
      if (loadingButtonState == this.state)
        return;
      this.state = loadingButtonState;
      this.toggleState();
    }
  }

  private void toggleState()
  {
    switch (this.state)
    {
      case AdLoadingButtonState.Initializing:
        ((Selectable) this.button).interactable = false;
        this.button_localized_text.setKeyAndUpdate("waiting_for_ad");
        this.button_image.sprite = this.spriteOff;
        break;
      case AdLoadingButtonState.AdLoading:
        ((Selectable) this.button).interactable = false;
        this.button_localized_text.setKeyAndUpdate("loading_ads");
        this.button_image.sprite = this.spriteOff;
        break;
      case AdLoadingButtonState.AdReady:
        ((Selectable) this.button).interactable = true;
        this.button_localized_text.setKeyAndUpdate("watch_ad");
        this.button_image.sprite = this.spriteOn;
        break;
    }
  }
}
