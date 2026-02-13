// Decompiled with JetBrains decompiler
// Type: MetaTextReportLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class MetaTextReportLibrary : AssetLibrary<MetaTextReportAsset>
{
  public override void init()
  {
    base.init();
    this.addGeneralMeta();
    this.addCity();
  }

  private void addCity()
  {
    MetaTextReportAsset pAsset1 = new MetaTextReportAsset();
    pAsset1.id = "happy";
    pAsset1.color = "#ADADAD";
    pAsset1.report_action = (MetaTextReportAction) (pObject => (double) pObject.getRatioHappy() > 0.800000011920929);
    this.add(pAsset1);
    MetaTextReportAsset pAsset2 = new MetaTextReportAsset();
    pAsset2.id = "unhappy";
    pAsset2.color = "#919191";
    pAsset2.report_action = (MetaTextReportAction) (pObject => (double) pObject.getRatioUnhappy() > 0.800000011920929);
    this.add(pAsset2);
    MetaTextReportAsset pAsset3 = new MetaTextReportAsset();
    pAsset3.id = "many_children";
    pAsset3.color = "#ADADAD";
    pAsset3.report_action = (MetaTextReportAction) (pObject => pObject.countUnits() >= 20 && (double) pObject.getRatioChildren() > 0.699999988079071);
    this.add(pAsset3);
    MetaTextReportAsset pAsset4 = new MetaTextReportAsset();
    pAsset4.id = "many_homeless";
    pAsset4.color = "#919191";
    pAsset4.report_action = (MetaTextReportAction) (pObject => pObject.countUnits() >= 20 && (double) pObject.getRatioHomeless() > 0.800000011920929);
    this.add(pAsset4);
    MetaTextReportAsset pAsset5 = new MetaTextReportAsset();
    pAsset5.id = "food_plenty";
    pAsset5.color = "#ADADAD";
    pAsset5.report_action = (MetaTextReportAction) (pObject =>
    {
      City city = pObject as City;
      return city.countFoodTotal() > city.getPopulationPeople() * 4;
    });
    this.add(pAsset5);
    MetaTextReportAsset pAsset6 = new MetaTextReportAsset();
    pAsset6.id = "food_running_out";
    pAsset6.color = "#919191";
    pAsset6.report_action = (MetaTextReportAction) (pObject =>
    {
      City city = pObject as City;
      int num = city.countFoodTotal();
      if (num == 0)
        return false;
      int populationPeople = city.getPopulationPeople();
      return num < populationPeople * 2;
    });
    this.add(pAsset6);
    MetaTextReportAsset pAsset7 = new MetaTextReportAsset();
    pAsset7.id = "food_none";
    pAsset7.color = "#919191";
    pAsset7.report_action = (MetaTextReportAction) (pObject => (pObject as City).countFoodTotal() == 0);
    this.add(pAsset7);
    MetaTextReportAsset pAsset8 = new MetaTextReportAsset();
    pAsset8.id = "stone_none";
    pAsset8.color = "#919191";
    pAsset8.report_action = (MetaTextReportAction) (pObject => (pObject as City).amount_stone == 0);
    this.add(pAsset8);
    MetaTextReportAsset pAsset9 = new MetaTextReportAsset();
    pAsset9.id = "wood_none";
    pAsset9.color = "#919191";
    pAsset9.report_action = (MetaTextReportAction) (pObject => (pObject as City).amount_wood == 0);
    this.add(pAsset9);
    MetaTextReportAsset pAsset10 = new MetaTextReportAsset();
    pAsset10.id = "metal_none";
    pAsset10.color = "#919191";
    pAsset10.report_action = (MetaTextReportAction) (pObject => (pObject as City).amount_common_metals == 0);
    this.add(pAsset10);
    MetaTextReportAsset pAsset11 = new MetaTextReportAsset();
    pAsset11.id = "gold_none";
    pAsset11.color = "#919191";
    pAsset11.report_action = (MetaTextReportAction) (pObject => (pObject as City).amount_gold == 0);
    this.add(pAsset11);
    MetaTextReportAsset pAsset12 = new MetaTextReportAsset();
    pAsset12.id = "war_high_casualties";
    pAsset12.color = "#919191";
    pAsset12.report_action = (MetaTextReportAction) (pObject => (pObject as War).getTotalDeaths() > 100L);
    this.add(pAsset12);
    MetaTextReportAsset pAsset13 = new MetaTextReportAsset();
    pAsset13.id = "war_long";
    pAsset13.color = "#919191";
    pAsset13.report_action = (MetaTextReportAction) (pObject => pObject.getAge() > 100);
    this.add(pAsset13);
    MetaTextReportAsset pAsset14 = new MetaTextReportAsset();
    pAsset14.id = "war_fresh";
    pAsset14.color = "#ADADAD";
    pAsset14.report_action = (MetaTextReportAction) (pObject => pObject.getAge() < 5);
    this.add(pAsset14);
    MetaTextReportAsset pAsset15 = new MetaTextReportAsset();
    pAsset15.id = "war_defenders_getting_captured";
    pAsset15.color = "#ADADAD";
    pAsset15.report_action = (MetaTextReportAction) (pObject =>
    {
      War pWar = pObject as War;
      return pWar.areDefendersGettingCaptured() && !pWar.areAttackersGettingCaptured();
    });
    this.add(pAsset15);
    MetaTextReportAsset pAsset16 = new MetaTextReportAsset();
    pAsset16.id = "war_attackers_getting_captured";
    pAsset16.color = "#ADADAD";
    pAsset16.report_action = (MetaTextReportAction) (pObject =>
    {
      War pWar = pObject as War;
      return pWar.areAttackersGettingCaptured() && !pWar.areDefendersGettingCaptured();
    });
    this.add(pAsset16);
    MetaTextReportAsset pAsset17 = new MetaTextReportAsset();
    pAsset17.id = "war_quiet";
    pAsset17.color = "#ADADAD";
    pAsset17.report_action = (MetaTextReportAction) (pObject =>
    {
      War pWar = pObject as War;
      return !pWar.areAttackersGettingCaptured() && !pWar.areDefendersGettingCaptured();
    });
    this.add(pAsset17);
    MetaTextReportAsset pAsset18 = new MetaTextReportAsset();
    pAsset18.id = "war_full_on_battle";
    pAsset18.color = "#ADADAD";
    pAsset18.report_action = (MetaTextReportAction) (pObject =>
    {
      War pWar = pObject as War;
      return pWar.areAttackersGettingCaptured() && pWar.areDefendersGettingCaptured();
    });
    this.add(pAsset18);
  }

  private void addGeneralMeta()
  {
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (MetaTextReportAsset pAsset in this.list)
    {
      foreach (string localeId in pAsset.getLocaleIDs())
        this.checkLocale((Asset) pAsset, localeId);
    }
  }
}
