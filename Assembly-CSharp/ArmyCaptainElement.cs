// Decompiled with JetBrains decompiler
// Type: ArmyCaptainElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ArmyCaptainElement : ArmyElement
{
  [SerializeField]
  private GameObject _title_element;
  [SerializeField]
  private PrefabUnitElement _captain_element;

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ArmyCaptainElement armyCaptainElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (!armyCaptainElement.army.hasCaptain())
      return false;
    armyCaptainElement.track_objects.Add((NanoObject) armyCaptainElement.army.getCaptain());
    armyCaptainElement._title_element.gameObject.SetActive(true);
    ((Component) armyCaptainElement._captain_element).gameObject.SetActive(true);
    armyCaptainElement._captain_element.show(armyCaptainElement.army.getCaptain());
    return false;
  }

  protected override void clear()
  {
    this._title_element.gameObject.SetActive(false);
    ((Component) this._captain_element).gameObject.SetActive(false);
    base.clear();
  }

  public override bool checkRefreshWindow()
  {
    return ((Component) this._captain_element).gameObject.activeSelf && !this.army.hasCaptain() || base.checkRefreshWindow();
  }
}
