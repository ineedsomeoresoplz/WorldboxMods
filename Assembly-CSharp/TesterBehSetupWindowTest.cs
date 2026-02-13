// Decompiled with JetBrains decompiler
// Type: TesterBehSetupWindowTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System;
using System.Collections.Generic;

#nullable disable
public class TesterBehSetupWindowTest : BehaviourActionTester
{
  private int currentWindow;
  private City city;
  private Clan clan;
  private Plot plot;
  private Alliance alliance;
  private War war;
  private Kingdom kingdom;
  private Culture culture;
  private Actor unit;

  public override BehResult execute(AutoTesterBot pObject)
  {
    List<WindowAsset> testableWindows = AssetManager.window_library.getTestableWindows();
    if (this.currentWindow >= testableWindows.Count)
      this.currentWindow = 0;
    string id = testableWindows[this.currentWindow++].id;
    this.pickTheBest();
    SelectedMetas.selected_city = this.city;
    SelectedMetas.selected_clan = this.clan;
    SelectedMetas.selected_plot = this.plot;
    SelectedMetas.selected_alliance = this.alliance;
    SelectedMetas.selected_war = this.war;
    SelectedMetas.selected_kingdom = this.kingdom;
    SelectedMetas.selected_culture = this.culture;
    if (!this.unit.isRekt())
      SelectedUnit.select(this.unit);
    else
      SelectedUnit.clear();
    Config.current_brush = Brush.getRandom();
    Config.power_to_unlock = GodPower.premium_powers.Find((Predicate<GodPower>) (tPower => tPower.id == "cybercore"));
    Config.selected_trait_editor = PowerLibrary.traits_delta_rain_edit.id;
    SaveManager.setCurrentSlot(1);
    if (id.Contains("workshop"))
      SaveManager.currentWorkshopMapData = WorkshopMapData.currentMapToWorkshop();
    return BehResult.Continue;
  }

  private void pickTheBest()
  {
    List<City> pList1 = new List<City>();
    pList1.AddRange((IEnumerable<City>) BehaviourActionBase<AutoTesterBot>.world.cities);
    pList1.Sort(new Comparison<City>(ComponentListBase<CityListElement, City, CityData, CityListComponent>.sortByPopulation));
    this.city = Randy.getRandom<City>(pList1);
    List<Clan> pList2 = new List<Clan>();
    pList2.AddRange((IEnumerable<Clan>) BehaviourActionBase<AutoTesterBot>.world.clans);
    pList2.Sort(new Comparison<Clan>(ComponentListBase<ClanListElement, Clan, ClanData, ClanListComponent>.sortByPopulation));
    this.clan = Randy.getRandom<Clan>(pList2);
    List<Actor> pList3 = new List<Actor>();
    pList3.AddRange((IEnumerable<Actor>) BehaviourActionBase<AutoTesterBot>.world.units);
    pList3.Sort(new Comparison<Actor>(TesterBehSetupWindowTest.sortByActorMaturity));
    this.unit = Randy.getRandom<Actor>(pList3);
    List<Kingdom> pList4 = new List<Kingdom>();
    pList4.AddRange((IEnumerable<Kingdom>) BehaviourActionBase<AutoTesterBot>.world.kingdoms);
    pList4.Sort(new Comparison<Kingdom>(KingdomListComponent.sortByArmy));
    this.kingdom = Randy.getRandom<Kingdom>(pList4);
    List<Alliance> pList5 = new List<Alliance>();
    pList5.AddRange((IEnumerable<Alliance>) BehaviourActionBase<AutoTesterBot>.world.alliances);
    pList5.Sort(new Comparison<Alliance>(ComponentListBase<AllianceListElement, Alliance, AllianceData, AllianceListComponent>.sortByPopulation));
    this.alliance = Randy.getRandom<Alliance>(pList5);
    List<Culture> pList6 = new List<Culture>();
    pList6.AddRange((IEnumerable<Culture>) BehaviourActionBase<AutoTesterBot>.world.cultures);
    pList6.Sort(new Comparison<Culture>(ComponentListBase<CultureListElement, Culture, CultureData, CultureListComponent>.sortByPopulation));
    this.culture = Randy.getRandom<Culture>(pList6);
    List<Plot> pList7 = new List<Plot>();
    pList7.AddRange((IEnumerable<Plot>) BehaviourActionBase<AutoTesterBot>.world.plots);
    pList7.Sort(new Comparison<Plot>(PlotListComponent.sortBySupporters));
    this.plot = Randy.getRandom<Plot>(pList7);
    List<War> pList8 = new List<War>();
    pList8.AddRange((IEnumerable<War>) BehaviourActionBase<AutoTesterBot>.world.wars);
    pList8.Sort(new Comparison<War>(WarListComponent.sortByAge));
    this.war = Randy.getRandom<War>(pList8);
  }

  public static int sortByActorMaturity(Actor pActor1, Actor pActor2)
  {
    if (pActor2.hasClan() && !pActor1.hasClan())
      return 1;
    if (pActor1.hasClan() && !pActor2.hasClan())
      return -1;
    if (pActor2.hasCulture() && !pActor1.hasCulture())
      return 1;
    if (pActor1.hasCulture() && !pActor2.hasCulture())
      return -1;
    if (pActor2.isKing() && !pActor1.isKing())
      return 1;
    if (pActor1.isKing() && !pActor2.isKing())
      return -1;
    int num1 = pActor2.countTraits().CompareTo(pActor1.countTraits());
    if (num1 != 0)
      return num1;
    int num2 = pActor2.data.level.CompareTo(pActor1.data.level);
    return num2 != 0 ? num2 : pActor2.getAge().CompareTo(pActor1.getAge());
  }
}
