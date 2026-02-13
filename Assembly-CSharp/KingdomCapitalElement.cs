// Decompiled with JetBrains decompiler
// Type: KingdomCapitalElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class KingdomCapitalElement : KingdomElement
{
  [SerializeField]
  private CityListElement _capital_element;

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    KingdomCapitalElement kingdomCapitalElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (!kingdomCapitalElement.kingdom.hasCapital())
      return false;
    kingdomCapitalElement.track_objects.Add((NanoObject) kingdomCapitalElement.kingdom.capital);
    ((Component) kingdomCapitalElement._capital_element).gameObject.SetActive(true);
    kingdomCapitalElement._capital_element.show(kingdomCapitalElement.kingdom.capital);
    return false;
  }

  protected override void clear()
  {
    ((Component) this._capital_element).gameObject.SetActive(false);
    base.clear();
  }

  public override bool checkRefreshWindow()
  {
    return ((Component) this._capital_element).gameObject.activeSelf && !this.kingdom.hasCapital() || base.checkRefreshWindow();
  }
}
