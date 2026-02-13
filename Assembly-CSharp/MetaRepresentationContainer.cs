// Decompiled with JetBrains decompiler
// Type: MetaRepresentationContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MetaRepresentationContainer : MetaRepresentationContainerBase
{
  private IMetaWindow _meta_window;

  protected override void init()
  {
    base.init();
    this._meta_window = ((Component) this).GetComponentInParent<IMetaWindow>();
  }

  protected override void fillDict(
    ref int pTotal,
    ref bool pAny,
    Dictionary<IMetaObject, int> pDict)
  {
    foreach (Actor unit in this.getMetaObject().getUnits())
    {
      ++pTotal;
      if (this.asset.check_has_meta(unit))
      {
        pAny = true;
        IMetaObject key = this.asset.meta_getter(unit);
        if (!pDict.ContainsKey(key))
          pDict.Add(key, 0);
        pDict[key]++;
      }
    }
  }

  protected override void checkShowNone(bool pAny, int pNone, int pTotal)
  {
    if (!pAny || !this.asset.show_none_percent || pNone <= 0)
      return;
    this.showBar(this.showStatRow("statistics_breakdown_none", (object) this.amountWithPercent(pNone, pTotal), ColorStyleLibrary.m.color_text_grey, pColorText: true, pIconPath: this.asset.general_icon_path), pNone, pTotal, ColorStyleLibrary.m.color_text_grey);
  }

  private IMetaObject getMetaObject() => this._meta_window.getCoreObject() as IMetaObject;
}
