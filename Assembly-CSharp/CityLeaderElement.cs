// Decompiled with JetBrains decompiler
// Type: CityLeaderElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CityLeaderElement : CityElement
{
  [SerializeField]
  private GameObject _title_element;
  [SerializeField]
  private PrefabUnitElement _ruler_element;

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    CityLeaderElement cityLeaderElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (!cityLeaderElement.city.hasLeader())
      return false;
    cityLeaderElement.track_objects.Add((NanoObject) cityLeaderElement.city.leader);
    cityLeaderElement._title_element.gameObject.SetActive(true);
    ((Component) cityLeaderElement._ruler_element).gameObject.SetActive(true);
    cityLeaderElement._ruler_element.show(cityLeaderElement.city.leader);
    return false;
  }

  protected override void clear()
  {
    this._title_element.gameObject.SetActive(false);
    ((Component) this._ruler_element).gameObject.SetActive(false);
    base.clear();
  }

  public override bool checkRefreshWindow()
  {
    return ((Component) this._ruler_element).gameObject.activeSelf && !this.city.hasLeader() || base.checkRefreshWindow();
  }
}
