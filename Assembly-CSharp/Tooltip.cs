// Decompiled with JetBrains decompiler
// Type: Tooltip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Tooltip : MonoBehaviour
{
  private static Dictionary<string, Tooltip> _dict_tooltips = new Dictionary<string, Tooltip>();
  private bool _sim_tooltip;
  private object _last_object;
  internal static float tweenTime = 0.08f;
  public Sprite tooltipTopGraphicsFlat;
  public Sprite tooltipTopGraphicsNormal;
  public Image topGraphics;
  private LayoutElement _headline;
  private VerticalLayoutGroup _layout_group;
  public Image background;
  public Text name;
  public Text description;
  public Text description_2;
  public Text stats_description;
  public Text stats_values;
  public GameObject stats_container;
  private GameObject _description_container;
  private GameObject _description_2_container;
  internal List<TooltipOpinionInfo> opinion_list = new List<TooltipOpinionInfo>();
  internal ObjectPoolGenericMono<Image> pool_traits_actor;
  internal ObjectPoolGenericMono<TooltipOutlineItem> pool_equipments_actor;
  internal ObjectPoolGenericMono<Image> pool_traits_culture;
  internal ObjectPoolGenericMono<Image> pool_traits_language;
  internal ObjectPoolGenericMono<StatsIcon> pool_icons;
  internal ObjectPoolGenericMono<StatsIcon> pool_icons_2;
  [NonSerialized]
  public TooltipAsset asset;
  [NonSerialized]
  private string _type;
  public TooltipData data;
  private RectTransform _rect;
  private Coroutine _hide_tooltip_timer;
  private Vector2 _hide_pos;
  private float _last_height;
  private float _timeout;
  private float _timeout_animation;
  private float _clear_timeout;
  private const int TOOLTIP_WIDTH = 113;
  private const int CURSOR_MARGIN = 25;
  private bool _touch;

  private static Canvas _parent_canvas => CanvasMain.instance.canvas_tooltip;

  private void Awake()
  {
    this._rect = ((Component) this).GetComponent<RectTransform>();
    this._description_container = ((Component) ((Component) this.description).transform.parent).gameObject;
    this._layout_group = ((Component) this).GetComponent<VerticalLayoutGroup>();
    this._headline = ((Component) ((Component) this.topGraphics).transform.parent).GetComponent<LayoutElement>();
    if (!Object.op_Inequality((Object) this.description_2, (Object) null))
      return;
    this._description_2_container = ((Component) ((Component) this.description_2).transform.parent).gameObject;
  }

  public static Tooltip getTooltip(string pID)
  {
    Tooltip tooltip1 = (Tooltip) null;
    if (!Tooltip._dict_tooltips.TryGetValue(pID, out tooltip1))
    {
      TooltipAsset tooltipAsset = AssetManager.tooltips.get(pID);
      if (tooltipAsset == null)
      {
        string message = $"Tooltip Asset {pID} doesn't exist.";
        Debug.LogError((object) message);
        throw new Exception(message);
      }
      Tooltip tooltip2 = Resources.Load<Tooltip>(tooltipAsset.prefab_id);
      if (Object.op_Equality((Object) tooltip2, (Object) null))
      {
        Debug.LogWarning((object) $"Tooltip prefab for {tooltipAsset.prefab_id} could not be found");
        tooltip2 = Resources.Load<Tooltip>("tooltips/tooltip_normal");
      }
      tooltip1 = Object.Instantiate<Tooltip>(tooltip2, ((Component) Tooltip._parent_canvas).transform);
      ((Object) ((Component) tooltip1).transform).name = tooltipAsset.id;
      tooltip1.asset = tooltipAsset;
      Tooltip._dict_tooltips.Add(pID, tooltip1);
    }
    return tooltip1;
  }

  public static void checkClearAll()
  {
    foreach (Tooltip tooltip in Tooltip._dict_tooltips.Values)
      tooltip.checkClear();
  }

  public void checkClear()
  {
    if (((Component) this).gameObject.activeSelf || this._last_object == null)
      return;
    if ((double) this._clear_timeout < 0.20000000298023224)
      this._clear_timeout += Time.deltaTime;
    else
      this._last_object = (object) null;
  }

  public static bool isShowingFor(object pObject)
  {
    foreach (Tooltip tooltip in Tooltip._dict_tooltips.Values)
    {
      if (tooltip._last_object == pObject)
        return true;
    }
    return false;
  }

  public static Tooltip findActive(Predicate<Tooltip> pMatch)
  {
    foreach (Tooltip active in Tooltip._dict_tooltips.Values)
    {
      if (((Component) active).gameObject.activeSelf && pMatch(active))
        return active;
    }
    return (Tooltip) null;
  }

  public static void show(object pObject, string pType, TooltipData pData)
  {
    if ((double) CanvasMain.tooltip_show_timeout > 0.0 || ScrollWindow.isAnimationActive() || Config.isDraggingItem() || InputHelpers.mouseSupported && ((Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(1)) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2) || (double) Input.mouseScrollDelta.y != 0.0))
      return;
    Tooltip.hideTooltip((object) null, false, pType);
    Tooltip tooltip = Tooltip.getTooltip(pType);
    if (Object.op_Equality((Object) tooltip, (Object) null))
      return;
    tooltip.clear();
    tooltip.data = pData;
    if (pObject == null)
      return;
    tooltip.showTooltip(pObject, pType);
  }

  public void clearTextRows()
  {
    this.stats_description.text = "";
    this.stats_values.text = "";
  }

  private void clearStats()
  {
    this.clearTextRows();
    this.stats_container.SetActive(false);
    this.resetDescription();
    this.resetBottomDescription();
  }

  public void showTooltip(object pObject, string pType)
  {
    if (ScrollWindow.isAnimationActive())
      return;
    this._touch = Input.touchCount > 0;
    bool flag = false;
    if (this._last_object == pObject)
      flag = true;
    else if (this._last_object != null && this._last_object.GetType() == pObject.GetType())
      flag = true;
    this._last_object = pObject;
    this._sim_tooltip = this.data.is_sim_tooltip;
    if (!((Component) this).gameObject.activeSelf)
      ((Component) this).gameObject.SetActive(true);
    this._type = pType;
    this.asset = AssetManager.tooltips.get(this._type);
    ((Component) this).transform.localScale = new Vector3(this.data.tooltip_scale, this.data.tooltip_scale, 1f);
    this._timeout = 0.1f;
    this.clearStats();
    this.description.text = "";
    this.opinion_list.Clear();
    TooltipShowAction callback = this.asset.callback;
    if (callback != null)
      callback(this, this._type, this.data);
    this.checkBottomLineSeparator();
    this.showStatValues();
    LayoutRebuilder.ForceRebuildLayoutImmediate(this._rect);
    this.reposition();
    ((Component) this.name).GetComponent<LocalizedText>().checkSpecialLanguages(this.data.game_language_asset);
    ((Component) this.description).GetComponent<LocalizedText>().checkSpecialLanguages(this.data.game_language_asset);
    ((Component) this.description_2)?.GetComponent<LocalizedText>().checkSpecialLanguages(this.data.game_language_asset);
    if (flag)
      return;
    int num = this.data.sound_allowed ? 1 : 0;
  }

  private void checkBottomLineSeparator()
  {
    GameObject gameObject = ((Component) ((Component) this).transform.FindRecursive("Line Bottom Separator"))?.gameObject;
    if (Object.op_Equality((Object) gameObject, (Object) null))
      return;
    bool flag = ((Component) this.description).gameObject.activeSelf && this.description.text.Length > 0;
    if (((!((Component) this.description_2).gameObject.activeSelf ? 0 : (this.description_2.text.Length > 0 ? 1 : 0)) & (flag ? 1 : 0)) != 0 && this.stats_description.text.Length == 0)
      gameObject.SetActive(true);
    else
      gameObject.SetActive(false);
  }

  public bool isTouchTooltip() => this._touch;

  internal void reposition()
  {
    Vector2 pPos;
    this.getPosition(this.getDirection(Vector2.op_Implicit(Input.mousePosition)), out pPos);
    ((Transform) this._rect).position = Vector2.op_Implicit(pPos);
    if ((double) ((Component) this).transform.localScale.x != (double) this.data.tooltip_scale)
      ((Component) this).transform.localScale = new Vector3(this.data.tooltip_scale, this.data.tooltip_scale, 1f);
    float y = ((Graphic) this.background).rectTransform.sizeDelta.y;
    if (y.Equals(this._last_height))
      return;
    if ((double) y < 27.0)
    {
      this.topGraphics.sprite = this.tooltipTopGraphicsNormal;
      this._headline.preferredHeight = 17f;
    }
    else
    {
      this.topGraphics.sprite = this.tooltipTopGraphicsFlat;
      this._headline.preferredHeight = 21.6f;
    }
    this._last_height = y;
  }

  internal bool nullCheck(object pObject)
  {
    if (this.data == null)
      return true;
    switch (pObject)
    {
      case null:
        return true;
      case NanoObject nanoObject:
        if (!nanoObject.isAlive())
          return true;
        break;
      case GameObject gameObject:
        if (Object.op_Equality((Object) gameObject, (Object) null))
          return true;
        break;
      case MonoBehaviour monoBehaviour:
        if (Object.op_Equality((Object) monoBehaviour, (Object) null) || Object.op_Equality((Object) ((Component) monoBehaviour).gameObject, (Object) null))
          return true;
        break;
    }
    return false;
  }

  internal void getPosition(TooltipDirection pDirection, out Vector2 pPos)
  {
    pPos = Vector2.op_Implicit(Input.mousePosition);
    float num = Tooltip._parent_canvas.scaleFactor * this.data.tooltip_scale;
    Vector2 sizeDelta1 = this._rect.sizeDelta;
    Vector2 sizeDelta2 = this._rect.sizeDelta;
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(0.5f, 0.5f);
    if (pDirection.HasFlag((Enum) TooltipDirection.Up))
      vector2.y = 0.0f;
    if (pDirection.HasFlag((Enum) TooltipDirection.MagnetDown))
      vector2.y = 0.0f;
    if (pDirection.HasFlag((Enum) TooltipDirection.Down))
      vector2.y = 1f;
    if (pDirection.HasFlag((Enum) TooltipDirection.MagnetUp))
      vector2.y = 1f;
    if (pDirection.HasFlag((Enum) TooltipDirection.Left))
      vector2.x = 1f;
    if (pDirection.HasFlag((Enum) TooltipDirection.MagnetRight))
      vector2.x = 1f;
    if (pDirection.HasFlag((Enum) TooltipDirection.Right))
      vector2.x = 0.0f;
    if (pDirection.HasFlag((Enum) TooltipDirection.MagnetLeft))
      vector2.x = 0.0f;
    if (pDirection.HasFlag((Enum) TooltipDirection.Up))
      pPos.y += 25f;
    if (pDirection.HasFlag((Enum) TooltipDirection.Down))
      pPos.y -= 25f;
    if (pDirection.HasFlag((Enum) TooltipDirection.Left))
      pPos.x -= 25f;
    if (pDirection.HasFlag((Enum) TooltipDirection.Right))
      pPos.x += 25f;
    if (pDirection.HasFlag((Enum) TooltipDirection.MagnetUp))
      pPos.y = (float) Screen.height;
    if (pDirection.HasFlag((Enum) TooltipDirection.MagnetDown))
      pPos.y = 0.0f;
    if (pDirection.HasFlag((Enum) TooltipDirection.MagnetLeft))
      pPos.x = 0.0f;
    if (pDirection.HasFlag((Enum) TooltipDirection.MagnetRight))
      pPos.x = (float) Screen.width;
    this._rect.pivot = vector2;
    this._rect.anchorMin = vector2;
    this._rect.anchorMax = vector2;
  }

  internal TooltipDirection getDirection(Vector2 pPos)
  {
    double x = (double) pPos.x;
    double y = (double) pPos.y;
    float num1 = Tooltip._parent_canvas.scaleFactor * this.data.tooltip_scale;
    float num2 = (float) ((double) this._rect.sizeDelta.y * (double) num1 + 25.0);
    float num3 = (float) ((double) this._rect.sizeDelta.x * (double) num1 + 25.0);
    TooltipDirection direction = TooltipDirection.None;
    bool flag1 = y - (double) num2 <= 0.0;
    double num4 = (double) num2 / 2.0;
    bool flag2 = y + (double) num2 > (double) Screen.height;
    bool flag3 = y + (double) num2 / 2.0 > (double) Screen.height;
    bool flag4 = x + (double) num3 / 2.0 > (double) Screen.width;
    bool flag5 = x + (double) num3 > (double) Screen.width;
    bool flag6 = x - (double) num3 / 2.0 <= 0.0;
    bool flag7 = x - (double) num3 <= 0.0;
    bool flag8 = flag6 & flag4;
    bool flag9 = flag7 & flag5;
    if (!this.isTouchTooltip())
    {
      TooltipDirection tooltipDirection = !flag1 ? (!flag2 ? direction | TooltipDirection.Down : direction | TooltipDirection.Down) : direction | TooltipDirection.MagnetDown;
      direction = !flag5 ? tooltipDirection | TooltipDirection.Right : (!flag1 ? tooltipDirection | TooltipDirection.MagnetRight : tooltipDirection | TooltipDirection.Left);
    }
    else
    {
      if (flag2)
      {
        if (!flag9)
        {
          if (flag3)
            direction |= TooltipDirection.MagnetUp;
        }
        else if (flag1)
          direction |= TooltipDirection.MagnetUp;
        else
          direction |= TooltipDirection.Down;
      }
      else if (flag1)
        direction |= TooltipDirection.Up;
      else if (!flag8)
        direction |= TooltipDirection.Up;
      if (flag6)
      {
        if (flag7)
          direction |= TooltipDirection.Right;
        else
          direction |= TooltipDirection.MagnetLeft;
      }
      else if (flag4)
      {
        if (flag5)
          direction |= TooltipDirection.Left;
        else
          direction |= TooltipDirection.MagnetRight;
      }
      else if (direction == TooltipDirection.None || direction == TooltipDirection.MagnetUp)
      {
        if (flag7)
          direction |= TooltipDirection.Right;
        else
          direction |= TooltipDirection.Left;
      }
    }
    return direction;
  }

  internal void setDescription(string pDescription, string pColor = null)
  {
    this.resetDescription();
    this.addDescription(pDescription, pColor);
  }

  internal void addDescription(string pDescription, string pColor = null)
  {
    if (!(pDescription != ""))
      return;
    if (!string.IsNullOrEmpty(pColor))
      pDescription = Toolbox.coloredText(pDescription, pColor);
    this.description.text += pDescription;
    this._description_container.SetActive(true);
  }

  internal void resetDescription()
  {
    this.description.text = "";
    this.description.font = LocalizedTextManager.current_font;
    this._description_container.SetActive(false);
  }

  internal void setBottomDescription(string pDescription, string pColor = null)
  {
    this.resetBottomDescription();
    this.addBottomDescription(pDescription, pColor);
  }

  internal void addBottomDescription(string pDescription, string pColor = null)
  {
    if (!(pDescription != ""))
      return;
    if (!string.IsNullOrEmpty(pColor))
      pDescription = Toolbox.coloredText(pDescription, pColor);
    this.description_2.text += pDescription;
    this._description_2_container.SetActive(true);
  }

  internal void resetBottomDescription()
  {
    if (Object.op_Equality((Object) this.description_2, (Object) null))
      return;
    this.description_2.text = "";
    this.description_2.font = LocalizedTextManager.current_font;
    this._description_2_container.SetActive(false);
  }

  internal void addStatValues(string pStats, string pValues)
  {
    this.stats_description.text += pStats;
    this.stats_values.text += pValues;
    this.stats_container.SetActive(true);
  }

  internal void showOpinion(
    string pDescriptionString,
    string pValuesString,
    Text pTextDescription = null,
    Text pTextValues = null)
  {
    if (Object.op_Equality((Object) pTextDescription, (Object) null))
    {
      pTextDescription = this.stats_description;
      pTextValues = this.stats_values;
    }
    pTextDescription.text += pDescriptionString;
    pTextValues.text += pValuesString;
  }

  internal void showStatValues()
  {
    if (this.stats_description.text.Length <= 0)
      return;
    this.stats_container.SetActive(true);
    LocalizedText localizedText1;
    if (((Component) this.stats_values).TryGetComponent<LocalizedText>(ref localizedText1) && ((Behaviour) localizedText1).enabled)
      localizedText1.checkSpecialLanguages();
    LocalizedText localizedText2;
    if (!((Component) this.stats_description).TryGetComponent<LocalizedText>(ref localizedText2) || !((Behaviour) localizedText2).enabled)
      return;
    localizedText2.checkSpecialLanguages();
  }

  internal void addItemText(
    string pID,
    float pValue,
    bool pPercent = false,
    bool pAddColor = true,
    bool pAddPlus = true,
    string pMainColor = "#43FF43",
    bool pForceZero = false)
  {
    if ((double) pValue == 0.0 && !pForceZero)
      return;
    string pValue1 = pValue.ToText();
    if (pPercent)
      pValue1 += "%";
    if (!pAddColor)
      this.addLineText(pID, pValue1, "#FFFFFF", pPercent);
    else if ((double) pValue > 0.0)
    {
      if (pAddPlus)
        pValue1 = "+" + pValue1;
      this.addLineText(pID, pValue1, pMainColor, pPercent);
    }
    else
      this.addLineText(pID, pValue1, "#FB2C21", pPercent);
  }

  internal void addLineIntText(string pID, int pValue, string pColor = null, bool pLocalize = true)
  {
    this.addLineText(pID, pValue.ToText(), pColor, pLocalize: pLocalize);
  }

  internal void addLineIntText(
    string pID,
    long pValue,
    string pColor = null,
    bool pLocalize = true,
    int pLimitValue = 21)
  {
    this.addLineLongText(pID, pValue, pColor, pLocalize, pLimitValue);
  }

  internal void addLineLongText(
    string pID,
    long pValue,
    string pColor = null,
    bool pLocalize = true,
    int pLimitValue = 21)
  {
    this.addLineText(pID, pValue.ToText(), pColor, pLocalize: pLocalize, pLimitValue: pLimitValue);
  }

  internal void addLineBreak()
  {
    if (this.stats_description.text.Length == 0)
      return;
    this.stats_description.text += "\n";
    this.stats_values.text += "\n";
  }

  public void tryShowBoolDebug(string pIO, bool pValue)
  {
    string pColor = !pValue ? "#FB2C21" : "#43FF43";
    this.addLineText(pIO, pValue.ToString(), pColor, pLocalize: false);
  }

  internal void addLineText(
    string pID,
    string pValue,
    string pColor = null,
    bool pPercent = false,
    bool pLocalize = true,
    int pLimitValue = 21)
  {
    if (this.stats_description.text.Length > 0)
      this.addLineBreak();
    if (pValue != null && pValue.Length > pLimitValue)
      pValue = pValue.Substring(0, pLimitValue - 1) + "...";
    string str = pLocalize ? pID.Localize() : pID;
    if (pPercent)
      str += " %";
    if (!string.IsNullOrEmpty(pColor))
    {
      this.stats_description.text += str;
      this.stats_values.text += Toolbox.coloredText(pValue, pColor);
    }
    else
    {
      this.stats_description.text += str;
      this.stats_values.text += pValue;
    }
  }

  internal void addOpinion(TooltipOpinionInfo pOpinion) => this.opinion_list.Add(pOpinion);

  private void Update()
  {
    this.updateTextContentAnimation();
    if ((double) this._timeout > 0.0)
    {
      this._timeout -= Time.deltaTime;
    }
    else
    {
      if (!InputHelpers.GetAnyMouseButtonDown() && (double) Input.mouseScrollDelta.y == 0.0 && !ScrollRectExtended.isAnyDragged())
        return;
      this.hide();
    }
  }

  private void LateUpdate()
  {
    if (this._hide_tooltip_timer != null)
    {
      if ((double) Vector2.Distance(Vector2.op_Implicit(Input.mousePosition), this._hide_pos) <= 10.0)
        return;
      this.hide();
    }
    else
      this.reposition();
  }

  private void updateTextContentAnimation()
  {
    if ((double) this._timeout_animation > 0.0)
    {
      this._timeout_animation -= Time.deltaTime;
    }
    else
    {
      this._timeout_animation = 0.08f;
      if (this.asset.callback_text_animated == null)
        return;
      this.asset.callback_text_animated(this, this._type, this.data);
    }
  }

  public void hide()
  {
    this.clearHideTimer();
    ((Component) this).gameObject.SetActive(false);
    this._clear_timeout = 0.0f;
    this._sim_tooltip = false;
    this.data?.Dispose();
    this.data = (TooltipData) null;
  }

  private void OnDisable() => this.clear();

  private void clear()
  {
    this.pool_traits_actor?.clear();
    this.pool_equipments_actor?.clear();
    this.pool_traits_culture?.clear();
    this.pool_traits_language?.clear();
    this.pool_icons?.clear();
    this.pool_icons_2?.clear();
    this.clearHideTimer();
  }

  public static void hideTooltip(object pObjectToSkip, bool pOnlySimObjects, string pSkipType)
  {
    foreach (Tooltip tooltip in Tooltip._dict_tooltips.Values)
    {
      if ((pObjectToSkip == null || pObjectToSkip != tooltip._last_object) && (!pOnlySimObjects || tooltip._sim_tooltip) && (!(pSkipType != string.Empty) || !(tooltip.asset.id == pSkipType)) && ((Component) tooltip).gameObject.activeSelf)
      {
        tooltip.hide();
        tooltip._last_object = (object) null;
      }
    }
  }

  public static void blockTooltips(float pDuration = 0.0f)
  {
    Tooltip.hideTooltip((object) null, false, string.Empty);
    if ((double) pDuration <= 0.0)
      return;
    CanvasMain.addTooltipShowTimeout(pDuration);
  }

  public static void hideTooltipNow() => Tooltip.hideTooltip((object) null, false, string.Empty);

  public static void hideTooltip() => Tooltip.scheduledHide(0.08f);

  public static bool anyActive()
  {
    foreach (Component component in Tooltip._dict_tooltips.Values)
    {
      if (component.gameObject.activeSelf)
        return true;
    }
    return false;
  }

  public static void scheduledHide(float pTimeout = 0.15f, bool pSkipTouch = false)
  {
    foreach (Tooltip tooltip in Tooltip._dict_tooltips.Values)
    {
      if (((Component) tooltip).gameObject.activeSelf && (!pSkipTouch || !tooltip.isTouchTooltip()))
        tooltip.scheduleHide(pTimeout);
    }
  }

  public static void cancelHiding()
  {
    foreach (Tooltip tooltip in Tooltip._dict_tooltips.Values)
    {
      if (((Component) tooltip).gameObject.activeSelf)
        tooltip.clearHideTimer();
    }
  }

  private void scheduleHide(float pTimeout)
  {
    this.clearHideTimer();
    this._hide_tooltip_timer = this.StartCoroutine(this.hideDelayed(pTimeout));
    this._hide_pos = Vector2.op_Implicit(Input.mousePosition);
  }

  private void clearHideTimer()
  {
    if (this._hide_tooltip_timer != null)
      this.StopCoroutine(this._hide_tooltip_timer);
    this._hide_tooltip_timer = (Coroutine) null;
  }

  private IEnumerator hideDelayed(float pTimeout)
  {
    yield return (object) new WaitForSecondsRealtime(pTimeout);
    this.hide();
  }

  public Image getRawIcon(string pName)
  {
    Transform recursive = ((Component) this).transform.FindRecursive(pName);
    if (Object.op_Equality((Object) recursive, (Object) null))
      Debug.LogError((object) ("Icon not found " + pName));
    return ((Component) recursive).GetComponent<Image>();
  }

  public void setRawIcon(string pName, Sprite pSprite)
  {
    Image rawIcon = this.getRawIcon(pName);
    if (Object.op_Equality((Object) rawIcon, (Object) null))
      Debug.LogError((object) ("Icon not found " + pName));
    rawIcon.sprite = pSprite;
  }

  public void setTitle(string pMainText, string pSubText = "", string pColorHex = "#F3961F")
  {
    string str1 = Toolbox.coloredText(pMainText, pColorHex);
    if (pSubText != "")
    {
      string pColor = Toolbox.makeDarkerColor(pColorHex, 0.8f);
      string str2 = $"<size=7>{Toolbox.coloredText(LocalizedTextManager.getText(pSubText), pColor)}</size>";
      str1 = $"{str1}\n{str2}";
    }
    this.name.text = str1;
  }

  public Image getSpeciesIcon() => this.getRawIcon("IconSpecies");

  public void setSpeciesIcon(Sprite pSprite) => this.setRawIcon("IconSpecies", pSprite);
}
