// Decompiled with JetBrains decompiler
// Type: MetaNeedsElementUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MetaNeedsElementUnit : UnitElement
{
  [SerializeField]
  private GameObject _container;
  [SerializeField]
  private Text _text;

  protected override IEnumerator showContent()
  {
    Actor unit = SelectedUnit.unit;
    if (unit != null && unit.isAlive())
    {
      string str = MetaTextReportHelper.addSingleUnitText(unit, false, false);
      this._text.text = str;
      if (!string.IsNullOrEmpty(str))
      {
        this._container.gameObject.SetActive(true);
        yield break;
      }
    }
  }

  protected override void clear()
  {
    base.clear();
    this._container.gameObject.SetActive(false);
  }
}
