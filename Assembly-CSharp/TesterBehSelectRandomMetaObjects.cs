// Decompiled with JetBrains decompiler
// Type: TesterBehSelectRandomMetaObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class TesterBehSelectRandomMetaObjects : BehaviourActionTester
{
  private bool _pick_selected_objects;
  private string[] _trait_editors;

  public TesterBehSelectRandomMetaObjects(bool pPickSelectedObjects = false)
  {
    this._pick_selected_objects = pPickSelectedObjects;
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    if (this._trait_editors == null)
      this._trait_editors = new string[3]
      {
        PowerLibrary.traits_delta_rain_edit.id,
        PowerLibrary.traits_gamma_rain_edit.id,
        PowerLibrary.traits_omega_rain_edit.id
      };
    Config.selected_trait_editor = this._trait_editors.GetRandom<string>();
    SelectedMetas.selected_alliance = Randy.getRandom<Alliance>(BehaviourActionBase<AutoTesterBot>.world.alliances.list);
    SelectedMetas.selected_army = Randy.getRandom<Army>(BehaviourActionBase<AutoTesterBot>.world.armies.list);
    SelectedMetas.selected_city = Randy.getRandom<City>(BehaviourActionBase<AutoTesterBot>.world.cities.list);
    SelectedMetas.selected_clan = Randy.getRandom<Clan>(BehaviourActionBase<AutoTesterBot>.world.clans.list);
    SelectedMetas.selected_culture = Randy.getRandom<Culture>(BehaviourActionBase<AutoTesterBot>.world.cultures.list);
    SelectedMetas.selected_family = Randy.getRandom<Family>(BehaviourActionBase<AutoTesterBot>.world.families.list);
    SelectedMetas.selected_item = Randy.getRandom<Item>(BehaviourActionBase<AutoTesterBot>.world.items.list);
    SelectedMetas.selected_kingdom = Randy.getRandom<Kingdom>(BehaviourActionBase<AutoTesterBot>.world.kingdoms.list);
    SelectedMetas.selected_language = Randy.getRandom<Language>(BehaviourActionBase<AutoTesterBot>.world.languages.list);
    SelectedMetas.selected_plot = Randy.getRandom<Plot>(BehaviourActionBase<AutoTesterBot>.world.plots.list);
    SelectedMetas.selected_religion = Randy.getRandom<Religion>(BehaviourActionBase<AutoTesterBot>.world.religions.list);
    SelectedMetas.selected_subspecies = Randy.getRandom<Subspecies>(BehaviourActionBase<AutoTesterBot>.world.subspecies.list);
    SelectedMetas.selected_war = Randy.getRandom<War>(BehaviourActionBase<AutoTesterBot>.world.wars.list);
    int num = 10;
    while (num-- > 0)
    {
      Actor random = BehaviourActionBase<AutoTesterBot>.world.units.GetRandom();
      if (!random.isRekt() && random.asset.can_be_inspected)
      {
        SelectedUnit.clear();
        SelectedUnit.select(random);
        SelectedObjects.setNanoObject((NanoObject) random);
        PowerTabController.showTabSelectedUnit();
        break;
      }
    }
    if (SelectedMetas.selected_item != null)
      SelectedMetas.selected_item.data.favorite = Randy.randomBool();
    if (SelectedUnit.isSet())
      SelectedUnit.unit.data.favorite = Randy.randomBool();
    Config.selected_objects_graph.Clear();
    if (this._pick_selected_objects)
    {
      List<NanoObject> list = new List<NanoObject>();
      if (SelectedMetas.selected_alliance != null)
        list.Add((NanoObject) SelectedMetas.selected_alliance);
      if (SelectedMetas.selected_city != null)
        list.Add((NanoObject) SelectedMetas.selected_city);
      if (SelectedMetas.selected_clan != null)
        list.Add((NanoObject) SelectedMetas.selected_clan);
      if (SelectedMetas.selected_culture != null)
        list.Add((NanoObject) SelectedMetas.selected_culture);
      if (SelectedMetas.selected_family != null)
        list.Add((NanoObject) SelectedMetas.selected_family);
      if (SelectedMetas.selected_kingdom != null)
        list.Add((NanoObject) SelectedMetas.selected_kingdom);
      if (SelectedMetas.selected_language != null)
        list.Add((NanoObject) SelectedMetas.selected_language);
      if (SelectedMetas.selected_religion != null)
        list.Add((NanoObject) SelectedMetas.selected_religion);
      if (SelectedMetas.selected_subspecies != null)
        list.Add((NanoObject) SelectedMetas.selected_subspecies);
      list.Shuffle<NanoObject>();
      for (int index = 0; index < 3 && index < list.Count; ++index)
      {
        if (list[index] != null)
          Config.selected_objects_graph.Add(list[index]);
      }
    }
    return BehResult.Continue;
  }
}
