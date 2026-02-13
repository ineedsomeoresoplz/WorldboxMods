// Decompiled with JetBrains decompiler
// Type: PlotCategoryLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class PlotCategoryLibrary : BaseCategoryLibrary<PlotCategoryAsset>
{
  public override void init()
  {
    base.init();
    PlotCategoryAsset pAsset1 = new PlotCategoryAsset();
    pAsset1.id = "diplomacy";
    pAsset1.name = "plot_group_diplomacy";
    pAsset1.color = "#5EFFFF";
    pAsset1.show_counter = false;
    pAsset1.plot_retry_action = new PlotRetryAction(PlotCategoryLibrary.diplomacyRetryAction);
    this.add(pAsset1);
    PlotCategoryAsset pAsset2 = new PlotCategoryAsset();
    pAsset2.id = "rites_wrathful";
    pAsset2.name = "plot_group_rites_wrathful";
    pAsset2.color = "#FF6145";
    pAsset2.show_counter = false;
    pAsset2.plot_retry_action = new PlotRetryAction(PlotCategoryLibrary.ritesRetryAction);
    this.add(pAsset2);
    PlotCategoryAsset pAsset3 = new PlotCategoryAsset();
    pAsset3.id = "rites_summoning";
    pAsset3.name = "plot_group_rites_summoning";
    pAsset3.color = "#BC42FF";
    pAsset3.show_counter = false;
    pAsset3.plot_retry_action = new PlotRetryAction(PlotCategoryLibrary.ritesRetryAction);
    this.add(pAsset3);
    PlotCategoryAsset pAsset4 = new PlotCategoryAsset();
    pAsset4.id = "rites_merciful";
    pAsset4.name = "plot_group_rites_merciful";
    pAsset4.color = "#89FF56";
    pAsset4.show_counter = false;
    pAsset4.plot_retry_action = new PlotRetryAction(PlotCategoryLibrary.ritesRetryAction);
    this.add(pAsset4);
    PlotCategoryAsset pAsset5 = new PlotCategoryAsset();
    pAsset5.id = "culture";
    pAsset5.name = "plot_group_culture";
    pAsset5.color = "#FFDA23";
    pAsset5.show_counter = false;
    pAsset5.plot_retry_action = new PlotRetryAction(PlotCategoryLibrary.culturePlotsRetryAction);
    this.add(pAsset5);
    PlotCategoryAsset pAsset6 = new PlotCategoryAsset();
    pAsset6.id = "language";
    pAsset6.name = "plot_group_language";
    pAsset6.color = "#A3AFFF";
    pAsset6.show_counter = false;
    pAsset6.plot_retry_action = new PlotRetryAction(PlotCategoryLibrary.culturePlotsRetryAction);
    this.add(pAsset6);
    PlotCategoryAsset pAsset7 = new PlotCategoryAsset();
    pAsset7.id = "religion";
    pAsset7.name = "plot_group_religion";
    pAsset7.color = "#BAF0F4";
    pAsset7.show_counter = false;
    pAsset7.plot_retry_action = new PlotRetryAction(PlotCategoryLibrary.culturePlotsRetryAction);
    this.add(pAsset7);
    PlotCategoryAsset pAsset8 = new PlotCategoryAsset();
    pAsset8.id = "rites_various";
    pAsset8.name = "plot_group_rites_various";
    pAsset8.color = "#d86569";
    pAsset8.show_counter = false;
    pAsset8.plot_retry_action = new PlotRetryAction(PlotCategoryLibrary.ritesRetryAction);
    this.add(pAsset8);
    PlotCategoryAsset pAsset9 = new PlotCategoryAsset();
    pAsset9.id = "plots_others";
    pAsset9.name = "plot_group_others";
    pAsset9.color = "#D8D8D8";
    pAsset9.show_counter = false;
    pAsset9.plot_retry_action = new PlotRetryAction(PlotCategoryLibrary.ritesRetryAction);
    this.add(pAsset9);
  }

  public static bool diplomacyRetryAction()
  {
    return World.world.cities.isLocked() || World.world.kingdoms.isLocked() || World.world.alliances.isLocked() || World.world.wars.isLocked() || World.world.clans.isLocked();
  }

  public static bool ritesRetryAction()
  {
    return World.world.cities.isLocked() || World.world.kingdoms.isLocked() || World.world.clans.isLocked() || World.world.religions.isLocked() || World.world.subspecies.isLocked();
  }

  public static bool culturePlotsRetryAction()
  {
    return World.world.cultures.isLocked() || World.world.religions.isLocked() || World.world.languages.isLocked();
  }
}
