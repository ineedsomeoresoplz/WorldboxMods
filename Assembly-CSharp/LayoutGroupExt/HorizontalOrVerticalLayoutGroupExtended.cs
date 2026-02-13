// Decompiled with JetBrains decompiler
// Type: LayoutGroupExt.HorizontalOrVerticalLayoutGroupExtended
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace LayoutGroupExt;

[ExecuteAlways]
public abstract class HorizontalOrVerticalLayoutGroupExtended : LayoutGroupExtended
{
  [SerializeField]
  protected float m_Spacing;
  [SerializeField]
  protected bool m_ChildForceExpandWidth = true;
  [SerializeField]
  protected bool m_ChildForceExpandHeight = true;
  [SerializeField]
  protected bool m_ChildControlWidth = true;
  [SerializeField]
  protected bool m_ChildControlHeight = true;
  [SerializeField]
  protected bool m_ChildScaleWidth;
  [SerializeField]
  protected bool m_ChildScaleHeight;
  [SerializeField]
  protected bool m_ReverseArrangement;

  public float spacing
  {
    get => this.m_Spacing;
    set => this.SetProperty<float>(ref this.m_Spacing, value);
  }

  public bool childForceExpandWidth
  {
    get => this.m_ChildForceExpandWidth;
    set => this.SetProperty<bool>(ref this.m_ChildForceExpandWidth, value);
  }

  public bool childForceExpandHeight
  {
    get => this.m_ChildForceExpandHeight;
    set => this.SetProperty<bool>(ref this.m_ChildForceExpandHeight, value);
  }

  public bool childControlWidth
  {
    get => this.m_ChildControlWidth;
    set => this.SetProperty<bool>(ref this.m_ChildControlWidth, value);
  }

  public bool childControlHeight
  {
    get => this.m_ChildControlHeight;
    set => this.SetProperty<bool>(ref this.m_ChildControlHeight, value);
  }

  public bool childScaleWidth
  {
    get => this.m_ChildScaleWidth;
    set => this.SetProperty<bool>(ref this.m_ChildScaleWidth, value);
  }

  public bool childScaleHeight
  {
    get => this.m_ChildScaleHeight;
    set => this.SetProperty<bool>(ref this.m_ChildScaleHeight, value);
  }

  public bool reverseArrangement
  {
    get => this.m_ReverseArrangement;
    set => this.SetProperty<bool>(ref this.m_ReverseArrangement, value);
  }

  protected void CalcAlongAxis(int axis, bool isVertical)
  {
    float num1 = axis == 0 ? (float) this.padding.horizontal : (float) this.padding.vertical;
    bool controlSize = axis == 0 ? this.m_ChildControlWidth : this.m_ChildControlHeight;
    bool flag1 = axis == 0 ? this.m_ChildScaleWidth : this.m_ChildScaleHeight;
    bool childForceExpand = axis == 0 ? this.m_ChildForceExpandWidth : this.m_ChildForceExpandHeight;
    float num2 = num1;
    float num3 = num1;
    float num4 = 0.0f;
    bool flag2 = isVertical ^ axis == 1;
    int count = this.rectChildren.Count;
    for (int index = 0; index < count; ++index)
    {
      RectTransform rectChild = this.rectChildren[index];
      float min;
      float preferred;
      float flexible;
      this.GetChildSizes(rectChild, axis, controlSize, childForceExpand, out min, out preferred, out flexible);
      if (flag1)
      {
        Vector3 localScale = ((Transform) rectChild).localScale;
        float num5 = ((Vector3) ref localScale)[axis];
        min *= num5;
        preferred *= num5;
        flexible *= num5;
      }
      if (flag2)
      {
        num2 = Mathf.Max(min + num1, num2);
        num3 = Mathf.Max(preferred + num1, num3);
        num4 = Mathf.Max(flexible, num4);
      }
      else
      {
        num2 += min + this.spacing;
        num3 += preferred + this.spacing;
        num4 += flexible;
      }
    }
    if (!flag2 && this.rectChildren.Count > 0)
    {
      num2 -= this.spacing;
      num3 -= this.spacing;
    }
    float num6 = Mathf.Max(num2, num3);
    this.SetLayoutInputForAxis(num2, num6, num4, axis);
  }

  protected void SetChildrenAlongAxis(int axis, bool isVertical)
  {
    Rect rect = this.rectTransform.rect;
    Vector2 size1 = ((Rect) ref rect).size;
    float num1 = ((Vector2) ref size1)[axis];
    bool controlSize = axis == 0 ? this.m_ChildControlWidth : this.m_ChildControlHeight;
    bool flag = axis == 0 ? this.m_ChildScaleWidth : this.m_ChildScaleHeight;
    bool childForceExpand = axis == 0 ? this.m_ChildForceExpandWidth : this.m_ChildForceExpandHeight;
    float alignmentOnAxis = this.GetAlignmentOnAxis(axis);
    int num2 = isVertical ^ axis == 1 ? 1 : 0;
    int num3 = this.m_ReverseArrangement ? this.rectChildren.Count - 1 : 0;
    int count = this.m_ReverseArrangement ? 0 : this.rectChildren.Count;
    int num4 = this.m_ReverseArrangement ? -1 : 1;
    if (num2 != 0)
    {
      float num5 = num1 - (axis == 0 ? (float) this.padding.horizontal : (float) this.padding.vertical);
      for (int index = num3; (this.m_ReverseArrangement ? (index >= count ? 1 : 0) : (index < count ? 1 : 0)) != 0; index += num4)
      {
        RectTransform rectChild = this.rectChildren[index];
        float min;
        float preferred;
        float flexible;
        this.GetChildSizes(rectChild, axis, controlSize, childForceExpand, out min, out preferred, out flexible);
        double num6;
        if (!flag)
        {
          num6 = 1.0;
        }
        else
        {
          Vector3 localScale = ((Transform) rectChild).localScale;
          num6 = (double) ((Vector3) ref localScale)[axis];
        }
        float scaleFactor = (float) num6;
        float size2 = Mathf.Clamp(num5, min, (double) flexible > 0.0 ? num1 : preferred);
        float startOffset = this.GetStartOffset(axis, size2 * scaleFactor);
        if (controlSize)
        {
          this.SetChildAlongAxisWithScale(rectChild, axis, startOffset, size2, scaleFactor);
        }
        else
        {
          double num7 = (double) size2;
          Vector2 sizeDelta = rectChild.sizeDelta;
          double num8 = (double) ((Vector2) ref sizeDelta)[axis];
          float num9 = (float) (num7 - num8) * alignmentOnAxis;
          this.SetChildAlongAxisWithScale(rectChild, axis, startOffset + num9, scaleFactor);
        }
      }
    }
    else
    {
      float pos = axis == 0 ? (float) this.padding.left : (float) this.padding.top;
      float num10 = 0.0f;
      float num11 = num1 - this.GetTotalPreferredSize(axis);
      if ((double) num11 > 0.0)
      {
        if ((double) this.GetTotalFlexibleSize(axis) == 0.0)
          pos = this.GetStartOffset(axis, this.GetTotalPreferredSize(axis) - (axis == 0 ? (float) this.padding.horizontal : (float) this.padding.vertical));
        else if ((double) this.GetTotalFlexibleSize(axis) > 0.0)
          num10 = num11 / this.GetTotalFlexibleSize(axis);
      }
      float num12 = 0.0f;
      if ((double) this.GetTotalMinSize(axis) != (double) this.GetTotalPreferredSize(axis))
        num12 = Mathf.Clamp01((float) (((double) num1 - (double) this.GetTotalMinSize(axis)) / ((double) this.GetTotalPreferredSize(axis) - (double) this.GetTotalMinSize(axis))));
      for (int index = num3; (this.m_ReverseArrangement ? (index >= count ? 1 : 0) : (index < count ? 1 : 0)) != 0; index += num4)
      {
        RectTransform rectChild = this.rectChildren[index];
        float min;
        float preferred;
        float flexible;
        this.GetChildSizes(rectChild, axis, controlSize, childForceExpand, out min, out preferred, out flexible);
        double num13;
        if (!flag)
        {
          num13 = 1.0;
        }
        else
        {
          Vector3 localScale = ((Transform) rectChild).localScale;
          num13 = (double) ((Vector3) ref localScale)[axis];
        }
        float scaleFactor = (float) num13;
        float size3 = Mathf.Lerp(min, preferred, num12) + flexible * num10;
        if (controlSize)
        {
          this.SetChildAlongAxisWithScale(rectChild, axis, pos, size3, scaleFactor);
        }
        else
        {
          double num14 = (double) size3;
          Vector2 sizeDelta = rectChild.sizeDelta;
          double num15 = (double) ((Vector2) ref sizeDelta)[axis];
          float num16 = (float) (num14 - num15) * alignmentOnAxis;
          this.SetChildAlongAxisWithScale(rectChild, axis, pos + num16, scaleFactor);
        }
        pos += size3 * scaleFactor + this.spacing;
      }
    }
  }

  private void GetChildSizes(
    RectTransform child,
    int axis,
    bool controlSize,
    bool childForceExpand,
    out float min,
    out float preferred,
    out float flexible)
  {
    if (!controlSize)
    {
      ref float local = ref min;
      Vector2 sizeDelta = child.sizeDelta;
      double num = (double) ((Vector2) ref sizeDelta)[axis];
      local = (float) num;
      preferred = min;
      flexible = 0.0f;
    }
    else
    {
      min = LayoutUtility.GetMinSize(child, axis);
      preferred = LayoutUtility.GetPreferredSize(child, axis);
      flexible = LayoutUtility.GetFlexibleSize(child, axis);
    }
    if (!childForceExpand)
      return;
    flexible = Mathf.Max(flexible, 1f);
  }
}
