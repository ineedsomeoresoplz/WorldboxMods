// Decompiled with JetBrains decompiler
// Type: ButtonGraphCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ButtonGraphCategory : MonoBehaviour
{
  public Sprite sprite_on;
  public Sprite sprite_off;
  public Sprite sprite_on_light;
  private Image _button_graphics;
  private Image _icon;
  public bool is_on;
  private GraphCategoriesContainer _main_container;
  private Text _text;
  private Image _colored_circle;
  private Image _background_circle;
  private TipButton _tip_button;
  private HistoryDataAsset _asset;
  private bool _initialized;

  private void Awake() => this.init();

  public void init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(switchCategory)));
    this._tip_button = ((Component) this).GetComponent<TipButton>();
    this._button_graphics = ((Component) this).GetComponent<Image>();
    this._icon = ((Component) ((Component) this).transform.FindRecursive("Icon")).GetComponent<Image>();
    this._main_container = ((Component) this).GetComponentInParent<GraphCategoriesContainer>();
    this._text = ((Component) ((Component) this).transform.FindRecursive("Title")).GetComponent<Text>();
    this._colored_circle = ((Component) ((Component) this).transform.FindRecursive("Colored Circle")).GetComponent<Image>();
    this._background_circle = ((Component) ((Component) this).transform.FindRecursive("Background Circle")).GetComponent<Image>();
    this._tip_button.hoverAction = (TooltipAction) (() =>
    {
      if (!InputHelpers.mouseSupported)
        return;
      this.showTooltip();
    });
    this.checkSpriteStatus();
  }

  public void setAsset(HistoryDataAsset pAsset)
  {
    HistoryDataAsset historyDataAsset = pAsset;
    if (historyDataAsset == null)
      return;
    this._asset = pAsset;
    ((Graphic) this._colored_circle).color = historyDataAsset.getColorMain();
    this._icon.sprite = SpriteTextureLoader.getSprite(historyDataAsset.path_icon);
  }

  private void Update() => this.checkSpriteStatus();

  private void checkSpriteStatus()
  {
    if (this.is_on)
    {
      this._button_graphics.sprite = this.sprite_on;
      ((Component) this._background_circle).gameObject.SetActive(true);
    }
    else
    {
      this._button_graphics.sprite = this.sprite_off;
      ((Component) this._background_circle).gameObject.SetActive(false);
    }
  }

  private void switchCategory()
  {
    if (!InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) this))
      this.showTooltip();
    this.is_on = !this.is_on;
    this._main_container.setCategoryEnabled(((Object) this).name, this.is_on);
  }

  private void showTooltip()
  {
    Tooltip.show((object) this, "tip", new TooltipData()
    {
      tip_name = this._asset.getLocaleID(),
      tip_description = this._asset.getDescriptionID(),
      tip_description_2 = "graph_tip"
    });
  }

  public void turnOff() => this.is_on = false;

  public void turnOn() => this.is_on = true;
}
