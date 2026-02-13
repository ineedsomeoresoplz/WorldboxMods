// Decompiled with JetBrains decompiler
// Type: RewardPowerWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RewardPowerWindow : MonoBehaviour
{
  [SerializeField]
  private LocalizedText _description;
  [SerializeField]
  private Image[] _icons;
  [SerializeField]
  private bool _auto_gift_key;

  private void OnEnable()
  {
    if (Object.op_Inequality((Object) this._description, (Object) null) && this._auto_gift_key)
      this._description.setKeyAndUpdate(!(Config.power_to_unlock?.id == "clock") ? "unlock_powers_description_any" : "unlock_powers_description_clock_hours");
    InitAds.initAdProviders();
    this.updateButtonIcons();
  }

  private void updateButtonIcons()
  {
    if (Config.power_to_unlock == null || this._icons.Length == 0)
      return;
    PowerButton powerButton = PowerButton.get(Config.power_to_unlock.id);
    if (!Object.op_Inequality((Object) powerButton, (Object) null))
      return;
    Sprite sprite = powerButton.icon.sprite;
    foreach (Image icon in this._icons)
      icon.sprite = sprite;
  }

  public void showRewardedAd()
  {
    if (Config.power_to_unlock?.id == "clock")
    {
      PlayerConfig.instance.data.powerReward = "clock";
      if (!Config.isMobile && !Config.isEditor)
        return;
      RewardedAds.instance.ShowRewardedAd("clock");
    }
    else
    {
      PlayerConfig.instance.data.powerReward = Config.power_to_unlock.id;
      if (!Config.isMobile && !Config.isEditor)
        return;
      RewardedAds.instance.ShowRewardedAd("power");
    }
  }
}
