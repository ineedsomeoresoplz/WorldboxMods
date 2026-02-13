// Decompiled with JetBrains decompiler
// Type: OpinionLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class OpinionLibrary : AssetLibrary<OpinionAsset>
{
  public override void init()
  {
    base.init();
    OpinionAsset pAsset1 = new OpinionAsset();
    pAsset1.id = "opinion_king";
    pAsset1.translation_key = "opinion_king";
    pAsset1.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num = 0;
      if (pTarget.hasKing())
        num += (int) pTarget.king.stats["diplomacy"];
      return num;
    });
    this.add(pAsset1);
    OpinionAsset pAsset2 = new OpinionAsset();
    pAsset2.id = "opinion_kings_mood";
    pAsset2.translation_key = "opinion_kings_mood";
    pAsset2.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num = 0;
      if (pMain.hasKing())
        num = (int) pMain.king.stats["opinion"];
      return num;
    });
    this.add(pAsset2);
    OpinionAsset pAsset3 = new OpinionAsset();
    pAsset3.id = "opinion_is_supreme";
    pAsset3.translation_key = "opinion_is_supreme";
    pAsset3.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num = 0;
      if (pTarget.isSupreme() && World.world.kingdoms.Count >= 3)
        num = -100;
      return num;
    });
    this.add(pAsset3);
    OpinionAsset pAsset4 = new OpinionAsset();
    pAsset4.id = "opinion_borders";
    pAsset4.translation_key = "opinion_far_borders";
    pAsset4.translation_key_negative = "opinion_close_borders";
    pAsset4.calc = (OpinionDelegateCalc) ((pMain, pTarget) => !DiplomacyHelpers.areKingdomsClose(pMain, pTarget) ? 25 : -25);
    this.add(pAsset4);
    OpinionAsset pAsset5 = new OpinionAsset();
    pAsset5.id = "opinion_far_lands";
    pAsset5.translation_key = "opinion_far_lands";
    pAsset5.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num = 0;
      if (!DiplomacyHelpers.areKingdomsClose(pMain, pTarget) && pMain.hasCapital() && pTarget.hasCapital())
      {
        WorldTile tile1 = pMain.capital.getTile();
        WorldTile tile2 = pTarget.capital.getTile();
        if (tile1 != null && tile2 != null && !tile1.isSameIsland(tile2))
          num = 60;
      }
      return num;
    });
    this.add(pAsset5);
    OpinionAsset pAsset6 = new OpinionAsset();
    pAsset6.id = "opinion_in_war";
    pAsset6.translation_key = "opinion_in_war";
    pAsset6.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num = 0;
      if (pMain.isEnemy(pTarget))
        num = -500;
      return num;
    });
    this.add(pAsset6);
    OpinionAsset pAsset7 = new OpinionAsset();
    pAsset7.id = "opinion_same_wars";
    pAsset7.translation_key = "opinion_same_wars";
    pAsset7.calc = (OpinionDelegateCalc) ((pMain, pTarget) => pMain.isEnemy(pTarget) || !pMain.isInWarOnSameSide(pTarget) ? 0 : 50);
    this.add(pAsset7);
    OpinionAsset pAsset8 = new OpinionAsset();
    pAsset8.id = "opinion_species";
    pAsset8.translation_key = "opinion_same_species";
    pAsset8.translation_key_negative = "opinion_different_species";
    pAsset8.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      Actor king1 = pMain.king;
      Actor king2 = pTarget.king;
      return king1 == null || king2 == null || !king1.canHavePrejudice() ? 0 : (!(pMain.getSpecies() == pTarget.getSpecies()) ? -10 : 15);
    });
    this.add(pAsset8);
    OpinionAsset pAsset9 = new OpinionAsset();
    pAsset9.id = "opinion_zones";
    pAsset9.translation_key = "opinion_zones";
    pAsset9.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num1 = 0;
      int num2 = 0;
      foreach (City city in pMain.getCities())
        num1 += city.zones.Count;
      foreach (City city in pTarget.getCities())
        num2 += city.zones.Count;
      int num3 = (num1 - num2) / 5;
      if (num3 > 0)
        num3 = 0;
      if (num3 < -20)
        num3 = -20;
      return num3;
    });
    this.add(pAsset9);
    OpinionAsset pAsset10 = new OpinionAsset();
    pAsset10.id = "opinion_peace_time";
    pAsset10.translation_key = "opinion_peace_time";
    pAsset10.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      if (pMain.isEnemy(pTarget))
        return 0;
      DiplomacyRelation relation = World.world.diplomacy.getRelation(pMain, pTarget);
      if (relation == null)
        return 0;
      float yearsSince = (float) Date.getYearsSince(relation.data.timestamp_last_war_ended);
      if ((double) yearsSince <= (double) SimGlobals.m.minimum_years_between_wars)
        return 0;
      int num = (int) yearsSince;
      if ((double) yearsSince > 20.0)
        num = 20;
      return num;
    });
    this.add(pAsset10);
    OpinionAsset pAsset11 = new OpinionAsset();
    pAsset11.id = "opinion_power";
    pAsset11.translation_key = "opinion_power";
    pAsset11.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num4 = 0;
      int num5 = pMain.countCities() * 5 + pMain.getPopulationPeople();
      int num6 = pTarget.countCities() * 5 + pTarget.getPopulationPeople() - num5;
      if (num6 > 0)
        num4 = num6 / 10;
      return num4;
    });
    this.add(pAsset11);
    OpinionAsset pAsset12 = new OpinionAsset();
    pAsset12.id = "opinion_loyalty_traits";
    pAsset12.translation_key = "opinion_traits";
    pAsset12.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num7 = 0;
      if (pTarget.hasCulture() && pMain.hasKing() && pTarget.hasKing())
      {
        int num8 = AssetManager.traits.checkTraitsMod(pTarget.king, pMain.king);
        num7 += num8;
      }
      return num7;
    });
    this.add(pAsset12);
    OpinionAsset pAsset13 = new OpinionAsset();
    pAsset13.id = "opinion_culture";
    pAsset13.translation_key = "opinion_culture_same";
    pAsset13.translation_key_negative = "opinion_culture_different";
    pAsset13.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num = 0;
      if (pTarget.hasCulture())
        num = pTarget.getCulture() != pMain.getCulture() ? -15 : 15;
      return num;
    });
    this.add(pAsset13);
    OpinionAsset pAsset14 = new OpinionAsset();
    pAsset14.id = "opinion_religion";
    pAsset14.translation_key = "opinion_religion_same";
    pAsset14.translation_key_negative = "opinion_religion_different";
    pAsset14.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num = 0;
      if (pTarget.hasReligion())
        num = pTarget.getReligion() != pMain.getReligion() ? -15 : 15;
      return num;
    });
    this.add(pAsset14);
    OpinionAsset pAsset15 = new OpinionAsset();
    pAsset15.id = "opinion_language";
    pAsset15.translation_key = "opinion_language_same";
    pAsset15.translation_key_negative = "opinion_language_different";
    pAsset15.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num = 0;
      if (pTarget.hasLanguage())
        num = pTarget.getLanguage() != pMain.getLanguage() ? -15 : 15;
      return num;
    });
    this.add(pAsset15);
    OpinionAsset pAsset16 = new OpinionAsset();
    pAsset16.id = "opinion_subspecies";
    pAsset16.translation_key = "opinion_subspecies_same";
    pAsset16.translation_key_negative = "opinion_subspecies_different";
    pAsset16.calc = (OpinionDelegateCalc) ((pMain, pTarget) => !pMain.hasKing() || pMain.getSpecies() == pTarget.getSpecies() || !pMain.king.canHavePrejudice() ? 0 : (pTarget.getMainSubspecies() != pMain.getMainSubspecies() ? -10 : 10));
    this.add(pAsset16);
    OpinionAsset pAsset17 = new OpinionAsset();
    pAsset17.id = "opinion_clan";
    pAsset17.translation_key = "opinion_same_clan";
    pAsset17.translation_key_negative = "opinion_different_clan";
    pAsset17.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      Actor king3 = pMain.king;
      Actor king4 = pTarget.king;
      return king3 == null || king4 == null || king3.subspecies != king4.subspecies || !king3.hasClan() || !king4.hasClan() || !king3.canHavePrejudice() ? 0 : (king3.clan != king4.clan ? -40 : 40);
    });
    this.add(pAsset17);
    OpinionAsset pAsset18 = new OpinionAsset();
    pAsset18.id = "opinion_in_alliance";
    pAsset18.translation_key = "opinion_in_alliance";
    pAsset18.calc = (OpinionDelegateCalc) ((pMain, pTarget) =>
    {
      int num = 0;
      if (pTarget.hasAlliance() && pMain.hasAlliance() && pTarget.getAlliance() == pMain.getAlliance())
        num = 30;
      return num;
    });
    this.add(pAsset18);
    OpinionAsset pAsset19 = new OpinionAsset();
    pAsset19.id = "opinion_truce";
    pAsset19.translation_key = "opinion_truce";
    pAsset19.calc = (OpinionDelegateCalc) ((pMain, pTarget) => pMain.isEnemy(pTarget) || (double) Date.getYearsSince(World.world.diplomacy.getRelation(pMain, pTarget).data.timestamp_last_war_ended) > (double) SimGlobals.m.minimum_years_between_wars ? 0 : 100);
    this.add(pAsset19);
    OpinionAsset pAsset20 = new OpinionAsset();
    pAsset20.id = "opinion_world_era";
    pAsset20.translation_key = "opinion_world_era";
    pAsset20.calc = (OpinionDelegateCalc) ((pMain, pTarget) => World.world_era.bonus_opinion);
    this.add(pAsset20);
    OpinionAsset pAsset21 = new OpinionAsset();
    pAsset21.id = "opinion_baby_king";
    pAsset21.translation_key = "opinion_baby_king";
    pAsset21.calc = (OpinionDelegateCalc) ((pMain, pTarget) => !pMain.hasKing() || !pTarget.hasKing() || pMain.king.isBaby() || !pTarget.king.isBaby() ? 0 : -50);
    this.add(pAsset21);
    OpinionAsset pAsset22 = new OpinionAsset();
    pAsset22.id = "opinion_ethnocentric_guard";
    pAsset22.translation_key = "opinion_ethnocentric_guard";
    pAsset22.calc = (OpinionDelegateCalc) ((pMain, pTarget) => !pMain.hasCulture() || !pMain.culture.hasTrait("ethnocentric_guard") || pMain.getMainSubspecies() != pTarget.getMainSubspecies() || pMain.culture == pTarget.culture ? 0 : -50);
    this.add(pAsset22);
    OpinionAsset pAsset23 = new OpinionAsset();
    pAsset23.id = "opinion_xenophobic";
    pAsset23.translation_key = "opinion_xenophobic";
    pAsset23.calc = (OpinionDelegateCalc) ((pMain, pTarget) => !pMain.hasCulture() || !pMain.culture.hasTrait("xenophobic") || pMain.getMainSubspecies() == pTarget.getMainSubspecies() ? 0 : -50);
    this.add(pAsset23);
    OpinionAsset pAsset24 = new OpinionAsset();
    pAsset24.id = "opinion_xenophiles";
    pAsset24.translation_key = "opinion_xenophiles";
    pAsset24.calc = (OpinionDelegateCalc) ((pMain, pTarget) => !pMain.hasCulture() || !pMain.culture.hasTrait("xenophiles") || pMain.getMainSubspecies() != pTarget.getMainSubspecies() ? 0 : 20);
    this.add(pAsset24);
  }

  public override void editorDiagnosticLocales()
  {
    foreach (OpinionAsset pAsset in this.list)
    {
      foreach (string localeId in pAsset.getLocaleIDs())
        this.checkLocale((Asset) pAsset, localeId);
    }
    base.editorDiagnosticLocales();
  }
}
