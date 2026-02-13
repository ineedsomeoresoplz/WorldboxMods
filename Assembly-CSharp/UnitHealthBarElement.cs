// Decompiled with JetBrains decompiler
// Type: UnitHealthBarElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UnitHealthBarElement : UnitElement
{
  [SerializeField]
  private StatBar _health;

  protected override IEnumerator showContent()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UnitHealthBarElement healthBarElement = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    healthBarElement._health.setBar((float) healthBarElement.actor.getHealth(), (float) healthBarElement.actor.getMaxHealth(), "/" + healthBarElement.actor.getMaxHealth().ToText(4));
    return false;
  }
}
