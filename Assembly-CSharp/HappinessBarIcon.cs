// Decompiled with JetBrains decompiler
// Type: HappinessBarIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HappinessBarIcon : MonoBehaviour
{
  [SerializeField]
  private Image _icon;
  private Actor _actor;

  private void Awake()
  {
    ((Component) this).GetComponentInParent<StatBar>().addCallback(new StatBarUpdated(this.barUpdated));
  }

  public void load(Actor pActor) => this._actor = pActor;

  private void barUpdated(float pValue, float pMax)
  {
    if (this._actor.isRekt())
      return;
    this._icon.sprite = HappinessHelper.getSpriteBasedOnHappinessValue(this._actor.getHappiness());
  }

  private void OnDisable() => this._actor = (Actor) null;
}
