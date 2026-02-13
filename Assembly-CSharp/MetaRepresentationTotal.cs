// Decompiled with JetBrains decompiler
// Type: MetaRepresentationTotal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class MetaRepresentationTotal : MetaRepresentationContainerBase
{
  protected override void fillDict(
    ref int pTotal,
    ref bool pAny,
    Dictionary<IMetaObject, int> pDict)
  {
    // ISSUE: unable to decompile the method.
  }

  protected override void checkShowNone(bool pAny, int pNone, int pTotal)
  {
    if (!(this.asset.show_none_percent_for_total & pAny) || pNone <= 0)
      return;
    this.showBar(this.showStatRow("statistics_breakdown_none_list".Localize() + Toolbox.coloredGreyPart((object) pNone, ColorStyleLibrary.m.color_text_grey), (object) this.amountWithPercent(pNone, pTotal), ColorStyleLibrary.m.color_text_grey, pColorText: true, pIconPath: this.asset.general_icon_path, pLocalize: false), pNone, pTotal, ColorStyleLibrary.m.color_text_grey);
  }
}
