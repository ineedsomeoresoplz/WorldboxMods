// Decompiled with JetBrains decompiler
// Type: WarWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WarWindow : WindowMetaGeneric<War, WarData>
{
  [SerializeField]
  private WindowMetaTab _button_interesting_persons_tab;

  public override MetaType meta_type => MetaType.War;

  protected override War meta_object => SelectedMetas.selected_war;

  internal override void showStatsRows()
  {
    War metaObject = this.meta_object;
    this.tryShowPastNames();
    this.showStatRow("war_type", (object) LocalizedTextManager.getText(metaObject.getAsset().localized_war_name), MetaType.None, -1L, "iconWar", (string) null, (TooltipDataGetter) null);
    this.showStatRow("started_at", (object) metaObject.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    int num;
    if (metaObject.hasEnded())
    {
      num = metaObject.getYearEnded();
      this.showStatRow("war_ended_at", (object) (num.ToString() ?? ""), MetaType.None, -1L, "iconClose", (string) null, (TooltipDataGetter) null);
    }
    num = metaObject.getDuration();
    this.showStatRow("war_duration", (object) (num.ToString() ?? ""), MetaType.None, -1L, "iconClock", (string) null, (TooltipDataGetter) null);
    string pValue = metaObject.data.winner.getLocaleID().Localize();
    switch (metaObject.data.winner)
    {
      case WarWinner.Attackers:
        this.showStatRow("war_winner", (object) pValue, metaObject.getAttackersColorTextString(), pColorText: true, pIconPath: "iconAttackRate");
        break;
      case WarWinner.Defenders:
        this.showStatRow("war_winner", (object) pValue, metaObject.getDefendersColorTextString(), pColorText: true, pIconPath: "iconAttackRate");
        break;
      case WarWinner.Peace:
        this.showStatRow("war_outcome", (object) pValue, MetaType.None, -1L, "actor_traits/iconPeaceful", (string) null, (TooltipDataGetter) null);
        break;
      case WarWinner.Merged:
        this.showStatRow("war_outcome", (object) pValue, MetaType.None, -1L, "iconBre", (string) null, (TooltipDataGetter) null);
        break;
    }
    this.tryToShowActor("instigator", metaObject.data.started_by_actor_id, metaObject.data.started_by_actor_name, pIconPath: "worldrules/icon_angryvillagers");
    this.tryToShowMetaKingdom("instigator_from", metaObject.data.started_by_kingdom_id, metaObject.data.started_by_kingdom_name);
    num = metaObject.countKingdoms();
    this.showStatRow("kingdoms", (object) num.ToString(), MetaType.None, -1L, "iconKingdomList", (string) null, (TooltipDataGetter) null);
    num = metaObject.countCities();
    this.showStatRow("villages", (object) num.ToString(), MetaType.None, -1L, "iconVillages", (string) null, (TooltipDataGetter) null);
    this.showStatRow("deaths", (object) (metaObject.getTotalDeaths().ToString() ?? ""), MetaType.None, -1L, "iconDead", (string) null, (TooltipDataGetter) null);
    this.showStatRow("attackers_army", (object) metaObject.countAttackersWarriors(), MetaType.None, -1L, "iconArmyAttackers", (string) null, (TooltipDataGetter) null);
    this.showStatRow("attackers_population", (object) metaObject.countAttackersPopulation(), MetaType.None, -1L, "iconPopulationAttackers", (string) null, (TooltipDataGetter) null);
    this.showStatRow("attackers_deaths", (object) metaObject.getDeadAttackers(), MetaType.None, -1L, "iconDeathAttackers", (string) null, (TooltipDataGetter) null);
    this.showStatRow("attackers_cities", (object) metaObject.countAttackersCities(), MetaType.None, -1L, "iconVillages", (string) null, (TooltipDataGetter) null);
    this.showStatRow("defenders_army", (object) metaObject.countDefendersWarriors(), MetaType.None, -1L, "iconArmyDefenders", (string) null, (TooltipDataGetter) null);
    this.showStatRow("defenders_population", (object) metaObject.countDefendersPopulation(), MetaType.None, -1L, "iconPopulationDefenders", (string) null, (TooltipDataGetter) null);
    this.showStatRow("defenders_deaths", (object) metaObject.getDeadDefenders(), MetaType.None, -1L, "iconDeathDefenders", (string) null, (TooltipDataGetter) null);
    this.showStatRow("defenders_cities", (object) metaObject.countDefendersCities(), MetaType.None, -1L, "iconVillages", (string) null, (TooltipDataGetter) null);
    AchievementLibrary.ancient_war_of_geometry_and_evil.checkBySignal();
  }

  public override void startShowingWindow()
  {
    base.startShowingWindow();
    if (!this.meta_object.hasEnded())
      this._button_interesting_persons_tab.toggleActive(true);
    else
      this._button_interesting_persons_tab.toggleActive(false);
    if ((!Object.op_Equality((Object) this.tabs.getActiveTab(), (Object) this._button_interesting_persons_tab) ? 0 : (this.meta_object.hasEnded() ? 1 : 0)) == 0)
      return;
    this.showTab(this.tabs.tab_default);
  }
}
