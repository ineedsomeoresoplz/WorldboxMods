// Decompiled with JetBrains decompiler
// Type: LayoutGroupExt.VerticalLayoutGroupExtended
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace LayoutGroupExt;

[AddComponentMenu("Layout/Vertical Layout Group ( Extended )", 151)]
public class VerticalLayoutGroupExtended : HorizontalOrVerticalLayoutGroupExtended
{
  protected VerticalLayoutGroupExtended()
  {
  }

  public override void CalculateLayoutInputHorizontal()
  {
    base.CalculateLayoutInputHorizontal();
    this.CalcAlongAxis(0, true);
  }

  public virtual void CalculateLayoutInputVertical() => this.CalcAlongAxis(1, true);

  public virtual void SetLayoutHorizontal() => this.SetChildrenAlongAxis(0, true);

  public virtual void SetLayoutVertical() => this.SetChildrenAlongAxis(1, true);
}
