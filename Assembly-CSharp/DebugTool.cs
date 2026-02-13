// Decompiled with JetBrains decompiler
// Type: DebugTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class DebugTool : MonoBehaviour
{
  public const int DT_WIDTH = 126;
  public const int DT_HEIGHT = 60;
  protected ObjectPoolGenericMono<DebugToolTextElement> pool_texts;
  public DebugToolTextElement element_prefab;
  internal int textCount;
  public Dropdown dropdown;
  internal bool sort_order_reversed;
  internal bool sort_by_names;
  internal bool sort_by_values = true;
  internal bool show_averages = true;
  internal bool percentage_slowest;
  internal bool hide_zeroes = true;
  internal bool show_counter = true;
  internal bool show_max = true;
  internal DebugToolState state = DebugToolState.FrameBudget;
  public DebugToolType type;
  internal bool paused;
  internal DebugToolAsset asset;
  [HideInInspector]
  public DebugDropdown active_dropdown;
  private double last_update_timestamp;
  private List<DebugIconOptionAction> list_actions = new List<DebugIconOptionAction>();
  private List<Image> list_icons = new List<Image>();
  private Transform transform_texts;
  private Transform benchmark_icons;
  private string _latest_text;

  private void Awake()
  {
    this.populateOptions();
    this.benchmark_icons = ((Component) this).transform.FindRecursive("Benchmark Icons");
    this.initButtons();
    this.initElements();
  }

  private void initElements()
  {
    this.transform_texts = ((Component) this).transform.FindRecursive("Texts");
    this.pool_texts = new ObjectPoolGenericMono<DebugToolTextElement>(this.element_prefab, this.transform_texts);
    ((Component) this.element_prefab).gameObject.SetActive(false);
  }

  private float calculateLineHeight(Text pText)
  {
    Rect rectExtents = pText.cachedTextGenerator.rectExtents;
    Vector2 vector2 = Vector2.op_Multiply(((Rect) ref rectExtents).size, 0.5f);
    return pText.cachedTextGeneratorForLayout.GetPreferredHeight("A", pText.GetGenerationSettings(vector2));
  }

  internal void populateOptions()
  {
    this.dropdown.ClearOptions();
    List<string> stringList = new List<string>();
    foreach (DebugToolAsset debugToolAsset in AssetManager.debug_tool_library.list)
    {
      if (debugToolAsset.type == this.type)
        stringList.Add(debugToolAsset.name);
    }
    this.dropdown.AddOptions(stringList);
    // ISSUE: method pointer
    ((UnityEvent<int>) this.dropdown.onValueChanged).RemoveListener(new UnityAction<int>((object) this, __methodptr(switchTool)));
    // ISSUE: method pointer
    ((UnityEvent<int>) this.dropdown.onValueChanged).AddListener(new UnityAction<int>((object) this, __methodptr(switchTool)));
  }

  public void filterOptions(string pInput)
  {
    foreach (DebugDropdownOption componentsInChild in ((Component) ((Component) this.active_dropdown).transform).GetComponentsInChildren<DebugDropdownOption>(true))
    {
      string text = componentsInChild.title.text;
      if (text == "Debug option")
        ((Component) componentsInChild).gameObject.SetActive(false);
      else if ((string.IsNullOrEmpty(pInput) ? 0 : (!text.ToLower().Contains(pInput.ToLower()) ? 1 : 0)) != 0)
        ((Component) componentsInChild).gameObject.SetActive(false);
      else
        ((Component) componentsInChild).gameObject.SetActive(true);
    }
  }

  private void initButtons()
  {
    // ISSUE: method pointer
    this.newButton("SortByName", new UnityAction((object) this, __methodptr(clickSortByName)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.sort_by_names)));
    // ISSUE: method pointer
    this.newButton("SortByValues", new UnityAction((object) this, __methodptr(clickSortByValues)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.sort_by_values)));
    // ISSUE: method pointer
    this.newButton("SortReversed", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_2)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.sort_order_reversed)));
    // ISSUE: method pointer
    this.newButton("ShowAverages", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_4)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.isValueAverage())));
    // ISSUE: method pointer
    this.newButton("PercentBasedOnSlowest", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_6)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.percentage_slowest)));
    // ISSUE: method pointer
    this.newButton("HideZeroes", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_8)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.hide_zeroes)));
    // ISSUE: method pointer
    this.newButton("ShowCounter", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_10)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.show_counter)));
    // ISSUE: method pointer
    this.newButton("ShowMax", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_12)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.show_max)));
    // ISSUE: method pointer
    this.newButton("ShowSeconds", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_14)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.state == DebugToolState.Values)));
    // ISSUE: method pointer
    this.newButton("ShowPercentages", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_16)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.state == DebugToolState.Percent)));
    // ISSUE: method pointer
    this.newButton("ShowTimeSpent", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_18)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.state == DebugToolState.TimeSpent)));
    // ISSUE: method pointer
    this.newButton("ShowFrameBudget", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_20)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.state == DebugToolState.FrameBudget)));
    // ISSUE: method pointer
    this.newButton("Paused", new UnityAction((object) this, __methodptr(\u003CinitButtons\u003Eb__30_22)), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, this.paused)));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    this.newButton("EnableBenchmarks", DebugTool.\u003C\u003Ec.\u003C\u003E9__30_24 ?? (DebugTool.\u003C\u003Ec.\u003C\u003E9__30_24 = new UnityAction((object) DebugTool.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CinitButtons\u003Eb__30_24))), (DebugIconOptionAction) (pIcon => this.checkIcon(pIcon, Bench.bench_enabled)));
  }

  private void newButton(string pID, UnityAction pAction, DebugIconOptionAction pCheckIcon)
  {
    Transform recursive = ((Component) this).transform.FindRecursive(pID);
    ((UnityEvent) ((Component) recursive).GetComponent<Button>().onClick).AddListener(pAction);
    this.list_actions.Add(pCheckIcon);
    this.list_icons.Add(((Component) recursive).GetComponent<Image>());
  }

  public bool isValueAverage() => this.show_averages;

  public bool isState(DebugToolState pState) => this.state == pState;

  private void updateIcons()
  {
    for (int index = 0; index < this.list_actions.Count; ++index)
      this.list_actions[index](this.list_icons[index]);
  }

  private void checkIcon(Image pImageIcon, bool pValue)
  {
    if (pValue)
      ((Graphic) pImageIcon).color = Color.white;
    else
      ((Graphic) pImageIcon).color = Color32.op_Implicit(Toolbox.color_transparent_grey);
  }

  private void switchTool(int pIndex)
  {
    string text = this.dropdown.options[pIndex].text;
    this.setAsset(AssetManager.debug_tool_library.get(text));
  }

  public void setAsset(DebugToolAsset pAsset)
  {
    this.asset = pAsset;
    this.type = this.asset.type;
    ((Component) this.benchmark_icons).gameObject.SetActive(this.asset.show_benchmark_buttons);
    if (this.asset.action_start == null)
      return;
    this.asset.action_start(this);
  }

  private void Update()
  {
    if (SmoothLoader.isLoading())
      return;
    this.updateIcons();
    double curSessionTime = World.world.getCurSessionTime();
    if (curSessionTime < this.last_update_timestamp + (double) this.asset.update_timeout || this.paused)
      return;
    if (this.asset.action_update != null)
      this.asset.action_update(this);
    this.clearTexts();
    string text = this.dropdown.captionText.text;
    this.last_update_timestamp = curSessionTime;
    if (this.asset.action_1 != null)
      this.asset.action_1(this);
    if (this.asset.action_2 != null)
      this.asset.action_2(this);
    this.updateSize();
    this.pool_texts.disableInactive();
    this.StartCoroutine(this.updateSizeAfterFrame());
  }

  public IEnumerator updateSizeAfterFrame()
  {
    yield return (object) CoroutineHelper.wait_for_end_of_frame;
    this.updateSize();
  }

  private void updateSize()
  {
    float num1 = LayoutUtility.GetPreferredWidth(((Component) this.transform_texts).GetComponent<RectTransform>()) * 1.2f;
    float num2 = LayoutUtility.GetPreferredHeight(((Component) this.transform_texts).GetComponent<RectTransform>()) + 40f;
    if ((double) num1 < 126.0)
      num1 = 126f;
    if ((double) num2 < 60.0)
      num2 = 60f;
    ((Component) this).GetComponent<RectTransform>().sizeDelta = new Vector2(num1, num2);
  }

  public void clickSortByName()
  {
    this.sort_by_names = !this.sort_by_names;
    this.sort_by_values = !this.sort_by_names;
  }

  public void clickSortByValues()
  {
    this.sort_by_values = !this.sort_by_values;
    this.sort_by_names = !this.sort_by_values;
  }

  public int kingdomSorter(Kingdom k1, Kingdom k2) => k2.units.Count.CompareTo(k1.units.Count);

  public int citySorter(City c1, City c2)
  {
    return c2.getPopulationPeople().CompareTo(c1.getPopulationPeople());
  }

  internal void setText(
    string pT1,
    object pT2,
    float pBarValue = 0.0f,
    bool pShowBar = false,
    long pCounter = 0,
    bool pShowCounter = false,
    bool pShowMax = false,
    string pMaxValue = "")
  {
    DebugToolTextElement next = this.pool_texts.getNext();
    string str = pT2 == null ? "-" : pT2.ToString();
    if (pT2 != null)
    {
      if (pShowCounter && this.show_counter && (this.asset.split_benchmark || this.asset.show_last_count))
        str = $"{pCounter.ToString()} | {str}";
      if (pShowMax)
        str = $"{pMaxValue} | {str}";
    }
    next.text_left.text = pT1;
    next.text_right.text = str;
    ++this.textCount;
    if (pShowBar)
    {
      ((Component) next.text_bar).gameObject.SetActive(true);
      if ((double) pBarValue > 100.0)
        pBarValue = 101f;
      float num = pBarValue * 0.5f;
      ((Component) next.text_bar).GetComponent<RectTransform>().sizeDelta = new Vector2(num, 4.2f);
      if ((double) pBarValue > 70.0 && (double) pBarValue != 100.0)
        ((Graphic) next.text_bar).color = Color32.op_Implicit(Toolbox.color_debug_bar_red);
      else
        ((Graphic) next.text_bar).color = Color32.op_Implicit(Toolbox.color_debug_bar_blue);
    }
    else
      ((Component) next.text_bar).gameObject.SetActive(false);
  }

  internal void setSeparator()
  {
    DebugToolTextElement next = this.pool_texts.getNext();
    next.text_left.text = string.Empty;
    next.text_right.text = string.Empty;
    ((Component) next.text_bar).gameObject.SetActive(false);
  }

  private void clearTexts()
  {
    this.textCount = 0;
    this.pool_texts.clear(false);
  }

  public void clickClose() => Object.Destroy((Object) ((Component) this).gameObject, 0.01f);

  public void clickDuplicate()
  {
    DebugConfig.createTool(this.asset.id, (int) ((Component) this).transform.localPosition.x + 126 + 2, (int) ((Component) this).transform.localPosition.y);
  }
}
