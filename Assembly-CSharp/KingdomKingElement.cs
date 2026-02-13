// Decompiled with JetBrains decompiler
// Type: KingdomKingElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class KingdomKingElement : KingdomElement
{
  [SerializeField]
  private GameObject _title_element;
  [SerializeField]
  private PrefabUnitElement _king_element;

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    KingdomKingElement kingdomKingElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (!kingdomKingElement.kingdom.hasKing())
      return false;
    kingdomKingElement.track_objects.Add((NanoObject) kingdomKingElement.kingdom.king);
    kingdomKingElement._title_element.SetActive(true);
    ((Component) kingdomKingElement._king_element).gameObject.SetActive(true);
    kingdomKingElement._king_element.show(kingdomKingElement.kingdom.king);
    return false;
  }

  protected override void clear()
  {
    this._title_element.SetActive(false);
    ((Component) this._king_element).gameObject.SetActive(false);
    base.clear();
  }

  public override bool checkRefreshWindow()
  {
    return ((Component) this._king_element).gameObject.activeSelf && !this.kingdom.hasKing() || base.checkRefreshWindow();
  }
}
