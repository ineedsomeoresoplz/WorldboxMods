// Decompiled with JetBrains decompiler
// Type: LayoutGroupExt.GridLayoutGroupExtended
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
namespace LayoutGroupExt;

[AddComponentMenu("Layout/Grid Layout Group ( Extended )", 152)]
public class GridLayoutGroupExtended : LayoutGroupExtended
{
  private TweenerCore<Vector3, Vector3, VectorOptions>[] _axis_tween = new TweenerCore<Vector3, Vector3, VectorOptions>[2];
  [SerializeField]
  protected GridLayoutGroupExtended.Corner m_StartCorner;
  [SerializeField]
  protected GridLayoutGroupExtended.Axis m_StartAxis;
  [SerializeField]
  protected Vector2 m_CellSize = new Vector2(100f, 100f);
  [SerializeField]
  protected Vector2 m_Spacing = Vector2.zero;
  [SerializeField]
  protected GridLayoutGroupExtended.Constraint m_Constraint;
  [SerializeField]
  protected int m_ConstraintCount = 2;

  public GridLayoutGroupExtended.Corner startCorner
  {
    get => this.m_StartCorner;
    set => this.SetProperty<GridLayoutGroupExtended.Corner>(ref this.m_StartCorner, value);
  }

  public GridLayoutGroupExtended.Axis startAxis
  {
    get => this.m_StartAxis;
    set => this.SetProperty<GridLayoutGroupExtended.Axis>(ref this.m_StartAxis, value);
  }

  public Vector2 cellSize
  {
    get => this.m_CellSize;
    set => this.SetProperty<Vector2>(ref this.m_CellSize, value);
  }

  public Vector2 spacing
  {
    get => this.m_Spacing;
    set => this.SetProperty<Vector2>(ref this.m_Spacing, value);
  }

  public GridLayoutGroupExtended.Constraint constraint
  {
    get => this.m_Constraint;
    set => this.SetProperty<GridLayoutGroupExtended.Constraint>(ref this.m_Constraint, value);
  }

  public int constraintCount
  {
    get => this.m_ConstraintCount;
    set => this.SetProperty<int>(ref this.m_ConstraintCount, Mathf.Max(1, value));
  }

  protected GridLayoutGroupExtended()
  {
  }

  public override void CalculateLayoutInputHorizontal()
  {
    base.CalculateLayoutInputHorizontal();
    int constraintCount;
    int num;
    if (this.m_Constraint == GridLayoutGroupExtended.Constraint.FixedColumnCount)
      num = constraintCount = this.m_ConstraintCount;
    else if (this.m_Constraint == GridLayoutGroupExtended.Constraint.FixedRowCount)
    {
      num = constraintCount = Mathf.CeilToInt((float) ((double) this.rectChildren.Count / (double) this.m_ConstraintCount - 1.0 / 1000.0));
    }
    else
    {
      num = 1;
      constraintCount = Mathf.CeilToInt(Mathf.Sqrt((float) this.rectChildren.Count));
    }
    this.SetLayoutInputForAxis((float) this.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float) num - this.spacing.x, (float) this.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float) constraintCount - this.spacing.x, -1f, 0);
  }

  public virtual void CalculateLayoutInputVertical()
  {
    int constraintCount;
    if (this.m_Constraint == GridLayoutGroupExtended.Constraint.FixedColumnCount)
      constraintCount = Mathf.CeilToInt((float) ((double) this.rectChildren.Count / (double) this.m_ConstraintCount - 1.0 / 1000.0));
    else if (this.m_Constraint == GridLayoutGroupExtended.Constraint.FixedRowCount)
    {
      constraintCount = this.m_ConstraintCount;
    }
    else
    {
      Rect rect = this.rectTransform.rect;
      constraintCount = Mathf.CeilToInt((float) this.rectChildren.Count / (float) Mathf.Max(1, Mathf.FloorToInt((float) (((double) ((Rect) ref rect).width - (double) this.padding.horizontal + (double) this.spacing.x + 1.0 / 1000.0) / ((double) this.cellSize.x + (double) this.spacing.x)))));
    }
    float num = (float) this.padding.vertical + (this.cellSize.y + this.spacing.y) * (float) constraintCount - this.spacing.y;
    this.TweenLayoutInputForAxis(num, num, -1f, 1);
  }

  private void TweenLayoutInputForAxis(
    float totalMin,
    float totalPreferred,
    float totalFlexible,
    int axis)
  {
    if (!Application.isPlaying)
    {
      this.SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
    }
    else
    {
      Vector3 endValue;
      // ISSUE: explicit constructor call
      ((Vector3) ref endValue).\u002Ector(totalMin, totalPreferred, totalFlexible);
      Vector3 vector3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(this.GetTotalMinSize(axis), this.GetTotalPreferredSize(axis), this.GetTotalFlexibleSize(axis));
      if (Vector3.op_Equality(endValue, vector3))
        return;
      TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = this._axis_tween[axis];
      if (TweenExtensions.IsActive((Tween) tweenerCore))
      {
        if (Vector3.op_Equality(tweenerCore.endValue, endValue))
          return;
        TweenExtensions.Kill((Tween) tweenerCore, false);
      }
      this._axis_tween[axis] = this.DOPreferredSize(endValue, this.moveDuration * 0.5f, axis);
    }
  }

  private TweenerCore<Vector3, Vector3, VectorOptions> DOPreferredSize(
    Vector3 endValue,
    float duration,
    int axis)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    GridLayoutGroupExtended.\u003C\u003Ec__DisplayClass32_0 cDisplayClass320 = new GridLayoutGroupExtended.\u003C\u003Ec__DisplayClass32_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass320.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass320.axis = axis;
    // ISSUE: method pointer
    // ISSUE: method pointer
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(new DOGetter<Vector3>((object) cDisplayClass320, __methodptr(\u003CDOPreferredSize\u003Eb__0)), new DOSetter<Vector3>((object) cDisplayClass320, __methodptr(\u003CDOPreferredSize\u003Eb__1)), endValue, duration);
    // ISSUE: reference to a compiler-generated method
    TweenSettingsExtensions.OnUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(tweenerCore, new TweenCallback(cDisplayClass320.\u003CDOPreferredSize\u003Eb__2));
    // ISSUE: reference to a compiler-generated method
    TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(tweenerCore, new TweenCallback(cDisplayClass320.\u003CDOPreferredSize\u003Eb__3));
    TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Vector3, VectorOptions>>(tweenerCore, (object) this);
    return tweenerCore;
  }

  protected override void OnDisable()
  {
    TweenExtensions.Kill((Tween) this._axis_tween[0], false);
    TweenExtensions.Kill((Tween) this._axis_tween[1], false);
    base.OnDisable();
  }

  public virtual void SetLayoutHorizontal() => this.SetCellsAlongAxis(0);

  public virtual void SetLayoutVertical() => this.SetCellsAlongAxis(1);

  private void SetCellsAlongAxis(int axis)
  {
    int count = this.rectChildren.Count;
    if (axis == 0)
    {
      for (int index = 0; index < count; ++index)
      {
        RectTransform rectChild = this.rectChildren[index];
        ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, rectChild, (DrivenTransformProperties) 16134);
        rectChild.anchorMin = Vector2.up;
        rectChild.anchorMax = Vector2.up;
        rectChild.sizeDelta = this.cellSize;
      }
    }
    else
    {
      Rect rect1 = this.rectTransform.rect;
      float x1 = ((Rect) ref rect1).size.x;
      Rect rect2 = this.rectTransform.rect;
      float y1 = ((Rect) ref rect2).size.y;
      int num1 = 1;
      int num2 = 1;
      if (this.m_Constraint == GridLayoutGroupExtended.Constraint.FixedColumnCount)
      {
        num1 = this.m_ConstraintCount;
        if (count > num1)
          num2 = count / num1 + (count % num1 > 0 ? 1 : 0);
      }
      else if (this.m_Constraint == GridLayoutGroupExtended.Constraint.FixedRowCount)
      {
        num2 = this.m_ConstraintCount;
        if (count > num2)
          num1 = count / num2 + (count % num2 > 0 ? 1 : 0);
      }
      else
      {
        num1 = (double) this.cellSize.x + (double) this.spacing.x > 0.0 ? Mathf.Max(1, Mathf.FloorToInt((float) (((double) x1 - (double) this.padding.horizontal + (double) this.spacing.x + 1.0 / 1000.0) / ((double) this.cellSize.x + (double) this.spacing.x)))) : int.MaxValue;
        num2 = (double) this.cellSize.y + (double) this.spacing.y > 0.0 ? Mathf.Max(1, Mathf.FloorToInt((float) (((double) y1 - (double) this.padding.vertical + (double) this.spacing.y + 1.0 / 1000.0) / ((double) this.cellSize.y + (double) this.spacing.y)))) : int.MaxValue;
      }
      int num3 = (int) this.startCorner % 2;
      int num4 = (int) this.startCorner / 2;
      int num5;
      int num6;
      int num7;
      if (this.startAxis == GridLayoutGroupExtended.Axis.Horizontal)
      {
        num5 = num1;
        num6 = Mathf.Clamp(num1, 1, count);
        num7 = this.m_Constraint != GridLayoutGroupExtended.Constraint.FixedRowCount ? Mathf.Clamp(num2, 1, Mathf.CeilToInt((float) count / (float) num5)) : Mathf.Min(num2, count);
      }
      else
      {
        num5 = num2;
        num7 = Mathf.Clamp(num2, 1, count);
        num6 = this.m_Constraint != GridLayoutGroupExtended.Constraint.FixedColumnCount ? Mathf.Clamp(num1, 1, Mathf.CeilToInt((float) count / (float) num5)) : Mathf.Min(num1, count);
      }
      Vector2 vector2_1;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_1).\u002Ector((float) ((double) num6 * (double) this.cellSize.x + (double) (num6 - 1) * (double) this.spacing.x), (float) ((double) num7 * (double) this.cellSize.y + (double) (num7 - 1) * (double) this.spacing.y));
      Vector2 vector2_2;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_2).\u002Ector(this.GetStartOffset(0, vector2_1.x), this.GetStartOffset(1, vector2_1.y));
      int num8 = 0;
      if (count > this.m_ConstraintCount && Mathf.CeilToInt((float) count / (float) num5) < this.m_ConstraintCount)
      {
        int num9 = this.m_ConstraintCount - Mathf.CeilToInt((float) count / (float) num5);
        num8 = num9 + Mathf.FloorToInt((float) num9 / ((float) num5 - 1f));
        if (count % num5 == 1)
          ++num8;
      }
      for (int index = 0; index < count; ++index)
      {
        int num10;
        int num11;
        if (this.startAxis == GridLayoutGroupExtended.Axis.Horizontal)
        {
          if (this.m_Constraint == GridLayoutGroupExtended.Constraint.FixedRowCount && count - index <= num8)
          {
            num10 = 0;
            num11 = this.m_ConstraintCount - (count - index);
          }
          else
          {
            num10 = index % num5;
            num11 = index / num5;
          }
        }
        else if (this.m_Constraint == GridLayoutGroupExtended.Constraint.FixedColumnCount && count - index <= num8)
        {
          num10 = this.m_ConstraintCount - (count - index);
          num11 = 0;
        }
        else
        {
          num10 = index / num5;
          num11 = index % num5;
        }
        if (num3 == 1)
          num10 = num6 - 1 - num10;
        if (num4 == 1)
          num11 = num7 - 1 - num11;
        RectTransform rectChild1 = this.rectChildren[index];
        double y2 = (double) vector2_2.y;
        Vector2 vector2_3 = this.cellSize;
        double num12 = (double) ((Vector2) ref vector2_3)[1];
        vector2_3 = this.spacing;
        double num13 = (double) ((Vector2) ref vector2_3)[1];
        double num14 = (num12 + num13) * (double) num11;
        double pos1 = y2 + num14;
        vector2_3 = this.cellSize;
        double size1 = (double) ((Vector2) ref vector2_3)[1];
        this.SetChildAlongAxis(rectChild1, 1, (float) pos1, (float) size1);
        RectTransform rectChild2 = this.rectChildren[index];
        double x2 = (double) vector2_2.x;
        vector2_3 = this.cellSize;
        double num15 = (double) ((Vector2) ref vector2_3)[0];
        vector2_3 = this.spacing;
        double num16 = (double) ((Vector2) ref vector2_3)[0];
        double num17 = (num15 + num16) * (double) num10;
        double pos2 = x2 + num17;
        vector2_3 = this.cellSize;
        double size2 = (double) ((Vector2) ref vector2_3)[0];
        this.SetChildAlongAxis(rectChild2, 0, (float) pos2, (float) size2);
      }
    }
  }

  public enum Corner
  {
    UpperLeft,
    UpperRight,
    LowerLeft,
    LowerRight,
  }

  public enum Axis
  {
    Horizontal,
    Vertical,
  }

  public enum Constraint
  {
    Flexible,
    FixedColumnCount,
    FixedRowCount,
  }
}
