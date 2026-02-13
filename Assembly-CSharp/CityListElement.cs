// Decompiled with JetBrains decompiler
// Type: CityListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CityListElement : WindowListElementBase<City, CityData>
{
  public Text text_name;
  public CountUpOnClick population;
  public CountUpOnClick army;
  public CountUpOnClick zones;
  [SerializeField]
  private CountUpOnClick _loyalty;
  public CountUpOnClick age;
  public UiUnitAvatarElement avatarLoader;
  public CityBanner city_banner;
  [SerializeField]
  private GameObject _icon_capital;
  [SerializeField]
  private CityLoyaltyElement _loyalty_element;

  internal override void show(City pCity)
  {
    base.show(pCity);
    this.avatarLoader.show(pCity.leader);
    this._loyalty_element.setCity(pCity);
    this.text_name.text = pCity.name;
    ((Graphic) this.text_name).color = pCity.kingdom.getColor().getColorText();
    this.population.setValue(pCity.getPopulationPeople());
    this.army.setValue(pCity.countWarriors());
    this.zones.setValue(pCity.zones.Count);
    int loyalty = pCity.getLoyalty(true);
    this._loyalty.setValue(loyalty);
    if (loyalty < 0)
      ((Graphic) this._loyalty.getText()).color = Toolbox.color_negative_RGBA;
    else
      ((Graphic) this._loyalty.getText()).color = Toolbox.color_positive_RGBA;
    this.age.setValue(pCity.getAge());
    this.toggleCapital(pCity.isCapitalCity());
  }

  protected override void initMonoFields()
  {
  }

  protected override void loadBanner() => this.city_banner.load((NanoObject) this.meta_object);

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "city", new TooltipData()
    {
      city = this.meta_object
    });
  }

  private void toggleCapital(bool pState) => this._icon_capital?.SetActive(pState);

  protected override ActorAsset getActorAsset() => this.meta_object.getActorAsset();
}
