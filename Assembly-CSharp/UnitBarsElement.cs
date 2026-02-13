// Decompiled with JetBrains decompiler
// Type: UnitBarsElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UnitBarsElement : UnitElement
{
  [SerializeField]
  private StatBar _hunger;
  [SerializeField]
  private StatBar _happiness;
  [SerializeField]
  private StatBar _stamina;
  [SerializeField]
  private StatBar _mana;
  [SerializeField]
  private Image _favorite_food_sprite;
  [SerializeField]
  private Image _favorite_food_bg;

  protected override IEnumerator showContent()
  {
    this.showHappiness();
    this.showHunger();
    this.showStamina();
    this.showMana();
    yield break;
  }

  private void showMana()
  {
    if (this.actor.asset.force_hide_mana)
      return;
    ((Component) this._mana).gameObject.SetActive(true);
    int maxMana = this.actor.getMaxMana();
    this._mana.setBar((float) Mathf.Clamp(this.actor.getMana(), 0, maxMana), (float) maxMana, "/" + maxMana.ToText(4));
  }

  private void showStamina()
  {
    if (this.actor.asset.force_hide_stamina)
      return;
    ((Component) this._stamina).gameObject.SetActive(true);
    int maxStamina = this.actor.getMaxStamina();
    this._stamina.setBar((float) Mathf.Clamp(this.actor.getStamina(), 0, maxStamina), (float) maxStamina, "/" + maxStamina.ToText(4));
  }

  private void showHappiness()
  {
    if (!this.actor.hasEmotions())
      return;
    ((Component) this._happiness).GetComponentInChildren<HappinessBarIcon>().load(this.actor);
    ((Component) this._happiness).gameObject.SetActive(true);
    this._happiness.setBar((float) this.actor.getHappinessPercent(), 100f, "%");
  }

  private void showHunger()
  {
    if (!this.actor.needsFood())
      return;
    ((Component) this._hunger).gameObject.SetActive(true);
    this._hunger.setBar((float) (int) ((double) this.actor.getNutrition() / (double) this.actor.getMaxNutrition() * 100.0), 100f, "%");
    if (!this.actor.hasFavoriteFood())
      return;
    ((Component) this._favorite_food_bg).gameObject.SetActive(true);
    ((Component) this._favorite_food_sprite).gameObject.SetActive(true);
    this._favorite_food_sprite.sprite = this.actor.favorite_food_asset.getSpriteIcon();
  }

  protected override void clear()
  {
    ((Component) this._mana).gameObject.SetActive(false);
    ((Component) this._stamina).gameObject.SetActive(false);
    ((Component) this._hunger).gameObject.SetActive(false);
    ((Component) this._happiness).gameObject.SetActive(false);
    ((Component) this._favorite_food_bg).gameObject.SetActive(false);
    ((Component) this._favorite_food_sprite).gameObject.SetActive(false);
    base.clear();
  }
}
