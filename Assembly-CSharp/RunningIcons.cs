// Decompiled with JetBrains decompiler
// Type: RunningIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class RunningIcons : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  [SerializeField]
  private RunningIcons.Direction _direction;
  [SerializeField]
  private float speed;
  private IconGetter _get_next_icon;
  private RunningIconCallback _next_item_action;
  private RunningIconCallback _prev_item_action;
  private float _first_position_x = float.MaxValue;
  private float _last_position_x = float.MinValue;
  private int _last_index;
  private List<RunningIcon> _icons = new List<RunningIcon>();
  private bool _state;
  private bool _initialized;
  private float _step;

  public void addIcon(RunningIcon pIcon) => this._icons.Add(pIcon);

  public void init(RunningIconCallback pNextItemAction, RunningIconCallback pPrevItemAction)
  {
    this._initialized = true;
    this._next_item_action = pNextItemAction;
    this._prev_item_action = pPrevItemAction;
    foreach (Transform transform in ((Component) this).transform)
    {
      if ((double) this._first_position_x > (double) transform.localPosition.x)
        this._first_position_x = transform.localPosition.x;
      if ((double) this._last_position_x < (double) transform.localPosition.x)
        this._last_position_x = transform.localPosition.x;
    }
    float num = float.MaxValue;
    foreach (Transform transform in ((Component) this).transform)
    {
      if ((double) transform.localPosition.x != (double) this._first_position_x && (double) num > (double) transform.localPosition.x)
        num = transform.localPosition.x;
    }
    this._step = num - this._first_position_x;
    this._last_position_x += this._step;
    this._last_index = ((Component) this).transform.childCount - 1;
    this.toggle(true);
  }

  private void OnEnable() => this.reset();

  private void reset()
  {
    if (this._icons == null)
      return;
    foreach (Component icon in this._icons)
      this._next_item_action(icon.transform);
  }

  private void Update()
  {
    if (!this._initialized || !this._state)
      return;
    this.moveBy(this.speed, this._direction);
  }

  public void moveBy(float pSpeed, RunningIcons.Direction pDirection, int pCounter = 0)
  {
    int num1 = 0;
    int num2 = this._last_index;
    float step = this._step;
    if (pDirection == RunningIcons.Direction.Left)
    {
      num1 = this._last_index;
      num2 = 0;
      pSpeed *= -1f;
      step *= -1f;
    }
    foreach (Transform transform in ((Component) this).transform)
    {
      Vector3 localPosition = transform.localPosition;
      localPosition.x += pSpeed;
      transform.localPosition = localPosition;
    }
    while (true)
    {
      Transform child1 = ((Component) this).transform.GetChild(num2);
      Vector3 localPosition = child1.localPosition;
      if (this.endReached(localPosition.x, pDirection))
      {
        Transform child2 = ((Component) this).transform.GetChild(num1);
        localPosition.x = child2.localPosition.x - step;
        child1.localPosition = localPosition;
        child1.SetSiblingIndex(num1);
        if (pDirection == RunningIcons.Direction.Left)
          this._prev_item_action(child1);
        else
          this._next_item_action(child1);
      }
      else
        break;
    }
  }

  public void toggle(bool pState) => this._state = pState;

  public bool getState() => this._state;

  private bool endReached(float pPosition, RunningIcons.Direction pDirection)
  {
    return pDirection == RunningIcons.Direction.Right ? (double) pPosition >= (double) this._last_position_x : (double) pPosition <= (double) this._first_position_x;
  }

  public void OnSelect(BaseEventData pEventData)
  {
    if (InputHelpers.mouseSupported)
      return;
    this.toggle(false);
  }

  public void OnDeselect(BaseEventData pEventData)
  {
    if (InputHelpers.mouseSupported)
      return;
    this.toggle(true);
  }

  public enum Direction
  {
    Left,
    Right,
  }
}
