// Decompiled with JetBrains decompiler
// Type: TooltipCultureTraitsRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class TooltipCultureTraitsRow : TooltipTraitsRow<CultureTrait>
{
  protected override IReadOnlyCollection<CultureTrait> traits_hashset
  {
    get => this.tooltip_data.culture.getTraits();
  }
}
