// Decompiled with JetBrains decompiler
// Type: SubspeciesWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SubspeciesWindow : 
  WindowMetaGeneric<Subspecies, SubspeciesData>,
  ITraitWindow<SubspeciesTrait, SubspeciesTraitButton>,
  IAugmentationsWindow<ITraitsEditor<SubspeciesTrait>>
{
  public StatBar experienceBar;

  public override MetaType meta_type => MetaType.Subspecies;

  protected override Subspecies meta_object => SelectedMetas.selected_subspecies;

  public override void startShowingWindow()
  {
    base.startShowingWindow();
    ActorAsset actorAsset = this.meta_object.getActorAsset();
    if (!actorAsset.isAvailable())
      actorAsset.unlock(true);
    AchievementLibrary.checkSubspeciesAchievements(this.meta_object);
  }

  protected override bool onNameChange(string pInput)
  {
    if (!base.onNameChange(pInput))
      return false;
    foreach (Religion religion in (CoreSystemManager<Religion, ReligionData>) World.world.religions)
    {
      if (!religion.isRekt() && religion.data.creator_subspecies_id == this.meta_object.getID())
        religion.data.creator_subspecies_name = this.meta_object.data.name;
    }
    foreach (Culture culture in (CoreSystemManager<Culture, CultureData>) World.world.cultures)
    {
      if (!culture.isRekt() && culture.data.creator_subspecies_id == this.meta_object.getID())
        culture.data.creator_subspecies_name = this.meta_object.data.name;
    }
    foreach (Clan clan in (CoreSystemManager<Clan, ClanData>) World.world.clans)
    {
      if (!clan.isRekt() && clan.data.creator_subspecies_id == this.meta_object.getID())
        clan.data.creator_subspecies_name = this.meta_object.data.name;
    }
    foreach (Language language in (CoreSystemManager<Language, LanguageData>) World.world.languages)
    {
      if (!language.isRekt() && language.data.creator_subspecies_id == this.meta_object.getID())
        language.data.creator_subspecies_name = this.meta_object.data.name;
    }
    foreach (Family family in (CoreSystemManager<Family, FamilyData>) World.world.families)
    {
      if (!family.isRekt() && family.data.subspecies_id == this.meta_object.getID())
        family.data.subspecies_name = this.meta_object.data.name;
    }
    return true;
  }

  internal override void showStatsRows()
  {
    this.tryShowPastNames();
    this.showStatRow("created", (object) this.meta_object.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    this.showStatRow("generation", (object) this.meta_object.getGeneration(), MetaType.None, -1L, "worldrules/icon_grow_trees_fast", (string) null, (TooltipDataGetter) null);
    this.showStatRow("world_population_percentage", (object) (this.meta_object.countPopulationPercentage().ToString() + "%"), MetaType.None, -1L, "iconPopulation", (string) null, (TooltipDataGetter) null);
    if (this.meta_object.hasParentSubspecies())
    {
      Subspecies pObject = World.world.subspecies.get(this.meta_object.data.parent_subspecies);
      if (pObject == null)
        this.showStatRow("subspecies_ancestor", (object) LocalizedTextManager.getText("subspecies_extinct"), ColorStyleLibrary.m.color_dead_text, pColorText: true);
      else
        this.tryToShowMetaSubspecies("subspecies_ancestor", pObject: pObject);
    }
    Subspecies pObject1 = World.world.subspecies.get(this.meta_object.data.evolved_into_subspecies);
    if (pObject1 == null)
      return;
    this.tryToShowMetaSubspecies("evolution", pObject: pObject1);
  }

  public void debugClearExpLevel()
  {
    this.meta_object.debugClear();
    this.OnEnable();
  }

  T IAugmentationsWindow<ITraitsEditor<SubspeciesTrait>>.GetComponentInChildren<T>(
    bool includeInactive)
  {
    return ((Component) this).GetComponentInChildren<T>(includeInactive);
  }
}
