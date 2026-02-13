// Decompiled with JetBrains decompiler
// Type: LanguageTraitLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class LanguageTraitLibrary : BaseTraitLibrary<LanguageTrait>
{
  protected override List<string> getDefaultTraitsForMeta(ActorAsset pAsset)
  {
    return pAsset.default_language_traits;
  }

  public override void init()
  {
    base.init();
    LanguageTrait pAsset1 = new LanguageTrait();
    pAsset1.id = "melodic";
    pAsset1.value = 3f;
    pAsset1.group_id = "harmony";
    this.add(pAsset1);
    LanguageTrait pAsset2 = new LanguageTrait();
    pAsset2.id = "stylish_writing";
    pAsset2.value = 3f;
    pAsset2.group_id = "harmony";
    this.add(pAsset2);
    LanguageTrait pAsset3 = new LanguageTrait();
    pAsset3.id = "scribble";
    pAsset3.group_id = "knowledge";
    this.add(pAsset3);
    this.t.addOpposite("nicely_structured_grammar");
    LanguageTrait pAsset4 = new LanguageTrait();
    pAsset4.id = "nicely_structured_grammar";
    pAsset4.value = 2f;
    pAsset4.group_id = "knowledge";
    this.add(pAsset4);
    this.t.addOpposite("scribble");
    LanguageTrait pAsset5 = new LanguageTrait();
    pAsset5.id = "beautiful_calligraphy";
    pAsset5.value = 2f;
    pAsset5.group_id = "harmony";
    this.add(pAsset5);
    LanguageTrait pAsset6 = new LanguageTrait();
    pAsset6.id = "magic_words";
    pAsset6.group_id = "spirit";
    this.add(pAsset6);
    this.t.setUnlockedWithAchievement("achievementTraitExplorerLanguage");
    LanguageTrait pAsset7 = new LanguageTrait();
    pAsset7.id = "words_of_madness";
    pAsset7.value = 0.1f;
    pAsset7.group_id = "chaos";
    pAsset7.spawn_random_trait_allowed = false;
    pAsset7.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (a.hasTrait("evil") || a.hasTrait("blessed") || !Randy.randomChance(pTrait.value))
        return;
      a.addTrait("madness");
    });
    this.add(pAsset7);
    this.t.setUnlockedWithAchievement("achievementCursedWorld");
    LanguageTrait pAsset8 = new LanguageTrait();
    pAsset8.id = "cursed_font";
    pAsset8.value = 0.2f;
    pAsset8.group_id = "chaos";
    pAsset8.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (a.hasTrait("evil") || !Randy.randomChance(pTrait.value))
        return;
      a.addStatusEffect("cursed");
    });
    this.add(pAsset8);
    this.t.setUnlockedWithAchievement("achievementTheCorruptedTrees");
    LanguageTrait pAsset9 = new LanguageTrait();
    pAsset9.id = "font_of_gods";
    pAsset9.value = 0.2f;
    pAsset9.group_id = "spirit";
    pAsset9.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (!Randy.randomChance(pTrait.value))
        return;
      a.addStatusEffect("enchanted");
    });
    this.add(pAsset9);
    LanguageTrait pAsset10 = new LanguageTrait();
    pAsset10.id = "chilly_font";
    pAsset10.value = 0.4f;
    pAsset10.group_id = "chaos";
    pAsset10.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (!Randy.randomChance(pTrait.value))
        return;
      a.addStatusEffect("frozen");
    });
    this.add(pAsset10);
    LanguageTrait pAsset11 = new LanguageTrait();
    pAsset11.id = "ancient_runes";
    pAsset11.value = 0.4f;
    pAsset11.group_id = "spirit";
    pAsset11.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (!Randy.randomChance(pTrait.value))
        return;
      a.addStatusEffect("spell_boost");
    });
    this.add(pAsset11);
    this.t.setUnlockedWithAchievement("achievementPlotsExplorer");
    LanguageTrait pAsset12 = new LanguageTrait();
    pAsset12.id = "repeated_sentences";
    pAsset12.value = 0.1f;
    pAsset12.group_id = "miscellaneous";
    pAsset12.spawn_random_trait_allowed = false;
    pAsset12.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (!Randy.randomChance(pTrait.value) || !a.hasCity() || !a.asset.can_be_cloned || !a.city.hasFreeHouseSlots() || a.city.hasReachedWorldLawLimit() || a.hasSubspecies() && a.subspecies.hasReachedPopulationLimit())
        return;
      World.world.units.cloneUnit(a);
    });
    this.add(pAsset12);
    this.t.setUnlockedWithAchievement("achievementCloneWars");
    LanguageTrait pAsset13 = new LanguageTrait();
    pAsset13.id = "spooky_language";
    pAsset13.value = 0.2f;
    pAsset13.group_id = "chaos";
    pAsset13.spawn_random_trait_allowed = false;
    pAsset13.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (!Randy.randomChance(pTrait.value) || !a.hasCity())
        return;
      World.world.units.spawnNewUnit("ghost", a.current_tile.neighboursAll.GetRandom<WorldTile>());
    });
    this.add(pAsset13);
    this.t.setUnlockedWithAchievement("achievementChildNamedToto");
    LanguageTrait pAsset14 = new LanguageTrait();
    pAsset14.id = "powerful_words";
    pAsset14.value = 0.2f;
    pAsset14.group_id = "spirit";
    pAsset14.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (!Randy.randomChance(pTrait.value))
        return;
      a.addStatusEffect("powerup");
    });
    this.add(pAsset14);
    LanguageTrait pAsset15 = new LanguageTrait();
    pAsset15.id = "confusing_semantics";
    pAsset15.value = 0.2f;
    pAsset15.group_id = "chaos";
    pAsset15.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (!Randy.randomChance(pTrait.value))
        return;
      a.makeStunned();
      if (!a.hasLanguage())
        return;
      a.joinLanguage((Language) null);
    });
    this.add(pAsset15);
    LanguageTrait pAsset16 = new LanguageTrait();
    pAsset16.id = "raging_paragraphs";
    pAsset16.value = 0.2f;
    pAsset16.group_id = "chaos";
    pAsset16.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (!Randy.randomChance(pTrait.value))
        return;
      a.addStatusEffect("rage");
    });
    this.add(pAsset16);
    LanguageTrait pAsset17 = new LanguageTrait();
    pAsset17.id = "mortal_tongue";
    pAsset17.value = 0.2f;
    pAsset17.group_id = "chaos";
    pAsset17.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (a.hasTrait("evil") || a.hasTrait("blessed") || !Randy.randomChance(pTrait.value))
        return;
      a.getHitFullHealth(AttackType.Divine);
    });
    this.add(pAsset17);
    this.t.setUnlockedWithAchievement("achievementCursedWorld");
    LanguageTrait pAsset18 = new LanguageTrait();
    pAsset18.id = "scorching_words";
    pAsset18.value = 0.5f;
    pAsset18.group_id = "chaos";
    pAsset18.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (!Randy.randomChance(pTrait.value))
        return;
      a.addStatusEffect("burning");
    });
    this.add(pAsset18);
    LanguageTrait pAsset19 = new LanguageTrait();
    pAsset19.id = "doomed_glyphs";
    pAsset19.value = 0.1f;
    pAsset19.group_id = "chaos";
    pAsset19.spawn_random_trait_allowed = false;
    pAsset19.read_book_trait_action = (BookTraitAction) ((a, pTrait, _) =>
    {
      if (a.hasTrait("lucky") || !WorldLawLibrary.world_law_disasters_nature.isEnabled() || !Randy.randomChance(pTrait.value))
        return;
      Meteorite.spawnMeteoriteDisaster(a.current_tile, a);
    });
    this.add(pAsset19);
    this.t.setUnlockedWithAchievement("achievementMultiplySpoken");
    LanguageTrait pAsset20 = new LanguageTrait();
    pAsset20.id = "enlightening_script";
    pAsset20.group_id = "knowledge";
    this.add(pAsset20);
    this.t.base_stats["intelligence"] = 10f;
    LanguageTrait pAsset21 = new LanguageTrait();
    pAsset21.id = "foolish_glyphs";
    pAsset21.group_id = "knowledge";
    this.add(pAsset21);
    this.t.base_stats["intelligence"] = -5f;
    LanguageTrait pAsset22 = new LanguageTrait();
    pAsset22.id = "eternal_text";
    pAsset22.group_id = "miscellaneous";
    pAsset22.spawn_random_trait_allowed = false;
    this.add(pAsset22);
    this.t.setUnlockedWithAchievement("achievementPie");
    this.t.base_stats["lifespan"] = 500f;
    LanguageTrait pAsset23 = new LanguageTrait();
    pAsset23.id = "elegant_words";
    pAsset23.group_id = "spirit";
    this.add(pAsset23);
    this.t.base_stats["offspring"] = 4f;
    LanguageTrait pAsset24 = new LanguageTrait();
    pAsset24.id = "strict_spelling";
    pAsset24.group_id = "knowledge";
    this.add(pAsset24);
    this.t.base_stats["warfare"] = 10f;
    LanguageTrait pAsset25 = new LanguageTrait();
    pAsset25.id = "divine_encryption";
    pAsset25.group_id = "special";
    pAsset25.can_be_given = false;
    pAsset25.can_be_removed = false;
    pAsset25.can_be_in_book = false;
    pAsset25.spawn_random_trait_allowed = false;
    this.add(pAsset25);
    LanguageTrait pAsset26 = new LanguageTrait();
    pAsset26.id = "grin_mark";
    pAsset26.group_id = "fate";
    pAsset26.spawn_random_trait_allowed = false;
    pAsset26.priority = -100;
    this.add(pAsset26);
    this.t.setTraitInfoToGrinMark();
    this.t.setUnlockedWithAchievement("achievementCreaturesExplorer");
  }

  protected override string icon_path => "ui/Icons/language_traits/";

  public static float getValueFloat(string pID) => AssetManager.language_traits.get(pID).value;

  public static int getValue(string pID) => (int) AssetManager.language_traits.get(pID).value;
}
