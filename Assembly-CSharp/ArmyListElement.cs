// Decompiled with JetBrains decompiler
// Type: ArmyListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ArmyListElement : WindowListElementBase<Army, ArmyData>
{
  [SerializeField]
  private Text _text_name;
  [SerializeField]
  private CountUpOnClick _amount;
  [SerializeField]
  private CountUpOnClick _age;
  [SerializeField]
  private CountUpOnClick _renown;
  [SerializeField]
  private CountUpOnClick _kills;
  [SerializeField]
  private CountUpOnClick _deaths;
  [SerializeField]
  private UiUnitAvatarElement _captain;
  [SerializeField]
  private ArmyBanner _army_banner;

  internal override void show(Army pArmy)
  {
    base.show(pArmy);
    this._text_name.text = pArmy.name;
    ((Graphic) this._text_name).color = pArmy.getColor().getColorText();
    bool flag = pArmy.hasCaptain();
    ((Component) this._captain).gameObject.SetActive(flag);
    if (flag)
      this._captain.show(pArmy.getCaptain());
    this._amount.setValue(pArmy.countUnits());
    this._age.setValue(pArmy.getAge());
    this._renown.setValue(pArmy.getRenown());
    this._kills.setValue((int) pArmy.getTotalKills());
    this._deaths.setValue((int) pArmy.getTotalDeaths());
  }

  protected override void initMonoFields()
  {
  }

  protected override void loadBanner() => this._army_banner.load((NanoObject) this.meta_object);

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "army", new TooltipData()
    {
      army = this.meta_object
    });
  }

  protected override ActorAsset getActorAsset() => this.meta_object.getActorAsset();
}
