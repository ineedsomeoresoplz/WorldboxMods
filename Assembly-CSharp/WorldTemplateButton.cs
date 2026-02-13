// Decompiled with JetBrains decompiler
// Type: WorldTemplateButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldTemplateButton : MonoBehaviour
{
  public Image icon;
  public Text counter;
  public Text text;
  public PowerButton button_left;
  public PowerButton button_right;
  public PowerButton button_switch;
  public Action eventLeft;
  public Action eventRight;
  public Color color_enabled;
  public Color color_disabled;

  private void OnEnable() => this.updateCounter();

  public void clickSwitch()
  {
    if (this.settings_asset == null)
    {
      Debug.LogError((object) ("Forgot to setup gen button - " + ((Object) ((Component) this).transform).name));
    }
    else
    {
      this.settings_asset.action_switch(this.settings_asset);
      this.updateCounter();
    }
  }

  public void clickLeft()
  {
    if (this.settings_asset == null)
      Debug.LogError((object) ("Forgot to setup gen button - " + ((Object) ((Component) this).transform).name));
    else if (this.settings_asset.decrease == null)
    {
      Debug.LogError((object) ("Forgot to setup gen button DECREASE - " + ((Object) ((Component) this).transform).name));
    }
    else
    {
      this.settings_asset.decrease(this.settings_asset);
      this.updateCounter();
    }
  }

  public void clickRight()
  {
    if (this.settings_asset == null)
      Debug.LogError((object) ("Forgot to setup gen button - " + ((Object) ((Component) this).transform).name));
    else if (this.settings_asset.increase == null)
    {
      Debug.LogError((object) ("Forgot to setup gen button INCREASE - " + ((Object) ((Component) this).transform).name));
    }
    else
    {
      this.settings_asset.increase(this.settings_asset);
      this.updateCounter();
    }
  }

  public void updateCounter()
  {
    int num = this.settings_asset.action_get();
    ((Component) this.text).GetComponent<LocalizedText>().setKeyAndUpdate(this.settings_asset.getLocaleID());
    if (!this.settings_asset.is_switch)
      this.counter.text = num.ToString();
    if (num == 0)
    {
      ((Graphic) this.text).color = this.color_disabled;
      ((Graphic) this.counter).color = this.color_disabled;
      ((Graphic) this.icon).color = this.color_disabled;
    }
    else
    {
      ((Graphic) this.text).color = this.color_enabled;
      ((Graphic) this.counter).color = this.color_enabled;
      ((Graphic) this.icon).color = this.color_enabled;
    }
    if (!this.settings_asset.is_switch)
      return;
    if (num == 1)
    {
      ((Component) this.button_switch).GetComponent<CanvasGroup>().alpha = 1f;
      ((Component) ((Component) this.button_switch).transform.Find("Text")).GetComponent<LocalizedText>().setKeyAndUpdate("short_on");
      this.button_switch.icon.sprite = SpriteTextureLoader.getSprite("ui/icons/IconOn");
    }
    else
    {
      ((Component) this.button_switch).GetComponent<CanvasGroup>().alpha = 0.8f;
      ((Component) ((Component) this.button_switch).transform.Find("Text")).GetComponent<LocalizedText>().setKeyAndUpdate("short_off");
      this.button_switch.icon.sprite = SpriteTextureLoader.getSprite("ui/icons/IconOff");
    }
  }

  private MapGenTemplate _template
  {
    get => AssetManager.map_gen_templates.get(Config.current_map_template);
  }

  private MapGenSettingsAsset settings_asset
  {
    get => AssetManager.map_gen_settings.get(((Object) ((Component) this).transform).name);
  }
}
