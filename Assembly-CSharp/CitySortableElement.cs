// Decompiled with JetBrains decompiler
// Type: CitySortableElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CitySortableElement : CityElement, ILayoutController
{
  private RectTransform _rect;
  private List<RectTransform> _rect_children = new List<RectTransform>();

  protected override void Awake()
  {
    this._rect = ((Component) this).GetComponent<RectTransform>();
    base.Awake();
  }

  protected virtual void onListChange()
  {
  }

  public void SetLayoutVertical()
  {
    if (Object.op_Equality((Object) this._rect, (Object) null))
      return;
    using (ListPool<RectTransform> layoutChildren = this._rect.getLayoutChildren())
    {
      if (layoutChildren.SequenceEqual<RectTransform>((IEnumerable<RectTransform>) this._rect_children))
        return;
      this._rect_children.Clear();
      this._rect_children.AddRange((IEnumerable<RectTransform>) layoutChildren);
      this.onListChange();
    }
  }

  public void SetLayoutHorizontal()
  {
  }
}
