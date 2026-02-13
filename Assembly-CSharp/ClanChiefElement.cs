// Decompiled with JetBrains decompiler
// Type: ClanChiefElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ClanChiefElement : ClanElement
{
  [SerializeField]
  private GameObject _title_element;
  [SerializeField]
  private PrefabUnitElement _chief_element;

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ClanChiefElement clanChiefElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (!clanChiefElement.clan.hasChief())
      return false;
    clanChiefElement.track_objects.Add((NanoObject) clanChiefElement.clan.getChief());
    clanChiefElement._title_element.SetActive(true);
    ((Component) clanChiefElement._chief_element).gameObject.SetActive(true);
    clanChiefElement._chief_element.show(clanChiefElement.clan.getChief());
    return false;
  }

  protected override void clear()
  {
    this._title_element.SetActive(false);
    ((Component) this._chief_element).gameObject.SetActive(false);
    base.clear();
  }

  public override bool checkRefreshWindow()
  {
    return ((Component) this._chief_element).gameObject.activeSelf && !this.clan.hasChief() || base.checkRefreshWindow();
  }
}
