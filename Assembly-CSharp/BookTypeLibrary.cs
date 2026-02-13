// Decompiled with JetBrains decompiler
// Type: BookTypeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BookTypeLibrary : AssetLibrary<BookTypeAsset>
{
  public override void init()
  {
    base.init();
    BookTypeAsset pAsset1 = new BookTypeAsset();
    pAsset1.id = "family_story";
    pAsset1.name_template = "book_name_love_story";
    pAsset1.color_text = "#FFA94D";
    pAsset1.writing_rate = 3;
    pAsset1.path_icons = "family_story/";
    pAsset1.requirement_check = (BookRequirementCheck) ((a, _) => a.hasFamily());
    this.add(pAsset1);
    this.t.base_stats["mana"] = 10f;
    this.t.base_stats["happiness"] = 5f;
    this.t.base_stats["experience"] = 5f;
    BookTypeAsset pAsset2 = new BookTypeAsset();
    pAsset2.id = "love_story";
    pAsset2.name_template = "book_name_love_story";
    pAsset2.color_text = "#FF6B6B";
    pAsset2.writing_rate = 3;
    pAsset2.path_icons = "love_story/";
    pAsset2.requirement_check = (BookRequirementCheck) ((a, _) => a.hasFamily());
    this.add(pAsset2);
    this.t.base_stats["mana"] = 10f;
    this.t.base_stats["happiness"] = 10f;
    this.t.base_stats["experience"] = 5f;
    BookTypeAsset pAsset3 = new BookTypeAsset();
    pAsset3.id = "friendship_story";
    pAsset3.name_template = "book_name_love_story";
    pAsset3.color_text = "#74C0FC";
    pAsset3.writing_rate = 3;
    pAsset3.path_icons = "friendship_story/";
    pAsset3.requirement_check = (BookRequirementCheck) ((a, _) => a.hasFamily());
    this.add(pAsset3);
    this.t.base_stats["mana"] = 10f;
    this.t.base_stats["happiness"] = 5f;
    this.t.base_stats["experience"] = 5f;
    BookTypeAsset pAsset4 = new BookTypeAsset();
    pAsset4.id = "bad_story_about_king";
    pAsset4.name_template = "book_name_bad_story";
    pAsset4.color_text = "#FFD700";
    pAsset4.writing_rate = 3;
    pAsset4.path_icons = "bad_story/";
    pAsset4.requirement_check = (BookRequirementCheck) ((a, _) =>
    {
      if (!a.kingdom.hasKing() || a.isKing())
        return false;
      Actor king = a.kingdom.king;
      return king.hasFamily() && king.hasClan();
    });
    this.add(pAsset4);
    this.t.base_stats["happiness"] = -10f;
    this.t.base_stats["experience"] = 10f;
    BookTypeAsset pAsset5 = new BookTypeAsset();
    pAsset5.id = "fable";
    pAsset5.name_template = "book_name_fable";
    pAsset5.color_text = "#9ACD32";
    pAsset5.writing_rate = 3;
    pAsset5.path_icons = "fable/";
    pAsset5.requirement_check = (BookRequirementCheck) ((a, _) => a.hasWeapon() && a.kingdom.hasKing() && a.kingdom.king.hasClan() && a.hasCity() && a.city.hasLeader());
    this.add(pAsset5);
    this.t.base_stats["mana"] = 20f;
    this.t.base_stats["happiness"] = 10f;
    this.t.base_stats["experience"] = 30f;
    BookTypeAsset pAsset6 = new BookTypeAsset();
    pAsset6.id = "warfare_manual";
    pAsset6.name_template = "book_name_warfare_manual";
    pAsset6.color_text = "#E8590C";
    pAsset6.path_icons = "warfare_manual/";
    pAsset6.rate_calc = (BookRateCalc) ((a, _) => (int) a.stats["warfare"]);
    pAsset6.requirement_check = (BookRequirementCheck) ((a, _) => a.kingdom.hasKing() && a.kingdom.king.hasClan() && a.hasCity() && a.city.hasLeader() && a.city.countWarriors() != 0);
    this.add(pAsset6);
    this.t.base_stats["warfare"] = 1f;
    this.t.base_stats["intelligence"] = 1f;
    this.t.base_stats["experience"] = 5f;
    BookTypeAsset pAsset7 = new BookTypeAsset();
    pAsset7.id = "economy_manual";
    pAsset7.name_template = "book_name_economy_manual";
    pAsset7.path_icons = "economy_manual/";
    pAsset7.color_text = "#EAC645";
    pAsset7.rate_calc = (BookRateCalc) ((a, b) => (int) a.stats["stewardship"]);
    pAsset7.requirement_check = (BookRequirementCheck) ((a, _) => a.kingdom.hasKing() && a.kingdom.king.hasClan() && a.hasCity() && a.city.hasLeader());
    this.add(pAsset7);
    this.t.base_stats["happiness"] = -10f;
    this.t.base_stats["stewardship"] = 1f;
    this.t.base_stats["intelligence"] = 1f;
    this.t.base_stats["experience"] = 10f;
    BookTypeAsset pAsset8 = new BookTypeAsset();
    pAsset8.id = "stewardship_manual";
    pAsset8.name_template = "book_name_stewardship_manual";
    pAsset8.color_text = "#D4A373";
    pAsset8.path_icons = "stewardship_manual/";
    pAsset8.rate_calc = (BookRateCalc) ((a, b) => (int) a.stats["stewardship"]);
    pAsset8.requirement_check = (BookRequirementCheck) ((a, _) => a.kingdom.hasKing() && a.kingdom.king.hasClan() && a.hasCity() && a.city.hasLeader());
    this.add(pAsset8);
    this.t.base_stats["happiness"] = -5f;
    this.t.base_stats["stewardship"] = 1f;
    this.t.base_stats["intelligence"] = 1f;
    this.t.base_stats["experience"] = 10f;
    BookTypeAsset pAsset9 = new BookTypeAsset();
    pAsset9.id = "diplomacy_manual";
    pAsset9.name_template = "book_name_diplomacy_manual";
    pAsset9.color_text = "#66D9E8";
    pAsset9.path_icons = "diplomacy_manual/";
    pAsset9.rate_calc = (BookRateCalc) ((a, _) => (int) a.stats["diplomacy"]);
    pAsset9.requirement_check = (BookRequirementCheck) ((a, _) => a.kingdom.hasKing() && a.kingdom.king.hasClan() && a.hasCity() && a.city.hasLeader());
    this.add(pAsset9);
    this.t.base_stats["happiness"] = -10f;
    this.t.base_stats["diplomacy"] = 1f;
    this.t.base_stats["experience"] = 10f;
    BookTypeAsset pAsset10 = new BookTypeAsset();
    pAsset10.id = "mathbook";
    pAsset10.name_template = "book_name_math";
    pAsset10.color_text = "#3BC9DB";
    pAsset10.path_icons = "mathbook/";
    pAsset10.rate_calc = (BookRateCalc) ((a, _) => (int) a.stats["intelligence"]);
    pAsset10.requirement_check = (BookRequirementCheck) ((a, _) => a.kingdom.hasKing() && a.kingdom.king.hasClan() && a.hasCity() && a.city.hasLeader() && a.hasCulture());
    this.add(pAsset10);
    this.t.base_stats["happiness"] = -20f;
    this.t.base_stats["intelligence"] = 1f;
    this.t.base_stats["experience"] = 30f;
    BookTypeAsset pAsset11 = new BookTypeAsset();
    pAsset11.id = "biology_book";
    pAsset11.name_template = "book_name_biology";
    pAsset11.path_icons = "biology_book/";
    pAsset11.color_text = "#80C980";
    pAsset11.rate_calc = (BookRateCalc) ((a, _) => (int) a.stats["intelligence"]);
    pAsset11.requirement_check = (BookRequirementCheck) ((a, _) => a.kingdom.hasKing() && a.kingdom.king.hasClan() && a.hasCity() && a.city.hasLeader() && a.hasSubspecies());
    this.add(pAsset11);
    this.t.base_stats["happiness"] = 10f;
    this.t.base_stats["intelligence"] = 1f;
    this.t.base_stats["experience"] = 30f;
    BookTypeAsset pAsset12 = new BookTypeAsset();
    pAsset12.id = "history_book";
    pAsset12.name_template = "book_name_history";
    pAsset12.path_icons = "history_book/";
    pAsset12.color_text = "#C49A6C";
    pAsset12.rate_calc = (BookRateCalc) ((a, _) => (int) a.stats["intelligence"]);
    pAsset12.requirement_check = (BookRequirementCheck) ((a, _) => a.kingdom.hasKing() && a.kingdom.king.hasClan() && a.hasCity() && a.city.hasLeader() && a.hasSubspecies());
    this.add(pAsset12);
    this.t.base_stats["happiness"] = 5f;
    this.t.base_stats["warfare"] = 1f;
    this.t.base_stats["diplomacy"] = 1f;
    this.t.base_stats["intelligence"] = 1f;
    this.t.base_stats["experience"] = 30f;
  }

  public override void editorDiagnostic()
  {
    base.editorDiagnostic();
    foreach (BookTypeAsset pAsset in this.list)
      this.checkSpriteExists("path_icons", pAsset.getFullIconPath(), (Asset) pAsset);
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (BookTypeAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
    }
  }
}
