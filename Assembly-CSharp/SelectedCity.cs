// Decompiled with JetBrains decompiler
// Type: SelectedCity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SelectedCity : SelectedMetaWithUnit<City, CityData>
{
  [SerializeField]
  private CityLoyaltyElement _loyalty_element;
  [SerializeField]
  private CitySelectedResources _resources;
  [SerializeField]
  private CitySelectedResources _food;
  private int _last_storage_version;

  protected override MetaType meta_type => MetaType.City;

  public override string unit_title_locale_key => "titled_leader";

  public override bool hasUnit() => this.nano_object.hasLeader();

  public override Actor getUnit() => this.nano_object.leader;

  protected override string getPowerTabAssetID() => "selected_city";

  protected override void showStatsGeneral(City pCity)
  {
    base.showStatsGeneral(pCity);
    int populationPeople = pCity.getPopulationPeople();
    pCity.countFoodTotal();
    if (populationPeople > pCity.getPopulationMaximum())
      this.setIconValue("i_population", (float) populationPeople, new float?((float) pCity.getPopulationMaximum()), "#FB2C21");
    else
      this.setIconValue("i_population", (float) populationPeople, new float?((float) pCity.getPopulationMaximum()));
    this.setIconValue("i_territory", (float) pCity.countZones());
    this.setIconValue("i_boats", (float) pCity.countBoats());
    this.setIconValue("i_books", (float) pCity.countBooks());
    int loyalty = pCity.getLoyalty(true);
    if (loyalty > 0)
      this.setIconValue("i_loyalty", (float) loyalty, pColor: "#43FF43");
    else
      this.setIconValue("i_loyalty", (float) loyalty, pColor: "#FB2C21");
    this._loyalty_element.setCity(pCity);
    if (WorldLawLibrary.world_law_civ_army.isEnabled())
      this.setIconValue("i_army", (float) pCity.countWarriors(), new float?((float) pCity.getMaxWarriors()));
    else
      this.setIconValue("i_army", (float) pCity.countWarriors());
    this.setIconValue("i_houses", (float) pCity.getHouseCurrent(), new float?((float) pCity.getHouseLimit()));
  }

  protected override void updateElements(City pNano)
  {
    if (pNano.isRekt())
      return;
    base.updateElements(pNano);
    this._last_storage_version = pNano.getStorageVersion();
  }

  protected override void updateElementsAlways(City pNano)
  {
    base.updateElementsAlways(pNano);
    if (!this.storageChanged(pNano))
      return;
    this._resources.update(pNano);
    this._food.update(pNano);
  }

  protected override void checkAchievements(City pCity)
  {
    AchievementLibrary.checkCityAchievements(pCity);
  }

  public void openInventoryTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Inventory");
  }

  public void openBooksTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Books");
  }

  public void openFamilyTab()
  {
    ScrollWindow.showWindow(this.window_id);
    ScrollWindow.getCurrentWindow().tabs.showTab("Family");
  }

  private bool storageChanged(City pCity)
  {
    return pCity.getStorageVersion() != this._last_storage_version || this.isNanoChanged(pCity);
  }
}
