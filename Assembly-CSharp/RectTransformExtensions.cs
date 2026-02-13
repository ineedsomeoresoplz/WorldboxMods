// Decompiled with JetBrains decompiler
// Type: RectTransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

#nullable disable
public static class RectTransformExtensions
{
  public static ListPool<RectTransform> getLayoutChildren(this RectTransform pRect)
  {
    List<Component> componentList = CollectionPool<List<Component>, Component>.Get();
    ListPool<RectTransform> layoutChildren = new ListPool<RectTransform>();
    int num = 0;
    for (int childCount = ((Transform) pRect).childCount; num < childCount; ++num)
    {
      RectTransform child = ((Transform) pRect).GetChild(num) as RectTransform;
      if (!Object.op_Equality((Object) child, (Object) null) && ((Component) child).gameObject.activeInHierarchy)
      {
        if (!((Component) child).HasComponent<ILayoutIgnorer>())
        {
          layoutChildren.Add(child);
        }
        else
        {
          ((Component) child).GetComponents(typeof (ILayoutIgnorer), componentList);
          if (componentList.Count == 0)
          {
            layoutChildren.Add(child);
          }
          else
          {
            for (int index = 0; index < componentList.Count; ++index)
            {
              if (!((ILayoutIgnorer) componentList[index]).ignoreLayout)
              {
                layoutChildren.Add(child);
                break;
              }
            }
            componentList.Clear();
          }
        }
      }
    }
    CollectionPool<List<Component>, Component>.Release(componentList);
    return layoutChildren;
  }

  public static void SetLeft(this RectTransform pRectTransform, float pLeft)
  {
    pRectTransform.offsetMin = new Vector2(pLeft, pRectTransform.offsetMin.y);
  }

  public static void SetRight(this RectTransform pRectTransform, float pRight)
  {
    pRectTransform.offsetMax = new Vector2(-pRight, pRectTransform.offsetMax.y);
  }

  public static void SetTop(this RectTransform pRectTransform, float pTop)
  {
    pRectTransform.offsetMax = new Vector2(pRectTransform.offsetMax.x, -pTop);
  }

  public static void SetBottom(this RectTransform pRectTransform, float pBottom)
  {
    pRectTransform.offsetMin = new Vector2(pRectTransform.offsetMin.x, pBottom);
  }

  public static void SetAnchor(
    this RectTransform pSource,
    AnchorPresets pAlign,
    float pOffsetX = 0.0f,
    float pOffsetY = 0.0f)
  {
    pSource.anchoredPosition = Vector2.op_Implicit(new Vector3(pOffsetX, pOffsetY, 0.0f));
    switch (pAlign)
    {
      case AnchorPresets.TopLeft:
        pSource.anchorMin = new Vector2(0.0f, 1f);
        pSource.anchorMax = new Vector2(0.0f, 1f);
        break;
      case AnchorPresets.TopCenter:
        pSource.anchorMin = new Vector2(0.5f, 1f);
        pSource.anchorMax = new Vector2(0.5f, 1f);
        break;
      case AnchorPresets.TopRight:
        pSource.anchorMin = new Vector2(1f, 1f);
        pSource.anchorMax = new Vector2(1f, 1f);
        break;
      case AnchorPresets.MiddleLeft:
        pSource.anchorMin = new Vector2(0.0f, 0.5f);
        pSource.anchorMax = new Vector2(0.0f, 0.5f);
        break;
      case AnchorPresets.MiddleCenter:
        pSource.anchorMin = new Vector2(0.5f, 0.5f);
        pSource.anchorMax = new Vector2(0.5f, 0.5f);
        break;
      case AnchorPresets.MiddleRight:
        pSource.anchorMin = new Vector2(1f, 0.5f);
        pSource.anchorMax = new Vector2(1f, 0.5f);
        break;
      case AnchorPresets.BottomLeft:
        pSource.anchorMin = new Vector2(0.0f, 0.0f);
        pSource.anchorMax = new Vector2(0.0f, 0.0f);
        break;
      case AnchorPresets.BottonCenter:
        pSource.anchorMin = new Vector2(0.5f, 0.0f);
        pSource.anchorMax = new Vector2(0.5f, 0.0f);
        break;
      case AnchorPresets.BottomRight:
        pSource.anchorMin = new Vector2(1f, 0.0f);
        pSource.anchorMax = new Vector2(1f, 0.0f);
        break;
      case AnchorPresets.VertStretchLeft:
        pSource.anchorMin = new Vector2(0.0f, 0.0f);
        pSource.anchorMax = new Vector2(0.0f, 1f);
        break;
      case AnchorPresets.VertStretchRight:
        pSource.anchorMin = new Vector2(1f, 0.0f);
        pSource.anchorMax = new Vector2(1f, 1f);
        break;
      case AnchorPresets.VertStretchCenter:
        pSource.anchorMin = new Vector2(0.5f, 0.0f);
        pSource.anchorMax = new Vector2(0.5f, 1f);
        break;
      case AnchorPresets.HorStretchTop:
        pSource.anchorMin = new Vector2(0.0f, 1f);
        pSource.anchorMax = new Vector2(1f, 1f);
        break;
      case AnchorPresets.HorStretchMiddle:
        pSource.anchorMin = new Vector2(0.0f, 0.5f);
        pSource.anchorMax = new Vector2(1f, 0.5f);
        break;
      case AnchorPresets.HorStretchBottom:
        pSource.anchorMin = new Vector2(0.0f, 0.0f);
        pSource.anchorMax = new Vector2(1f, 0.0f);
        break;
      case AnchorPresets.StretchAll:
        pSource.anchorMin = new Vector2(0.0f, 0.0f);
        pSource.anchorMax = new Vector2(1f, 1f);
        break;
    }
  }

  public static void SetPivot(this RectTransform pSource, PivotPresets pPreset, bool pKeepPosition = false)
  {
    Vector2 zero = Vector2.zero;
    switch (pPreset)
    {
      case PivotPresets.TopLeft:
        // ISSUE: explicit constructor call
        ((Vector2) ref zero).\u002Ector(0.0f, 1f);
        break;
      case PivotPresets.TopCenter:
        // ISSUE: explicit constructor call
        ((Vector2) ref zero).\u002Ector(0.5f, 1f);
        break;
      case PivotPresets.TopRight:
        // ISSUE: explicit constructor call
        ((Vector2) ref zero).\u002Ector(1f, 1f);
        break;
      case PivotPresets.MiddleLeft:
        // ISSUE: explicit constructor call
        ((Vector2) ref zero).\u002Ector(0.0f, 0.5f);
        break;
      case PivotPresets.MiddleCenter:
        // ISSUE: explicit constructor call
        ((Vector2) ref zero).\u002Ector(0.5f, 0.5f);
        break;
      case PivotPresets.MiddleRight:
        // ISSUE: explicit constructor call
        ((Vector2) ref zero).\u002Ector(1f, 0.5f);
        break;
      case PivotPresets.BottomLeft:
        // ISSUE: explicit constructor call
        ((Vector2) ref zero).\u002Ector(0.0f, 0.0f);
        break;
      case PivotPresets.BottomCenter:
        // ISSUE: explicit constructor call
        ((Vector2) ref zero).\u002Ector(0.5f, 0.0f);
        break;
      case PivotPresets.BottomRight:
        // ISSUE: explicit constructor call
        ((Vector2) ref zero).\u002Ector(1f, 0.0f);
        break;
    }
    if (!pKeepPosition)
    {
      pSource.pivot = zero;
    }
    else
    {
      Vector3 vector3_1 = Vector2.op_Implicit(Vector2.op_Subtraction(pSource.pivot, zero));
      ref Vector3 local = ref vector3_1;
      Rect rect = pSource.rect;
      Vector3 vector3_2 = Vector2.op_Implicit(((Rect) ref rect).size);
      ((Vector3) ref local).Scale(vector3_2);
      ((Vector3) ref vector3_1).Scale(((Transform) pSource).localScale);
      vector3_1 = Quaternion.op_Multiply(((Transform) pSource).rotation, vector3_1);
      pSource.pivot = zero;
      RectTransform rectTransform = pSource;
      ((Transform) rectTransform).localPosition = Vector3.op_Subtraction(((Transform) rectTransform).localPosition, vector3_1);
    }
  }

  public static Vector2 GetWorldCenter(this RectTransform pRectTransform)
  {
    RectTransform rectTransform = pRectTransform;
    Rect rect = pRectTransform.rect;
    Vector3 vector3 = Vector2.op_Implicit(((Rect) ref rect).center);
    return Vector2.op_Implicit(((Transform) rectTransform).TransformPoint(vector3));
  }

  public static Rect GetWorldRect(this RectTransform pRectTransform)
  {
    Rect rect = pRectTransform.rect;
    Rect worldRect = new Rect();
    ((Rect) ref worldRect).min = Vector2.op_Implicit(((Transform) pRectTransform).TransformPoint(Vector2.op_Implicit(((Rect) ref rect).min)));
    ((Rect) ref worldRect).max = Vector2.op_Implicit(((Transform) pRectTransform).TransformPoint(Vector2.op_Implicit(((Rect) ref rect).max)));
    return worldRect;
  }

  public static bool Overlaps(this RectTransform a, RectTransform b)
  {
    Rect rect = a.WorldRect();
    return ((Rect) ref rect).Overlaps(b.WorldRect());
  }

  public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
  {
    Rect rect = a.WorldRect();
    return ((Rect) ref rect).Overlaps(b.WorldRect(), allowInverse);
  }

  public static Rect WorldRect(this RectTransform rectTransform)
  {
    Vector2 sizeDelta = rectTransform.sizeDelta;
    float num1 = sizeDelta.x * ((Transform) rectTransform).lossyScale.x;
    float num2 = sizeDelta.y * ((Transform) rectTransform).lossyScale.y;
    RectTransform rectTransform1 = rectTransform;
    Rect rect = rectTransform.rect;
    Vector3 vector3_1 = Vector2.op_Implicit(((Rect) ref rect).center);
    Vector3 vector3_2 = ((Transform) rectTransform1).TransformPoint(vector3_1);
    return new Rect(vector3_2.x - num1 * 0.5f, vector3_2.y - num2 * 0.5f, num1, num2);
  }
}
