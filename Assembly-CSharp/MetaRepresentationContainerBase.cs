// Decompiled with JetBrains decompiler
// Type: MetaRepresentationContainerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityPools;

#nullable disable
public class MetaRepresentationContainerBase : StatsRowsContainer
{
  [SerializeField]
  protected MetaType _meta_type;
  [SerializeField]
  private LocalizedText _title;
  [SerializeField]
  private Image _background;
  [SerializeField]
  private Image _prefab_bar;
  [SerializeField]
  private LayoutElement _layout_element;
  protected MetaRepresentationAsset asset;

  protected override void init()
  {
    base.init();
    this.asset = AssetManager.meta_representation_library.getAsset(this._meta_type);
    ((Component) this._prefab_bar).gameObject.SetActive(false);
    this._title.setKeyAndUpdate(this.asset.getLocaleID());
  }

  protected override void showStats()
  {
    int pTotal = 0;
    bool pAny = false;
    Dictionary<IMetaObject, int> dictionary = UnsafeCollectionPool<Dictionary<IMetaObject, int>, KeyValuePair<IMetaObject, int>>.Get();
    this.fillDict(ref pTotal, ref pAny, dictionary);
    int pNone = pTotal;
    foreach (KeyValuePair<IMetaObject, int> keyValuePair in (IEnumerable<KeyValuePair<IMetaObject, int>>) dictionary.OrderByDescending<KeyValuePair<IMetaObject, int>, int>((Func<KeyValuePair<IMetaObject, int>, int>) (p => p.Value)))
    {
      IMetaObject key = keyValuePair.Key;
      int num = keyValuePair.Value;
      pNone -= num;
      string pValue = this.amountWithPercent(num, pTotal);
      string pIconPath = this.asset.icon_getter(key);
      string icon = this.asset.show_species_icon ? key.getActorAsset().icon : (string) null;
      this.showBar(this.showStatRowTwoIcons(key.name + Toolbox.coloredGreyPart((object) num, key.getColor().color_text), (object) pValue, key.getColor().color_text, this.asset.meta_type, key.getID(), true, pIconPath, icon, pLocalize: false), num, pTotal, key.getColor().color_text);
    }
    this.checkShowNone(pAny, pNone, pTotal);
    UnsafeCollectionPool<Dictionary<IMetaObject, int>, KeyValuePair<IMetaObject, int>>.Release(dictionary);
    this._layout_element.ignoreLayout = !pAny;
    ((Behaviour) this._background).enabled = pAny;
    ((Component) this._title).gameObject.SetActive(pAny);
  }

  protected virtual void fillDict(
    ref int pTotal,
    ref bool pAny,
    Dictionary<IMetaObject, int> pDict)
  {
    throw new NotImplementedException();
  }

  protected virtual void checkShowNone(bool pAny, int pNone, int pTotal)
  {
    throw new NotImplementedException();
  }

  protected void showBar(KeyValueField pField, int pAmount, int pTotal, string pColorHex)
  {
    float num1 = pTotal > 0 ? (float) pAmount / (float) pTotal : 0.0f;
    Image component = ((Component) ((Component) pField).transform.Find("gen_percent_bar"))?.GetComponent<Image>();
    if (Object.op_Equality((Object) component, (Object) null))
    {
      component = Object.Instantiate<GameObject>(((Component) this._prefab_bar).gameObject, ((Component) pField).transform).GetComponent<Image>();
      ((Component) component).gameObject.SetActive(true);
      ((Object) component).name = "gen_percent_bar";
    }
    float num2 = (float) (100.0 * (double) num1 * 0.5);
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(num2, 8.5f);
    ((Component) component).GetComponent<RectTransform>().sizeDelta = vector2;
    ((Component) component).GetComponent<RectTransform>().anchoredPosition = new Vector2(-2f, 0.0f);
    ((Component) component).transform.SetAsFirstSibling();
    Color color = Toolbox.makeColor(pColorHex);
    color.a = 0.4f;
    ((Graphic) component).color = color;
  }

  protected string amountWithPercent(int pAmount, int pTotal)
  {
    float pFloat = pTotal > 0 ? (float) ((double) pAmount / (double) pTotal * 100.0) : 0.0f;
    if (pTotal == pAmount)
      pFloat = 100f;
    return pFloat.ToText() + "%";
  }

  internal KeyValueField showStatRowTwoIcons(
    string pId,
    object pValue,
    string pColor,
    MetaType pMetaType = MetaType.None,
    long pMetaId = -1,
    bool pColorText = false,
    string pIconPath = null,
    string pIconSecondaryPath = null,
    string pTooltipId = null,
    TooltipDataGetter pTooltipData = null,
    bool pLocalize = true)
  {
    KeyValueField keyValueField = this.showStatRow(pId, pValue, pColor, pMetaType, pMetaId, pColorText, pIconPath, pTooltipId, pTooltipData, pLocalize);
    bool flag = !string.IsNullOrEmpty(pIconSecondaryPath);
    if (flag)
    {
      Sprite sprite = SpriteTextureLoader.getSprite("ui/Icons/" + pIconSecondaryPath);
      keyValueField.icon_secondary.sprite = sprite;
    }
    ((Component) keyValueField.icon_secondary).gameObject.SetActive(flag);
    return keyValueField;
  }

  public void setMetaType(MetaType pType) => this._meta_type = pType;
}
