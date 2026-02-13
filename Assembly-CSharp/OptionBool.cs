// Decompiled with JetBrains decompiler
// Type: OptionBool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
[Obsolete]
public class OptionBool : MonoBehaviour
{
  public bool optionEnabled = true;
  public bool invokeCallbackOnStart = true;
  public SpriteRenderer spriteRenderer;
  public Image icon;
  private Button button;
  public Sprite spriteOn;
  public Sprite spriteOff;
  public UnityEvent callback;
  public UnityEvent<bool> boolCallback;
  public bool gameOption;
  public OptionType gameOptionType;
  public string gameOptionName = "-";

  private void Start()
  {
    this.updateSprite();
    if (!this.invokeCallbackOnStart)
      return;
    if (this.callback != null)
      this.callback.Invoke();
    if (this.boolCallback == null)
      return;
    this.boolCallback.Invoke(this.optionEnabled);
  }

  public void checkGameOption(bool pSwitch = false)
  {
    if (pSwitch)
      PlayerConfig.switchOption(this.gameOptionName, this.gameOptionType);
    this.optionEnabled = PlayerConfig.optionEnabled(this.gameOptionName, this.gameOptionType);
    this.updateSprite();
    OptionAsset pAsset = AssetManager.options_library.get(this.gameOptionName);
    ActionOptionAsset action = pAsset.action;
    if (action == null)
      return;
    action(pAsset);
  }

  private void OnEnable()
  {
    if (Object.op_Equality((Object) World.world, (Object) null) || !this.gameOption)
      return;
    this.updateSprite();
    this.checkGameOption();
    this.updateSprite();
  }

  public void clickButton()
  {
    if (this.gameOption)
    {
      this.checkGameOption(true);
      PlayerConfig.saveData();
    }
    else
    {
      this.optionEnabled = !this.optionEnabled;
      this.updateSprite();
      if (this.callback != null)
        this.callback.Invoke();
      if (this.boolCallback == null)
        return;
      this.boolCallback.Invoke(this.optionEnabled);
    }
  }

  private void updateSprite()
  {
    if (this.optionEnabled)
      this.icon.sprite = this.spriteOn;
    else
      this.icon.sprite = this.spriteOff;
  }

  public void optionSpriteAnimation() => Config.sprite_animations_on = !Config.sprite_animations_on;

  public void optionShowWORLD()
  {
    ((Component) World.world).gameObject.SetActive(!((Component) World.world).gameObject.activeSelf);
  }

  public void optionRemovePremuium()
  {
    Config.hasPremium = false;
    PlayerConfig.instance.data.premium = false;
    PlayerConfig.saveData();
    PremiumElementsChecker.checkElements();
    if (!Config.isMobile)
      return;
    InAppManager.consumePremium();
  }

  public void clearRewards()
  {
    PlayerConfig.instance.data.rewardedPowers.Clear();
    PlayerConfig.saveData();
    PremiumElementsChecker.checkElements();
  }

  public void optionShowCanvas() => ((Behaviour) World.world.canvas).enabled = false;

  public void optionRenderer()
  {
    ((Renderer) this.spriteRenderer).enabled = this.optionEnabled;
    this.updateSprite();
  }
}
