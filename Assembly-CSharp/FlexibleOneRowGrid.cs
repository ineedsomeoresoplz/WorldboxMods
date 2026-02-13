// Decompiled with JetBrains decompiler
// Type: FlexibleOneRowGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using LayoutGroupExt;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

#nullable disable
[DisallowMultipleComponent]
public class FlexibleOneRowGrid : MonoBehaviour, ILayoutController
{
  public bool debug;
  public int bonus_spacing_x;
  private RectTransform _grid_rect;
  private GridLayoutGroup _grid;
  private GridLayoutGroupExtended _grid_extended;
  private bool _is_extended;
  private bool _initialized;

  private void Awake() => this.init();

  private void init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    if (((Component) this).HasComponent<GridLayoutGroup>())
    {
      this._grid = ((Component) this).GetComponent<GridLayoutGroup>();
      this._grid_rect = ((Component) this._grid).GetComponent<RectTransform>();
    }
    else
    {
      this._grid_extended = ((Component) this).GetComponent<GridLayoutGroupExtended>();
      this._grid_rect = ((Component) this._grid_extended).GetComponent<RectTransform>();
      this._is_extended = true;
    }
  }

  public void SetLayoutHorizontal()
  {
    if (!this.debug && !Application.isPlaying)
      return;
    this.init();
    float num1 = this._is_extended ? this._grid_extended.cellSize.x : this._grid.cellSize.x;
    Rect rect = this._grid_rect.rect;
    float width = ((Rect) ref rect).width;
    float children = this.calculateChildren();
    float num2;
    if ((double) num1 * (double) children + (double) this.bonus_spacing_x * ((double) children - 1.0) < (double) width)
    {
      num2 = (float) this.bonus_spacing_x;
    }
    else
    {
      float num3 = num1 * children;
      num2 = (float) (((double) width - (double) num3) / ((double) children - 1.0));
    }
    if (this._is_extended)
      this._grid_extended.spacing = new Vector2(num2, 0.0f);
    else
      this._grid.spacing = new Vector2(num2, 0.0f);
  }

  public float calculateChildren()
  {
    List<Component> componentList = CollectionPool<List<Component>, Component>.Get();
    int children = 0;
    int num = 0;
    for (int childCount = ((Transform) this._grid_rect).childCount; num < childCount; ++num)
    {
      RectTransform child = ((Transform) this._grid_rect).GetChild(num) as RectTransform;
      if (!Object.op_Equality((Object) child, (Object) null) && ((Component) child).gameObject.activeInHierarchy)
      {
        if (!((Component) child).HasComponent<ILayoutIgnorer>())
        {
          ++children;
        }
        else
        {
          ((Component) child).GetComponents(typeof (ILayoutIgnorer), componentList);
          for (int index = 0; index < componentList.Count; ++index)
          {
            if (!((ILayoutIgnorer) componentList[index]).ignoreLayout)
            {
              ++children;
              break;
            }
          }
          componentList.Clear();
        }
      }
    }
    CollectionPool<List<Component>, Component>.Release(componentList);
    return (float) children;
  }

  public void SetLayoutVertical()
  {
  }
}
