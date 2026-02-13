// Decompiled with JetBrains decompiler
// Type: UnitCityElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UnitCityElement : UnitElement
{
  [SerializeField]
  private GameObject _title;
  [SerializeField]
  private CityListElement _city_element;

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UnitCityElement unitCityElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (!unitCityElement.actor.hasCity())
      return false;
    unitCityElement.track_objects.Add((NanoObject) unitCityElement.actor.getCity());
    unitCityElement._title.SetActive(true);
    unitCityElement._city_element.show(unitCityElement.actor.getCity());
    ((Component) unitCityElement._city_element).gameObject.SetActive(true);
    return false;
  }

  protected override void clear()
  {
    this._title.SetActive(false);
    ((Component) this._city_element).gameObject.SetActive(false);
    base.clear();
  }

  public override bool checkRefreshWindow()
  {
    return ((Component) this._city_element).gameObject.activeSelf && !this.actor.hasCity() || base.checkRefreshWindow();
  }
}
