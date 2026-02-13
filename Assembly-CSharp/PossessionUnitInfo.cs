// Decompiled with JetBrains decompiler
// Type: PossessionUnitInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PossessionUnitInfo : MonoBehaviour
{
  [SerializeField]
  private Text _name_field;
  [SerializeField]
  private Image _icon_species;
  [SerializeField]
  private Image _icon_sex;
  [SerializeField]
  private KingdomBanner _banner_kingdom;
  [SerializeField]
  private Text _text_age;
  [SerializeField]
  private Text _text_kills;
  [SerializeField]
  private Text _text_level;
  [SerializeField]
  private StatBar _bar_health;

  private void OnEnable()
  {
    Actor controllableUnit = ControllableUnit.getControllableUnit();
    if (controllableUnit == null)
      return;
    this.showForUnit(controllableUnit);
  }

  private void Update()
  {
    Actor controllableUnit = ControllableUnit.getControllableUnit();
    if (controllableUnit == null)
      return;
    this.showForUnit(controllableUnit);
  }

  private void showForUnit(Actor pActor)
  {
    this._icon_sex.sprite = !pActor.isSexMale() ? SpriteTextureLoader.getSprite("ui/icons/IconFemale") : SpriteTextureLoader.getSprite("ui/icons/IconMale");
    this._icon_species.sprite = pActor.asset.getSpriteIcon();
    if (pActor.kingdom.isCiv())
    {
      ((Component) this._banner_kingdom).gameObject.SetActive(true);
      this._banner_kingdom.load((NanoObject) pActor.kingdom);
    }
    else
      ((Component) this._banner_kingdom).gameObject.SetActive(false);
    float health = (float) pActor.getHealth();
    float maxHealth = (float) pActor.getMaxHealth();
    this._bar_health.setBar(health, maxHealth, "/" + ((int) maxHealth).ToText(4), false, pSpeed: 0.25f);
    this._name_field.text = pActor.getName();
    ((Graphic) this._name_field).color = pActor.kingdom.getColor().getColorText();
    this._text_age.text = pActor.getAge().ToString();
    Text textKills = this._text_kills;
    int num = pActor.data.kills;
    string str1 = num.ToString();
    textKills.text = str1;
    Text textLevel = this._text_level;
    num = pActor.level;
    string str2 = num.ToString();
    textLevel.text = str2;
  }
}
