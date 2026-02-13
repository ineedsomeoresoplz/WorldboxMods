// Decompiled with JetBrains decompiler
// Type: LayoutGroupExt.LayoutGroupExtended
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

#nullable disable
namespace LayoutGroupExt;

[DisallowMultipleComponent]
[ExecuteAlways]
[RequireComponent(typeof (RectTransform))]
public abstract class LayoutGroupExtended : LayoutGroup
{
  [SerializeField]
  public float moveDuration = 0.15f;
  [Tooltip("Will position the n items immediately, animating the next ones.")]
  [SerializeField]
  public int delayItems = 1;
  private Dictionary<RectTransform, TweenerCore<Vector2, Vector2, VectorOptions>> RectPositionXTweens = new Dictionary<RectTransform, TweenerCore<Vector2, Vector2, VectorOptions>>();
  private Dictionary<RectTransform, TweenerCore<Vector2, Vector2, VectorOptions>> RectPositionYTweens = new Dictionary<RectTransform, TweenerCore<Vector2, Vector2, VectorOptions>>();
  private static List<RectTransform> _to_remove = new List<RectTransform>();
  internal List<RectTransform> m_Children = new List<RectTransform>();
  internal Dictionary<int, List<RectTransform>> m_Axis = new Dictionary<int, List<RectTransform>>()
  {
    {
      0,
      new List<RectTransform>()
    },
    {
      1,
      new List<RectTransform>()
    }
  };
  internal Vector2[] m_Positions = new Vector2[0];
  internal RectTransform[] m_Sort = new RectTransform[0];
  internal Dictionary<RectTransform, Vector2> m_Grid_Positions = new Dictionary<RectTransform, Vector2>();
  internal Dictionary<RectTransform, Vector2> m_Grid_Anchors = new Dictionary<RectTransform, Vector2>();
  private int _skip_frame = -1;
  private static RectTransform _highlighter_prefab;
  private ObjectPoolGenericMono<RectTransform> _pool_highlighter;

  protected void SetChildAlongAxis(RectTransform rect, int axis, float pos)
  {
    if (Object.op_Equality((Object) rect, (Object) null))
      return;
    this.SetChildAlongAxisWithScale(rect, axis, pos, 1f);
  }

  public virtual void CalculateLayoutInputHorizontal()
  {
    if (this._skip_frame == Time.frameCount)
    {
      this.SetDirty();
    }
    else
    {
      bool flag = this.rectChildren.Count == 0;
      this.rectChildren.Clear();
      List<Component> componentList = CollectionPool<List<Component>, Component>.Get();
      for (int index = 0; index < ((Transform) this.rectTransform).childCount; ++index)
      {
        if (Application.isPlaying & flag && this.rectChildren.Count == this.delayItems)
        {
          this._skip_frame = Time.frameCount;
          this.SetDirty();
          break;
        }
        RectTransform child = ((Transform) this.rectTransform).GetChild(index) as RectTransform;
        if (!Object.op_Equality((Object) child, (Object) null) && ((Component) child).gameObject.activeInHierarchy)
        {
          ((Component) child).GetComponents(typeof (ILayoutIgnorer), componentList);
          if (componentList.Count == 0)
          {
            this.rectChildren.Add(child);
          }
          else
          {
            foreach (ILayoutIgnorer ilayoutIgnorer in componentList)
            {
              if (!ilayoutIgnorer.ignoreLayout && ((Behaviour) ilayoutIgnorer).enabled)
              {
                this.rectChildren.Add(child);
                break;
              }
            }
          }
        }
      }
      CollectionPool<List<Component>, Component>.Release(componentList);
      ((DrivenRectTransformTracker) ref this.m_Tracker).Clear();
    }
  }

  protected void SetChildAlongAxisWithScale(
    RectTransform rect,
    int axis,
    float pos,
    float scaleFactor)
  {
    if (Object.op_Equality((Object) rect, (Object) null))
      return;
    ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, rect, (DrivenTransformProperties) (3840 /*0x0F00*/ | (axis == 0 ? 2 : 4)));
    rect.anchorMin = Vector2.up;
    rect.anchorMax = Vector2.up;
    Vector2 anchoredPosition;
    if (!this.m_Grid_Anchors.TryGetValue(rect, out anchoredPosition) || !Application.isPlaying)
    {
      anchoredPosition = rect.anchoredPosition;
      this.m_Grid_Anchors[rect] = anchoredPosition;
    }
    ref Vector2 local = ref anchoredPosition;
    int num1 = axis;
    double num2;
    if (axis != 0)
    {
      double num3 = -(double) pos;
      Vector2 vector2 = rect.sizeDelta;
      double num4 = (double) ((Vector2) ref vector2)[axis];
      vector2 = rect.pivot;
      double num5 = 1.0 - (double) ((Vector2) ref vector2)[axis];
      double num6 = num4 * num5 * (double) scaleFactor;
      num2 = num3 - num6;
    }
    else
    {
      double num7 = (double) pos;
      Vector2 vector2 = rect.sizeDelta;
      double num8 = (double) ((Vector2) ref vector2)[axis];
      vector2 = rect.pivot;
      double num9 = (double) ((Vector2) ref vector2)[axis];
      double num10 = num8 * num9 * (double) scaleFactor;
      num2 = num7 + num10;
    }
    ((Vector2) ref local)[num1] = (float) num2;
    this.SetPosition(rect, anchoredPosition, axis);
  }

  protected void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
  {
    if (Object.op_Equality((Object) rect, (Object) null))
      return;
    this.SetChildAlongAxisWithScale(rect, axis, pos, size, 1f);
  }

  protected void SetChildAlongAxisWithScale(
    RectTransform rect,
    int axis,
    float pos,
    float size,
    float scaleFactor)
  {
    if (Object.op_Equality((Object) rect, (Object) null))
      return;
    ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, rect, (DrivenTransformProperties) (3840 /*0x0F00*/ | (axis == 0 ? 4098 : 8196)));
    rect.anchorMin = Vector2.up;
    rect.anchorMax = Vector2.up;
    Vector2 sizeDelta = rect.sizeDelta;
    ((Vector2) ref sizeDelta)[axis] = size;
    rect.sizeDelta = sizeDelta;
    Vector2 anchoredPosition;
    if (!this.m_Grid_Anchors.TryGetValue(rect, out anchoredPosition) || !Application.isPlaying)
    {
      anchoredPosition = rect.anchoredPosition;
      this.m_Grid_Anchors[rect] = anchoredPosition;
    }
    ref Vector2 local = ref anchoredPosition;
    int num1 = axis;
    double num2;
    if (axis != 0)
    {
      double num3 = -(double) pos;
      double num4 = (double) size;
      Vector2 pivot = rect.pivot;
      double num5 = 1.0 - (double) ((Vector2) ref pivot)[axis];
      double num6 = num4 * num5 * (double) scaleFactor;
      num2 = num3 - num6;
    }
    else
    {
      double num7 = (double) pos;
      double num8 = (double) size;
      Vector2 pivot = rect.pivot;
      double num9 = (double) ((Vector2) ref pivot)[axis];
      double num10 = num8 * num9 * (double) scaleFactor;
      num2 = num7 + num10;
    }
    ((Vector2) ref local)[num1] = (float) num2;
    this.SetPosition(rect, anchoredPosition, axis);
  }

  public void SetPosition(RectTransform rect, Vector2 pos, int axis)
  {
    if (!Application.isPlaying)
    {
      rect.anchoredPosition = pos;
    }
    else
    {
      if (!this.m_Axis[axis].Contains(rect))
      {
        if (this.m_Axis[axis].Count >= this.delayItems)
        {
          Vector2 vector2_1 = Vector2.zero;
          float num1 = float.MaxValue;
          for (int index = this.m_Axis[axis].Count - 1; index >= 0; --index)
          {
            Vector2 vector2_2 = this.m_Axis[axis][index].anchoredPosition;
            if (Vector2.op_Equality(vector2_2, Vector2.zero))
              vector2_2 = this.m_Grid_Anchors[this.m_Axis[axis][index]];
            float num2 = Vector2.Distance(vector2_2, pos);
            if ((double) num2 < (double) num1)
            {
              num1 = num2;
              vector2_1 = vector2_2;
            }
          }
          Vector2 vector2_3 = Vector2.op_Subtraction(vector2_1, pos);
          rect.anchoredPosition = (double) Mathf.Abs(vector2_3.y) <= (double) Mathf.Abs(vector2_3.x) ? Vector2.op_Subtraction(vector2_1, new Vector2(1f, 0.0f)) : Vector2.op_Addition(vector2_1, new Vector2(0.0f, 1f));
        }
        else
          rect.anchoredPosition = pos;
        this.m_Axis[axis].Add(rect);
        if (!this.m_Children.Contains(rect))
          this.m_Children.Add(rect);
      }
      this.m_Grid_Anchors[rect] = pos;
      Vector2 anchoredPosition = rect.anchoredPosition;
      rect.anchoredPosition = pos;
      this.m_Grid_Positions[rect] = Vector2.op_Implicit(((Transform) rect).position);
      rect.anchoredPosition = anchoredPosition;
      if (this.m_Children.Count != this.m_Positions.Length)
      {
        this.m_Positions = new Vector2[this.m_Children.Count];
        this.m_Sort = new RectTransform[this.m_Children.Count];
      }
      this.m_Children.Sort((Comparison<RectTransform>) ((a, b) => ((Transform) a).GetSiblingIndex().CompareTo(((Transform) b).GetSiblingIndex())));
      for (int index = 0; index < this.m_Children.Count; ++index)
      {
        Vector2 gridPosition = this.m_Grid_Positions[this.m_Children[index]];
        this.m_Positions[index] = gridPosition;
        this.m_Sort[index] = this.m_Children[index];
      }
      Dictionary<RectTransform, TweenerCore<Vector2, Vector2, VectorOptions>> dict;
      TweenerCore<Vector2, Vector2, VectorOptions> tween;
      switch (axis)
      {
        case 0:
          dict = this.RectPositionXTweens;
          TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore1;
          if (dict.TryGetValue(rect, out tweenerCore1) && TweenExtensions.IsActive((Tween) tweenerCore1))
          {
            if (Mathf.Approximately(tweenerCore1.endValue.x, pos.x))
              return;
            TweenExtensions.Kill((Tween) tweenerCore1, false);
          }
          if (Mathf.Approximately(rect.anchoredPosition.x, pos.x))
            return;
          tween = DOTweenModuleUI.DOAnchorPosX(rect, pos.x, this.moveDuration, false);
          break;
        case 1:
          dict = this.RectPositionYTweens;
          TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore2;
          if (dict.TryGetValue(rect, out tweenerCore2) && TweenExtensions.IsActive((Tween) tweenerCore2))
          {
            if (Mathf.Approximately(tweenerCore2.endValue.y, pos.y))
              return;
            TweenExtensions.Kill((Tween) tweenerCore2, false);
          }
          if (Mathf.Approximately(rect.anchoredPosition.y, pos.y))
            return;
          tween = DOTweenModuleUI.DOAnchorPosY(rect, pos.y, this.moveDuration, false);
          break;
        default:
          return;
      }
      TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore3 = tween;
      ((Tween) tweenerCore3).onKill = ((Tween) tweenerCore3).onKill + (TweenCallback) (() =>
      {
        if (!dict.ContainsKey(rect) || dict[rect] != tween)
          return;
        dict.Remove(rect);
      });
      TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore4 = tween;
      ((Tween) tweenerCore4).onComplete = ((Tween) tweenerCore4).onComplete + (TweenCallback) (() =>
      {
        LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
        dict.Remove(rect);
      });
      dict[rect] = tween;
    }
  }

  protected virtual void OnEnable()
  {
    base.OnEnable();
    ScrollWindow.addCallbackShow(new ScrollWindowNameAction(this.setDirty));
    ScrollWindow.addCallbackShowFinished(new ScrollWindowNameAction(this.setDirty));
  }

  protected virtual void OnDisable()
  {
    // ISSUE: unable to decompile the method.
  }

  private void LateUpdate()
  {
    foreach (RectTransform child in this.m_Children)
    {
      if (!this.rectChildren.Contains(child) || !((Component) child).gameObject.activeInHierarchy)
        LayoutGroupExtended._to_remove.Add(child);
    }
    foreach (RectTransform key in LayoutGroupExtended._to_remove)
    {
      this.m_Children.Remove(key);
      this.m_Axis[0].Remove(key);
      this.m_Axis[1].Remove(key);
      this.m_Grid_Positions.Remove(key);
      this.m_Grid_Anchors.Remove(key);
    }
    LayoutGroupExtended._to_remove.Clear();
  }

  private void setDirty(string pWindowName) => this.SetDirty();

  private void DebugInit()
  {
    if (Object.op_Equality((Object) LayoutGroupExtended._highlighter_prefab, (Object) null))
      LayoutGroupExtended._highlighter_prefab = Object.Instantiate<RectTransform>(Resources.Load<RectTransform>("ui/selector"));
    if (this._pool_highlighter == null)
      this._pool_highlighter = new ObjectPoolGenericMono<RectTransform>(LayoutGroupExtended._highlighter_prefab, ((Component) this).transform);
    this._pool_highlighter.clear();
  }

  protected virtual void Update()
  {
    if (!Application.isPlaying)
      return;
    if (!DebugConfig.isOn(DebugOption.ShowLayoutGroupGrid))
    {
      this._pool_highlighter?.clear();
    }
    else
    {
      this.DebugInit();
      for (int index = 0; index < this.m_Positions.Length; ++index)
      {
        Vector2 position = this.m_Positions[index];
        RectTransform next = this._pool_highlighter.getNext();
        ((Transform) next).localScale = ((Transform) this.m_Children[0]).localScale;
        ((Graphic) ((Component) ((Transform) next).GetChild(0)).GetComponent<Image>()).color = new Color(1f, 0.0f, 0.0f, 0.25f);
        ((Object) ((Component) next).gameObject).name = $"m_positions {index.ToString()} {position.ToString()}";
        ((Transform) next).position = Vector2.op_Implicit(position);
      }
      for (int index = 0; index < this.m_Sort.Length; ++index)
      {
        Vector2 gridAnchor = this.m_Grid_Anchors[this.m_Sort[index]];
        RectTransform next = this._pool_highlighter.getNext();
        ((Transform) next).localScale = ((Transform) this.m_Children[0]).localScale;
        ((Graphic) ((Component) ((Transform) next).GetChild(0)).GetComponent<Image>()).color = new Color(0.0f, 1f, 0.0f, 0.25f);
        ((Object) ((Component) next).gameObject).name = $"m_Grid_Anchors {index.ToString()} {gridAnchor.ToString()}";
        next.anchoredPosition = gridAnchor;
      }
      for (int index = 0; index < this.m_Sort.Length; ++index)
      {
        Vector2 gridPosition = this.m_Grid_Positions[this.m_Sort[index]];
        RectTransform next = this._pool_highlighter.getNext();
        ((Transform) next).localScale = ((Transform) this.m_Children[0]).localScale;
        ((Graphic) ((Component) ((Transform) next).GetChild(0)).GetComponent<Image>()).color = new Color(0.0f, 0.0f, 1f, 0.25f);
        ((Object) ((Component) next).gameObject).name = $"m_Grid_Positions {index.ToString()} {gridPosition.ToString()}";
        ((Transform) next).position = Vector2.op_Implicit(gridPosition);
      }
    }
  }
}
