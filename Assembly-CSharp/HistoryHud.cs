// Decompiled with JetBrains decompiler
// Type: HistoryHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HistoryHud : MonoBehaviour
{
  public static HistoryHud instance;
  [SerializeField]
  private GameObject _template_obj;
  private List<HistoryHudItem> _history_items = new List<HistoryHudItem>(10);
  private ObjectPoolGenericMono<HistoryHudItem> _parked_items;
  private Transform _content_group;
  private Transform _parked_group;
  private const int HISTORY_ITEM_SIZE = 15;
  private const int MAX_HISTORY_ITEMS = 10;
  private const float START_POSITION = 0.0f;
  private bool _recalc;
  private static bool _raycast_on = true;

  public bool raycastOn
  {
    get
    {
      return !MoveCamera.camera_drag_run && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2) && !MapBox.controlsLocked() && !MapBox.isControllingUnit() && !World.world.isAnyPowerSelected() && HistoryHud._raycast_on;
    }
    set => HistoryHud._raycast_on = value;
  }

  private void Awake()
  {
    HistoryHud.instance = this;
    this._content_group = ((Component) this).transform.Find("Scroll View/Viewport/Content");
    this._parked_group = ((Component) this).transform.Find("Scroll View/Viewport/Parked");
    this._parked_items = new ObjectPoolGenericMono<HistoryHudItem>(this._template_obj.GetComponent<HistoryHudItem>(), this._parked_group);
  }

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    ((Component) this._content_group).gameObject.GetComponent<RectTransform>().SetTop(0.0f);
    ((Component) this._content_group).gameObject.GetComponent<RectTransform>().SetLeft(0.0f);
  }

  private void OnDisable() => this.Clear();

  private void Update()
  {
    this.checkEnabled();
    if (!this._recalc)
      return;
    this._recalc = false;
    double num = (double) this.recalcPositions();
  }

  public static void disableRaycasts() => HistoryHud.instance.raycastOn = false;

  public static void enableRaycasts() => HistoryHud.instance.raycastOn = true;

  private float recalcPositions()
  {
    if (this._history_items.Count == 0)
      return 0.0f;
    float newBottom = 0.0f;
    float num1 = 0.0f;
    int num2 = 0;
    if (this._history_items.Count > 10)
      num2 = this._history_items.Count - 10;
    for (int index = 0; index < this._history_items.Count; ++index)
    {
      if (num2 > 0)
      {
        if ((double) this._history_items[index].target_bottom != (double) (num2 * -15))
          this._history_items[index].moveToAndDestroy((float) (num2 * -15));
        --num2;
      }
      else if (!this._history_items[index].isRemoving())
      {
        if ((double) this._history_items[index].target_bottom != (double) newBottom)
          this._history_items[index].moveTo(newBottom);
        newBottom += 15f;
      }
      else
        continue;
      num1 = -((Component) this._history_items[index]).GetComponent<RectTransform>().offsetMax.y;
    }
    return (double) num1 >= (double) newBottom ? num1 + 15f : newBottom;
  }

  private bool checkEnabled()
  {
    if (!PlayerConfig.optionBoolEnabled("history_log"))
    {
      if (((Component) this).gameObject.activeSelf)
        ((Component) this).gameObject.SetActive(false);
      return false;
    }
    if (!((Component) this).gameObject.activeSelf)
      ((Component) this).gameObject.SetActive(true);
    return true;
  }

  public void newHistory(WorldLogMessage pMessage)
  {
    if (!this.checkEnabled())
      return;
    this.newText(pMessage);
    ((Component) this).gameObject.SetActive(true);
  }

  public void makeInactive(HistoryHudItem historyItem)
  {
    this._parked_items.resetParent(historyItem);
    this._parked_items.release(historyItem);
    this._history_items.Remove(historyItem);
    this._recalc = true;
  }

  public void Clear()
  {
    for (int index = this._history_items.Count - 1; index >= 0; --index)
      this.makeInactive(this._history_items[index]);
  }

  private void newText(WorldLogMessage pMessage)
  {
    HistoryHudItem next = this._parked_items.getNext();
    ((Component) next).transform.SetParent(this._content_group);
    ((Object) ((Component) next).gameObject).name = "HistoryItem " + (this._history_items.Count + 1).ToString();
    ((Component) next).gameObject.SetActive(true);
    RectTransform component = ((Component) next).GetComponent<RectTransform>();
    ((Transform) component).localScale = Vector3.one;
    ((Transform) component).localPosition = Vector3.zero;
    component.SetLeft(0.0f);
    float pTop = this.recalcPositions();
    component.SetTop(pTop);
    component.sizeDelta = new Vector2(component.sizeDelta.x, 15f);
    next.target_bottom = pTop;
    next.setMessage(pMessage);
    this._history_items.Add(next);
    this._recalc = true;
  }
}
