// Decompiled with JetBrains decompiler
// Type: LayoutGroupExt.HorizontalLayoutGroupExtended
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace LayoutGroupExt;

[AddComponentMenu("Layout/Horizontal Layout Group ( Extended )", 150)]
public class HorizontalLayoutGroupExtended : HorizontalOrVerticalLayoutGroupExtended
{
  protected HorizontalLayoutGroupExtended()
  {
  }

  public override void CalculateLayoutInputHorizontal()
  {
    base.CalculateLayoutInputHorizontal();
    this.CalcAlongAxis(0, false);
  }

  public virtual void CalculateLayoutInputVertical() => this.CalcAlongAxis(1, false);

  public virtual void SetLayoutHorizontal() => this.SetChildrenAlongAxis(0, false);

  public virtual void SetLayoutVertical() => this.SetChildrenAlongAxis(1, false);
}
