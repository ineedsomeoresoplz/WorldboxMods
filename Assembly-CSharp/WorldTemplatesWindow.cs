// Decompiled with JetBrains decompiler
// Type: WorldTemplatesWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldTemplatesWindow : MonoBehaviour
{
  public Text text_hi;
  public Text text_size_warning;
  public Image icon_1;
  public Image icon_2;
  public Image preview_template;
  public Transform container_buttons;
  public GameObject reset_button;
  public CustomButtonSwitch switch_button;

  private void Awake()
  {
    this.switch_button.click_increase = new Action(this.increaseSize);
    this.switch_button.click_decrease = new Action(this.decreaseSize);
  }

  public void increaseSize()
  {
    int index = MapSizeLibrary.getSizes().IndexOf<string>(Config.customMapSize) + 1;
    if (index > MapSizeLibrary.getSizes().Length - 1)
      index = 0;
    Config.customMapSize = MapSizeLibrary.getSizes()[index];
  }

  public void decreaseSize()
  {
    int index = MapSizeLibrary.getSizes().IndexOf<string>(Config.customMapSize) - 1;
    if (index < 0)
      index = MapSizeLibrary.getSizes().Length - 1;
    Config.customMapSize = MapSizeLibrary.getSizes()[index];
  }

  private void Update()
  {
    MapSizeAsset mapSizeAsset = AssetManager.map_sizes.get(Config.customMapSize);
    if (mapSizeAsset.show_warning)
    {
      ((Component) this.text_hi).gameObject.SetActive(false);
      ((Component) this.text_size_warning).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.text_hi).gameObject.SetActive(true);
      ((Component) this.text_size_warning).gameObject.SetActive(false);
    }
    this.icon_1.sprite = mapSizeAsset.getIconSprite();
    this.icon_2.sprite = mapSizeAsset.getIconSprite();
  }

  private void OnEnable()
  {
    MapGenTemplate mapGenTemplate = AssetManager.map_gen_templates.get(Config.current_map_template);
    this.preview_template.sprite = SpriteTextureLoader.getSprite(mapGenTemplate.path_icon);
    this.checkButtons();
    if (mapGenTemplate.show_reset_button)
      this.reset_button.SetActive(true);
    else
      this.reset_button.SetActive(false);
  }

  public void resetTemplate()
  {
    MapGenTemplate pAsset = AssetManager.map_gen_templates.get(Config.current_map_template);
    AssetManager.map_gen_templates.resetTemplateValues(pAsset);
    this.checkButtons();
  }

  private void checkButtons()
  {
    MapGenTemplate pAsset = AssetManager.map_gen_templates.get(Config.current_map_template);
    for (int index = 0; index < this.container_buttons.childCount; ++index)
    {
      WorldTemplateButton component = ((Component) this.container_buttons.GetChild(index)).gameObject.GetComponent<WorldTemplateButton>();
      if (!Object.op_Equality((Object) component, (Object) null))
      {
        string name = ((Object) component).name;
        if (AssetManager.map_gen_settings.get(name).allowed_check(pAsset))
        {
          ((Component) component).gameObject.SetActive(true);
          component.updateCounter();
        }
        else
          ((Component) component).gameObject.SetActive(false);
      }
    }
  }
}
