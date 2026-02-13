// Decompiled with JetBrains decompiler
// Type: LocalizationButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LocalizationButton : MonoBehaviour
{
  public Sprite button_current;
  public Sprite button_normal;
  public Sprite button_highlight;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _bg_image;
  [SerializeField]
  private Button _button;
  [SerializeField]
  private Text _text_field;
  private TipButton _tip_button;
  private LocalizedText _localized_text;
  [SerializeField]
  private Text _percent;
  private GameLanguageAsset _asset;
  private bool _initialized;

  public GameLanguageAsset getAsset() => this._asset;

  private void init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._localized_text = ((Component) this._text_field).GetComponent<LocalizedText>();
    this._tip_button = ((Component) this._button).GetComponent<TipButton>();
    this._tip_button.hoverAction = (TooltipAction) (() =>
    {
      if (!InputHelpers.mouseSupported)
        return;
      this.showTooltip();
    });
    this._tip_button.clickAction += (TooltipAction) (() =>
    {
      if (InputHelpers.mouseSupported)
        this.changeLanguage();
      else if (Tooltip.isShowingFor((object) this))
        this.changeLanguage();
      else
        this.showTooltip();
    });
    ((Object) ((Component) this).gameObject).name = this._asset.id;
    if (this._asset.path_icon != null)
    {
      this._icon.sprite = SpriteTextureLoader.getSprite(this._asset.path_icon);
      ((Component) this._icon).gameObject.SetActive(true);
      RectTransform component = ((Component) this._text_field).GetComponent<RectTransform>();
      component.offsetMin = new Vector2(18.5f, component.offsetMin.y);
      component.offsetMax = new Vector2(-4f, component.offsetMax.y);
    }
    else
    {
      ((Component) this._icon).gameObject.SetActive(false);
      RectTransform component = ((Component) this._text_field).GetComponent<RectTransform>();
      component.offsetMin = new Vector2(4f, component.offsetMin.y);
      component.offsetMax = new Vector2(-4f, component.offsetMax.y);
    }
    this._text_field.text = this._asset.name;
    this._localized_text.checkSpecialLanguages(this._asset);
  }

  private void showTooltip()
  {
    Tooltip.show((object) this, "game_language", new TooltipData()
    {
      game_language_asset = this._asset
    });
  }

  private void changeLanguage()
  {
    LocalizedTextManager.instance.setLanguage(this._asset.id);
    WorldLanguagesWindow.updateButtons();
  }

  internal void checkSprite()
  {
    if (LocalizedTextManager.current_language == this._asset)
      this._bg_image.sprite = this.button_current;
    else if (LocalizedTextManager.getCulture(((Object) ((Component) ((Component) this).transform).gameObject).name) == LocalizedTextManager.getCurrentCulture())
      this._bg_image.sprite = this.button_highlight;
    else
      this._bg_image.sprite = this.button_normal;
  }

  public void SetAsset(GameLanguageAsset pAsset, int pDone)
  {
    this._asset = pAsset;
    if (pDone > 0)
    {
      if (pDone < 40)
        ((Graphic) this._percent).color = Toolbox.color_negative_RGBA;
      else if (pDone < 60)
        ((Graphic) this._percent).color = Toolbox.color_log_warning;
      else if (pDone < 80 /*0x50*/)
        ((Graphic) this._percent).color = Toolbox.color_text_default;
      else
        ((Graphic) this._percent).color = Toolbox.color_positive_RGBA;
      this._percent.text = pDone.ToString() + "%";
      ((Component) this._percent).gameObject.SetActive(true);
    }
    else
      ((Component) this._percent).gameObject.SetActive(false);
    this.init();
    this.checkSprite();
  }
}
