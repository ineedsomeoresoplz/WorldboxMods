// Decompiled with JetBrains decompiler
// Type: UnitFamilyOriginElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UnitFamilyOriginElement : UnitElement
{
  [SerializeField]
  private FamilyListElement _origin_element;
  [SerializeField]
  private GameObject _family_origin_title;
  private Family _ancestor_family;

  protected override IEnumerator showContent()
  {
    UnitFamilyOriginElement familyOriginElement = this;
    if (familyOriginElement.actor.data.ancestor_family.hasValue())
    {
      familyOriginElement._ancestor_family = World.world.families.get(familyOriginElement.actor.data.ancestor_family);
      if (!familyOriginElement._ancestor_family.isRekt())
      {
        familyOriginElement.track_objects.Add((NanoObject) familyOriginElement._ancestor_family);
        yield return (object) new WaitForSecondsRealtime(0.025f);
        if (familyOriginElement._ancestor_family.isAlive())
        {
          familyOriginElement._family_origin_title.SetActive(true);
          ((Component) familyOriginElement._origin_element).gameObject.SetActive(true);
          familyOriginElement._origin_element.show(familyOriginElement._ancestor_family);
        }
      }
    }
  }

  protected override void clear()
  {
    this._ancestor_family = (Family) null;
    this._family_origin_title.SetActive(false);
    ((Component) this._origin_element).gameObject.SetActive(false);
    base.clear();
  }
}
