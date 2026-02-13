// Decompiled with JetBrains decompiler
// Type: GenericCustomizeWindow`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class GenericCustomizeWindow<TMetaObject, TData, TBanner> : MonoBehaviour
  where TMetaObject : MetaObject<TData>
  where TData : MetaObjectData
  where TBanner : BannerGeneric<TMetaObject, TData>
{
  private bool _created;
  protected Text counter_option_1;
  protected Text counter_option_2;
  protected Text counter_color;
  protected Image image_banner_option_1;
  protected Image image_banner_option_2;
  protected LocalizedText title;
  protected LocalizedText title_option_1;
  protected LocalizedText title_option_2;
  protected LocalizedText title_color;
  protected Transform banner_area;
  protected Image icon_banner;
  protected Image icon_top;
  protected Transform option_1;
  protected Transform option_2;
  protected Transform colors;
  protected Transform colors_parent;
  public TBanner banner;
  private List<ColorElement> _color_elements = new List<ColorElement>();

  protected virtual TMetaObject meta_object
  {
    get => throw new NotImplementedException("meta_object is not set");
  }

  protected TData data => this.meta_object.data;

  private MetaCustomizationAsset meta_asset
  {
    get => AssetManager.meta_customization_library.getAsset(this.meta_type);
  }

  protected virtual MetaType meta_type => throw new NotImplementedException("meta_type is not set");

  private void OnEnable()
  {
    this.loadBanner();
    this.apply();
    int color = this.banner.color;
    this.clickColorElement(this._color_elements[color], color);
  }

  public int getChangeValue()
  {
    int changeValue = 1;
    if (HotkeyLibrary.many_mod.isHolding())
      changeValue = 5;
    return changeValue;
  }

  protected virtual void apply()
  {
    this.updateBanner();
    this.loadBanner();
    this.updateSelection();
  }

  protected virtual void loadBanner() => this.banner.load((NanoObject) this.meta_object);

  protected virtual void updateColors() => this.updateColorsBanner();

  protected virtual void updateColorsBanner()
  {
    ColorAsset color = this.meta_object.getColor();
    ((Graphic) this.image_banner_option_1).color = color.getColorMainSecond();
    if (!this.meta_asset.option_2_color_editable)
      return;
    ((Graphic) this.image_banner_option_2).color = color.getColorBanner();
  }

  private void Awake() => this.create();

  private void create()
  {
    if (this._created)
      return;
    this._created = true;
    this.setupParts();
    this.setupButtons();
    this.setupBanner();
    this.setupTexts();
    this.setupImages();
  }

  protected virtual void setupParts()
  {
    this.title = ((Component) ((Component) this).transform.FindRecursive("Background").Find("Title")).GetComponent<LocalizedText>();
    this.option_1 = ((Component) this).transform.FindRecursive("Option 1");
    this.option_2 = ((Component) this).transform.FindRecursive("Option 2");
    this.colors = ((Component) this).transform.FindRecursive("Colors");
    this.colors_parent = this.colors.FindRecursive("Colors BG");
    Transform recursive = ((Component) this).transform.FindRecursive("Banner");
    this.banner_area = recursive.FindRecursive("BannerArea");
    this.icon_banner = ((Component) recursive.FindRecursive("Icon")).GetComponent<Image>();
    this.icon_top = ((Component) ((Component) this).transform.FindRecursive("Cat")).GetComponent<Image>();
    this.counter_option_1 = ((Component) this.option_1.FindRecursive("Counter")).GetComponent<Text>();
    this.counter_option_2 = ((Component) this.option_2.FindRecursive("Counter")).GetComponent<Text>();
    this.counter_color = ((Component) this.colors.FindRecursive("Counter")).GetComponent<Text>();
    this.title_option_1 = ((Component) this.option_1.FindRecursive("Title")).GetComponent<LocalizedText>();
    this.title_option_2 = ((Component) this.option_2.FindRecursive("Title")).GetComponent<LocalizedText>();
    this.title_color = ((Component) this.colors.FindRecursive("Title")).GetComponent<LocalizedText>();
    this.image_banner_option_1 = ((Component) this.option_1.FindRecursive("Image")).GetComponent<Image>();
    this.image_banner_option_2 = ((Component) this.option_2.FindRecursive("Image")).GetComponent<Image>();
    ((Component) this.option_1).gameObject.SetActive(this.meta_asset.option_1_editable);
    ((Component) this.option_2).gameObject.SetActive(this.meta_asset.option_2_editable);
    ((Component) this.colors).gameObject.SetActive(this.meta_asset.color_editable);
  }

  protected virtual void setupButtons()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this.option_1.FindRecursive("Left")).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(option1Left)));
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this.option_1.FindRecursive("Right")).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(option1Right)));
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this.option_2.FindRecursive("Left")).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(option2Left)));
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this.option_2.FindRecursive("Right")).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(option2Right)));
    // ISSUE: method pointer
    ((UnityEvent) ((Component) ((Component) this).transform.FindRecursive("Randomize")).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(randomize)));
    ColorElement colorElementPrefab = ((Component) this).GetComponentInParent<CustomizeWindow>().color_element_prefab;
    for (int index = 0; index < this.meta_asset.color_count(); ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenericCustomizeWindow<TMetaObject, TData, TBanner>.\u003C\u003Ec__DisplayClass36_0 cDisplayClass360 = new GenericCustomizeWindow<TMetaObject, TData, TBanner>.\u003C\u003Ec__DisplayClass36_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass360.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass360.tIndex = index;
      // ISSUE: reference to a compiler-generated field
      ColorAsset colorByIndex = this.meta_asset.color_library().getColorByIndex(cDisplayClass360.tIndex);
      Color colorMainSecond = colorByIndex.getColorMainSecond();
      Color pInner = Color32.op_Implicit(colorByIndex.getColorBorderInsideAlpha32());
      // ISSUE: reference to a compiler-generated field
      cDisplayClass360.tColorElement = Object.Instantiate<ColorElement>(colorElementPrefab, this.colors_parent);
      // ISSUE: reference to a compiler-generated field
      this._color_elements.Add(cDisplayClass360.tColorElement);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass360.tColorElement.setColor(colorMainSecond, pInner);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass360.tColorElement.index = index;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass360.tColorElement.asset = this.meta_asset;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      cDisplayClass360.tColorElement.setAction(new UnityAction((object) cDisplayClass360, __methodptr(\u003CsetupButtons\u003Eb__0)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ((Component) cDisplayClass360.tColorElement).gameObject.GetComponent<TipButton>().setHoverAction(new TooltipAction(cDisplayClass360.tColorElement.showTooltip), false);
    }
  }

  protected virtual void setupBanner()
  {
    TBanner banner = Resources.Load<TBanner>(this.meta_asset.banner_prefab_id);
    if (Object.op_Equality((Object) (object) banner, (Object) null))
      Debug.LogWarning((object) $"Banner prefab for {this.meta_asset.banner_prefab_id} could not be found");
    this.banner = Object.Instantiate<TBanner>(banner);
    this.banner.enable_default_click = false;
    ((Component) (object) this.banner).transform.localScale = Vector3.one;
    ((Component) (object) this.banner).transform.SetParent(this.banner_area);
    LayoutElement layoutElement = ((Component) (object) this.banner).gameObject.AddComponent<LayoutElement>();
    layoutElement.ignoreLayout = true;
    ((Behaviour) layoutElement).enabled = false;
    ((Component) (object) this.banner).gameObject.AddComponent<DragSnapElement>().fly_back_parent = ((Component) (object) this.banner).transform.FindParentWithName("Viewport");
    RectTransform component = ((Component) (object) this.banner).GetComponent<RectTransform>();
    component.SetAnchor(AnchorPresets.MiddleCenter);
    ((Transform) component).localScale = Vector3.one;
  }

  protected virtual void setupTexts()
  {
    this.title.setKeyAndUpdate(this.meta_asset.title_locale);
    if (this.meta_asset.option_1_editable)
      this.title_option_1.setKeyAndUpdate(this.meta_asset.option_1_locale);
    if (this.meta_asset.option_2_editable)
      this.title_option_2.setKeyAndUpdate(this.meta_asset.option_2_locale);
    if (!this.meta_asset.color_editable)
      return;
    this.title_color.setKeyAndUpdate(this.meta_asset.color_locale);
  }

  protected virtual void setupImages()
  {
    this.icon_banner.sprite = SpriteTextureLoader.getSprite("ui/Icons/" + this.meta_asset.icon_banner);
    this.icon_top.sprite = SpriteTextureLoader.getSprite("ui/Icons/" + this.meta_asset.icon_creature);
  }

  protected virtual void updateCounters()
  {
    if (this.meta_asset.option_1_editable)
      this.counter_option_1.text = $"{(this.meta_asset.option_1_get() + 1).ToString()}/{this.meta_asset.option_1_count().ToString()}";
    if (this.meta_asset.option_2_editable)
      this.counter_option_2.text = $"{(this.meta_asset.option_2_get() + 1).ToString()}/{this.meta_asset.option_2_count().ToString()}";
    if (!this.meta_asset.color_editable)
      return;
    this.counter_color.text = $"{(this.meta_asset.color_get() + 1).ToString()}/{this.meta_asset.color_count().ToString()}";
  }

  protected virtual void updateBanner()
  {
    this.banner.normalize();
    this.banner.updateColor();
  }

  protected virtual void updateSelection()
  {
    this.updateCounters();
    this.updateColors();
    this.onBannerChange();
  }

  protected virtual void onBannerChange()
  {
  }

  public void randomize()
  {
    this.meta_object.generateBanner();
    this.banner.color = this.meta_asset.color_library().list.IndexOf(this.meta_asset.color_library().list.GetRandom<ColorAsset>());
    this.reselectAllColors();
    this._color_elements[this.banner.color].setSelected(true);
    this.apply();
  }

  public void option1Left()
  {
    this.banner.option_1 -= this.getChangeValue();
    this.apply();
  }

  public void option1Right()
  {
    this.banner.option_1 += this.getChangeValue();
    this.apply();
  }

  public void option2Left()
  {
    this.banner.option_2 -= this.getChangeValue();
    this.apply();
  }

  public void option2Right()
  {
    this.banner.option_2 += this.getChangeValue();
    this.apply();
  }

  private void colorSet(int pIndex)
  {
    this.banner.color = pIndex;
    this.apply();
  }

  private void reselectAllColors()
  {
    foreach (ColorElement colorElement in this._color_elements)
      colorElement.setSelected(false);
  }

  private void clickColorElement(ColorElement pElement, int pIndex)
  {
    this.reselectAllColors();
    this.colorSet(pIndex);
    pElement.setSelected(true);
  }
}
