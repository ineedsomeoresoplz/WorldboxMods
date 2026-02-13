// Decompiled with JetBrains decompiler
// Type: KingdomLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class KingdomLibrary : AssetLibrary<KingdomAsset>
{
  private const string TEMPLATE_MOB = "$TEMPLATE_MOB$";
  private const string TEMPLATE_MOB_GOOD = "$TEMPLATE_MOB_GOOD$";
  private const string TEMPLATE_MOB_VERY_GOOD = "$TEMPLATE_MOB_VERY_GOOD$";
  private const string TEMPLATE_ANIMAL = "$TEMPLATE_ANIMAL$";
  private const string TEMPLATE_ANIMAL_NEUTRAL = "$TEMPLATE_ANIMAL_NEUTRAL$";
  private const string TEMPLATE_ANIMAL_PEACEFUL = "$TEMPLATE_ANIMAL_PEACEFUL$";
  private const string TEMPLATE_CIV = "$TEMPLATE_CIV$";
  private const string TEMPLATE_CIV_GOOD = "$TEMPLATE_CIV_GOOD$";
  private const string TEMPLATE_NOMAD = "$TEMPLATE_NOMAD$";
  private const string TEMPLATE_CIV_NEW = "$TEMPLATE_CIV_NEW$";
  private ColorAsset _shared_default_color;

  public override void init()
  {
    base.init();
    this._shared_default_color = ColorAsset.tryMakeNewColorAsset("#888888");
    this._shared_default_color.id = "SHARED_COLOR";
    this.addTemplates();
    this.addNeutral();
    this.addNomads();
    this.addNewCivs();
    this.addCivs();
    this.addAnimals();
    this.addUnique();
    this.addMobs();
    this.addAnimalMinicivs();
    this.addCoolMinicivs();
    this.addCreeps();
  }

  private void addTemplates()
  {
    KingdomAsset pAsset1 = new KingdomAsset();
    pAsset1.id = "$TEMPLATE_MOB$";
    pAsset1.mobs = true;
    this.add(pAsset1);
    this.clone("$TEMPLATE_MOB_GOOD$", "$TEMPLATE_MOB$");
    this.t.addTag("good");
    this.t.addFriendlyTag("good");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("civ");
    this.t.addEnemyTag("orc");
    this.t.addEnemyTag("bandit");
    this.clone("$TEMPLATE_MOB_VERY_GOOD$", "$TEMPLATE_MOB_GOOD$");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("living_houses");
    this.t.addFriendlyTag("living_plants");
    this.t.addFriendlyTag("snowman");
    this.t.addEnemyTag("wolf");
    this.t.addEnemyTag("bear");
    this.clone("$TEMPLATE_ANIMAL$", "$TEMPLATE_MOB$");
    this.t.addTag("nature_creature");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("neutral_animals");
    this.clone("$TEMPLATE_ANIMAL_NEUTRAL$", "$TEMPLATE_MOB$");
    this.t.count_as_danger = false;
    this.t.addTag("neutral_animals");
    this.t.addTag("neutral");
    this.clone("$TEMPLATE_ANIMAL_PEACEFUL$", "$TEMPLATE_ANIMAL_NEUTRAL$");
    this.t.addFriendlyTag("good");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("civ");
    KingdomAsset pAsset2 = new KingdomAsset();
    pAsset2.id = "$TEMPLATE_CIV$";
    pAsset2.civ = true;
    this.add(pAsset2);
    this.t.addTag("civ");
    this.t.addEnemyTag("bandit");
    this.clone("$TEMPLATE_CIV_GOOD$", "$TEMPLATE_CIV$");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("good");
    this.clone("$TEMPLATE_NOMAD$", "$TEMPLATE_CIV_GOOD$");
    this.t.addFriendlyTag("neutral");
    this.t.civ = false;
    this.t.mobs = true;
    this.t.nomads = true;
    this.clone("$TEMPLATE_CIV_NEW$", "$TEMPLATE_CIV_GOOD$");
    this.t.addFriendlyTag("neutral");
  }

  private void addNomads()
  {
    this.clone("nomads_human", "$TEMPLATE_NOMAD$");
    this.t.group_main = true;
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#BACADD");
    this.t.setIcon("ui/Icons/iconHumans");
    this.t.addTag("human");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("human");
    this.clone("nomads_elf", "$TEMPLATE_NOMAD$");
    this.t.group_main = true;
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#98DB8C");
    this.t.setIcon("ui/Icons/iconElves");
    this.t.addTag("elf");
    this.t.addTag("nature_creature");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("elf");
    this.t.addFriendlyTag("nature_creature");
    this.clone("nomads_orc", "$TEMPLATE_NOMAD$");
    this.t.group_main = true;
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#FFCD70");
    this.t.setIcon("ui/Icons/iconOrcs");
    this.t.civ = false;
    this.t.mobs = true;
    this.t.addTag("orc");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("orc");
    this.t.addFriendlyTag("golden_brain");
    this.t.addFriendlyTag("wolf");
    this.t.addFriendlyTag("hyena");
    this.clone("nomads_dwarf", "$TEMPLATE_NOMAD$");
    this.t.group_main = true;
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#B1A0FF");
    this.t.setIcon("ui/Icons/iconDwarf");
    this.t.addTag("dwarf");
    this.t.addFriendlyTag("dwarf");
    this.t.addFriendlyTag("civ_crystal_golem");
  }

  private void addCivs()
  {
    this.clone("human", "nomads_human");
    this.t.group_main = true;
    this.t.clearKingdomColor();
    this.t.setIcon("ui/Icons/iconHumans");
    this.t.civ = true;
    this.t.mobs = false;
    this.clone("elf", "nomads_elf");
    this.t.group_main = true;
    this.t.clearKingdomColor();
    this.t.setIcon("ui/Icons/iconElves");
    this.t.civ = true;
    this.t.mobs = false;
    this.clone("dwarf", "nomads_dwarf");
    this.t.group_main = true;
    this.t.clearKingdomColor();
    this.t.setIcon("ui/Icons/iconDwarf");
    this.t.civ = true;
    this.t.mobs = false;
    this.clone("orc", "nomads_orc");
    this.t.group_main = true;
    this.t.clearKingdomColor();
    this.t.setIcon("ui/Icons/iconOrcs");
    this.t.civ = true;
    this.t.mobs = false;
  }

  private void addNewCivs()
  {
    this.clone("civ_cat", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_cat");
    this.t.addTag("sliceable");
    this.clone("civ_dog", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_dog");
    this.t.addTag("sliceable");
    this.clone("civ_chicken", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_chicken");
    this.t.addTag("sliceable");
    this.clone("civ_rabbit", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_rabbit");
    this.t.addTag("sliceable");
    this.clone("civ_monkey", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_monkey");
    this.t.addTag("sliceable");
    this.clone("civ_fox", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_fox");
    this.t.addTag("sliceable");
    this.clone("civ_sheep", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_sheep");
    this.t.addTag("sliceable");
    this.clone("civ_cow", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_cow");
    this.t.addTag("sliceable");
    this.clone("civ_armadillo", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_armadillo");
    this.clone("civ_wolf", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_wolf");
    this.t.addTag("sliceable");
    this.clone("civ_bear", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_bear");
    this.t.addTag("sliceable");
    this.clone("civ_rhino", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_rhino");
    this.clone("civ_buffalo", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_buffalo");
    this.t.addTag("sliceable");
    this.clone("civ_hyena", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_hyena");
    this.t.addTag("sliceable");
    this.clone("civ_rat", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_rat");
    this.t.addTag("sliceable");
    this.clone("civ_alpaca", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_alpaca");
    this.t.addTag("sliceable");
    this.clone("civ_capybara", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_capybara");
    this.t.friendship_for_everyone = true;
    this.t.addFriendlyTag("everyone");
    this.clone("civ_goat", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_goat");
    this.t.addTag("sliceable");
    this.clone("civ_crab", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_crab");
    this.t.addFriendlyTag("crab");
    this.clone("civ_scorpion", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_scorpion");
    this.clone("civ_penguin", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_penguin");
    this.t.addTag("sliceable");
    this.clone("civ_turtle", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_turtle");
    this.clone("civ_crocodile", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_crocodile");
    this.t.addTag("sliceable");
    this.clone("civ_snake", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_snake");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("snake");
    this.t.addFriendlyTag("miniciv_snake");
    this.clone("civ_frog", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_frog");
    this.t.addTag("sliceable");
    this.clone("civ_piranha", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_piranha");
    this.clone("civ_liliar", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_liliar");
    this.t.addTag("sliceable");
    this.clone("civ_garlic_man", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_garlic_man");
    this.t.addTag("garlic");
    this.clone("civ_lemon_man", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_lemon_man");
    this.t.addTag("sliceable");
    this.clone("civ_acid_gentleman", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_acid_gentleman");
    this.clone("civ_crystal_golem", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_crystal_golem");
    this.t.addFriendlyTag("dwarf");
    this.clone("civ_candy_man", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_candy_man");
    this.clone("civ_beetle", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_beetle");
    this.clone("civ_seal", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_seal");
    this.clone("civ_unicorn", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_unicorn");
    this.clone("civ_ghost", "$TEMPLATE_CIV_NEW$");
    this.t.setIcon("ui/Icons/civs/civ_ghost");
  }

  private void addMobs()
  {
    this.clone("bandit", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconBandit");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#E3362F");
    this.t.addTag("neutral");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("miniciv_bandit");
    this.t.addEnemyTag("civ");
    this.clone("snowman", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconSnowMan");
    this.t.addTag("snow");
    this.t.addFriendlyTag("good");
    this.t.addFriendlyTag("snow");
    this.clone("evil_mage", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconEvilMage");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#E3362F");
    this.t.addTag("evil");
    this.t.addFriendlyTag("demon");
    this.clone("white_mage", "$TEMPLATE_MOB_VERY_GOOD$");
    this.t.setIcon("ui/Icons/iconWhiteMage");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#91E1D6");
    this.clone("necromancer", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconNecromancer");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#81208B");
    this.t.addTag("evil");
    this.t.addFriendlyTag("undead");
    this.t.addFriendlyTag("miniciv_necromancer");
    this.t.addFriendlyTag("fly");
    this.t.addEnemyTag("garlic");
    this.clone("druid", "$TEMPLATE_MOB_GOOD$");
    this.t.setIcon("ui/Icons/iconDruid");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#85C32E");
    this.t.addTag("nature_creature");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("super_pumpkin");
    this.clone("plague_doctor", "$TEMPLATE_MOB_VERY_GOOD$");
    this.t.setIcon("ui/Icons/iconPlagueDoctor");
    this.clone("undead", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconZombie");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#D5D5D5");
    this.t.addFriendlyTag("necromancer");
    this.t.addEnemyTag("garlic");
    this.clone("cold_one", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconWalker");
    this.t.addTag("snow");
    this.t.addFriendlyTag("snow");
    this.clone("demon", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconDemon");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#A30000");
    this.t.addFriendlyTag("fire_elemental");
    this.clone("angle", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconAngle");
    this.t.addTag("good");
    this.t.addTag("nature_creature");
    this.t.addFriendlyTag("good");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("civ");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("super_pumpkin");
    this.t.addFriendlyTag("snowman");
    this.clone("aliens", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconAlien");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("assimilators");
    this.clone("mush", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/actor_traits/iconMushSpores");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("living_plants");
    this.clone("greg", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconGreg");
    this.t.addTag("sliceable");
    this.clone("fire_elemental", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconFireElemental");
    this.t.addFriendlyTag("demon");
    this.t.addFriendlyTag("dragons");
    this.t.addFriendlyTag("fire_skull");
    this.clone("dragons", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconDragon");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("fire_elemental");
    this.clone("living_plants", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconLivingPlants");
    this.t.addTag("nature_creature");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("good");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("mush");
    this.clone("living_houses", "$TEMPLATE_MOB$");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#E53B3B");
    this.t.setIcon("ui/Icons/iconLivingHouse");
    this.t.addFriendlyTag("living_houses");
    this.clone("fire_skull", "$TEMPLATE_MOB$");
    this.t.addTag("undead");
    this.t.addTag("demon");
    this.t.setIcon("ui/Icons/iconFireSkull");
    this.t.addFriendlyTag("demon");
    this.t.addFriendlyTag("dragons");
    this.t.addFriendlyTag("undead");
    this.t.addFriendlyTag("fire_elemental");
    this.clone("jumpy_skull", "$TEMPLATE_MOB$");
    this.t.addTag("undead");
    this.t.setIcon("ui/Icons/iconJumpySkull");
    this.t.addFriendlyTag("undead");
    this.t.addFriendlyTag("fire_skull");
    this.t.addFriendlyTag("necromancer");
    this.clone("fairy", "good");
    this.t.setIcon("ui/Icons/iconFairy");
    this.t.addTag("good");
  }

  private void addAnimals()
  {
    this.clone("cat", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconCat");
    this.t.addTag("small");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("living_houses");
    this.t.addFriendlyTag("snowman");
    this.t.addEnemyTag("snake");
    this.clone("dog", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconDog");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("wolf");
    this.t.addFriendlyTag("human");
    this.t.addEnemyTag("cat");
    this.clone("chicken", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconChicken");
    this.t.addTag("small");
    this.t.addTag("sliceable");
    this.clone("rabbit", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconRabbit");
    this.t.addTag("small");
    this.t.addTag("sliceable");
    this.clone("monkey", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconMonkey");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("living_houses");
    this.t.addFriendlyTag("snowman");
    this.t.addEnemyTag("snake");
    this.clone("fox", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconFox");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("wolf");
    this.t.addFriendlyTag("bear");
    this.clone("sheep", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconSheep");
    this.t.addTag("sliceable");
    this.clone("cow", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconCow");
    this.t.addTag("sliceable");
    this.clone("armadillo", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconArmadillo");
    this.clone("raccoon", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconRaccoon");
    this.t.addTag("sliceable");
    this.t.addTag("small");
    this.t.addFriendlyTag("bandit");
    this.clone("wolf", "$TEMPLATE_ANIMAL$");
    this.t.setIcon("ui/Icons/iconWolf");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("orc");
    this.t.addFriendlyTag("dog");
    this.t.addFriendlyTag("living_houses");
    this.clone("bear", "$TEMPLATE_ANIMAL$");
    this.t.setIcon("ui/Icons/iconBear");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("living_houses");
    this.t.addEnemyTag("rhino");
    this.t.addEnemyTag("crocodile");
    this.clone("rhino", "$TEMPLATE_ANIMAL$");
    this.t.setIcon("ui/Icons/iconRhino");
    this.t.addEnemyTag("hyena");
    this.t.addEnemyTag("snake");
    this.t.addEnemyTag("bear");
    this.t.addEnemyTag("wolf");
    this.t.addEnemyTag("rat");
    this.clone("buffalo", "$TEMPLATE_ANIMAL$");
    this.t.setIcon("ui/Icons/iconBuffalo");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("rhino");
    this.t.addEnemyTag("hyena");
    this.t.addEnemyTag("bear");
    this.t.addEnemyTag("wolf");
    this.t.addEnemyTag("crocodile");
    this.clone("hyena", "$TEMPLATE_ANIMAL$");
    this.t.setIcon("ui/Icons/iconHyena");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("orc");
    this.t.addFriendlyTag("living_houses");
    this.t.addEnemyTag("monkey");
    this.clone("rat", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconRat");
    this.t.addTag("sliceable");
    this.t.addTag("small");
    this.t.addFriendlyTag("civ_acid_gentleman");
    this.t.addFriendlyTag("miniciv_acid_blob");
    this.t.addFriendlyTag("acid_blob");
    this.t.addEnemyTag("cat");
    this.clone("alpaca", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconAlpaca");
    this.t.addTag("sliceable");
    this.clone("capybara", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconCapybara");
    this.t.friendship_for_everyone = true;
    this.t.addFriendlyTag("everyone");
    this.clone("goat", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconGoat");
    this.t.addFriendlyTag("nomads_dwarf");
    this.t.addFriendlyTag("dwarf");
    this.t.addFriendlyTag("civ_crystal_golem");
    this.t.addFriendlyTag("crystal_sword");
    this.clone("penguin", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconPenguin");
    this.t.addTag("sliceable");
    this.t.addFriendlyTag("bandit");
    this.t.addFriendlyTag("super_pumpkin");
    this.clone("ostrich", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconOstrich");
    this.t.addTag("sliceable");
    this.clone("crab", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconCrab");
    this.t.addTag("small");
    this.t.addFriendlyTag("living_houses");
    this.t.addFriendlyTag("snowman");
    this.t.addFriendlyTag("crabzilla");
    this.clone("scorpion", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconScorpion");
    this.clone("turtle", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconTurtle");
    this.clone("crocodile", "$TEMPLATE_ANIMAL$");
    this.t.setIcon("ui/Icons/iconCrocodile");
    this.t.addTag("sliceable");
    this.t.addEnemyTag("chicken");
    this.t.addEnemyTag("monkey");
    this.clone("snake", "$TEMPLATE_ANIMAL_NEUTRAL$");
    this.t.setIcon("ui/Icons/iconSnake");
    this.t.addTag("small");
    this.t.addTag("nature_creature");
    this.t.addFriendlyTag("civ_snake");
    this.t.addFriendlyTag("elf");
    this.t.addFriendlyTag("nature_creature");
    this.clone("frog", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconFrog");
    this.t.addTag("sliceable");
    this.clone("piranha", "$TEMPLATE_ANIMAL$");
    this.t.setIcon("ui/Icons/iconPiranha");
    this.t.addTag("sliceable");
    this.t.addTag("small");
    this.clone("seal", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconSeal");
    this.t.addTag("sliceable");
    this.clone("flower_bud", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconFlowerBud");
    this.t.addTag("sliceable");
    this.clone("crystal_sword", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconCrystalSword");
    this.t.addEnemyTag("sliceable");
    this.clone("lemon_snail", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconLemonSnail");
    this.t.addTag("sliceable");
    this.t.addTag("small");
    this.clone("garl", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconGarl");
    this.t.addTag("garlic");
    this.clone("smore", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconSmore");
    this.t.addTag("sliceable");
    this.t.addTag("small");
    this.clone("acid_blob", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconAcidBlob");
    this.t.addEnemyTag("small");
    this.clone("unicorn", "$TEMPLATE_ANIMAL_PEACEFUL$");
    this.t.setIcon("ui/Icons/iconUnicorn");
    this.t.addEnemyTag("sliceable");
  }

  private void addCoolMinicivs()
  {
    this.cloneAsMiniciv("civ_aliens", "aliens", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("civ_druid", "druid");
    this.t.group_minicivs_cool = true;
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#85C32E");
    this.t.addTag("sliceable");
    this.cloneAsMiniciv("miniciv_angle", "angle", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_bandit", "bandit", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_cold_one", "cold_one", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_demon", "demon", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_evil_mage", "evil_mage", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_fire_skull", "fire_skull", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_jumpy_skull", "jumpy_skull", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_necromancer", "necromancer", true);
    this.t.group_minicivs_cool = true;
    this.t.addFriendlyTag("necromancer");
    this.t.addFriendlyTag("undead");
    this.cloneAsMiniciv("miniciv_plague_doctor", "plague_doctor", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_white_mage", "white_mage", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_greg", "greg");
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_fairy", "fairy", true);
    this.t.group_minicivs_cool = true;
    this.cloneAsMiniciv("miniciv_snowman", "snowman", true);
    this.t.group_minicivs_cool = true;
  }

  private void addAnimalMinicivs()
  {
    this.cloneAsMiniciv("miniciv_cat", "cat");
    this.cloneAsMiniciv("miniciv_dog", "dog");
    this.cloneAsMiniciv("miniciv_chicken", "chicken");
    this.cloneAsMiniciv("miniciv_rabbit", "rabbit");
    this.cloneAsMiniciv("miniciv_monkey", "monkey");
    this.cloneAsMiniciv("miniciv_fox", "fox");
    this.cloneAsMiniciv("miniciv_sheep", "sheep");
    this.cloneAsMiniciv("miniciv_cow", "cow");
    this.cloneAsMiniciv("miniciv_armadillo", "armadillo");
    this.cloneAsMiniciv("miniciv_raccoon", "raccoon");
    this.cloneAsMiniciv("miniciv_wolf", "wolf");
    this.cloneAsMiniciv("miniciv_bear", "bear");
    this.cloneAsMiniciv("miniciv_rhino", "rhino");
    this.cloneAsMiniciv("miniciv_buffalo", "buffalo");
    this.cloneAsMiniciv("miniciv_hyena", "hyena");
    this.cloneAsMiniciv("miniciv_rat", "rat");
    this.cloneAsMiniciv("miniciv_alpaca", "alpaca");
    this.cloneAsMiniciv("miniciv_capybara", "capybara");
    this.t.addFriendlyTag("everyone");
    this.cloneAsMiniciv("miniciv_goat", "goat");
    this.cloneAsMiniciv("miniciv_penguin", "penguin");
    this.cloneAsMiniciv("miniciv_ostrich", "ostrich");
    this.cloneAsMiniciv("miniciv_crab", "crab");
    this.t.addFriendlyTag("crabzilla");
    this.cloneAsMiniciv("miniciv_scorpion", "scorpion");
    this.cloneAsMiniciv("miniciv_turtle", "turtle");
    this.cloneAsMiniciv("miniciv_crocodile", "crocodile");
    this.cloneAsMiniciv("miniciv_snake", "snake");
    this.cloneAsMiniciv("miniciv_frog", "frog");
    this.cloneAsMiniciv("miniciv_piranha", "piranha");
    this.cloneAsMiniciv("miniciv_seal", "seal");
    this.cloneAsMiniciv("miniciv_flower_bud", "flower_bud");
    this.cloneAsMiniciv("miniciv_crystal_sword", "crystal_sword");
    this.cloneAsMiniciv("miniciv_lemon_snail", "lemon_snail");
    this.cloneAsMiniciv("miniciv_garl", "garl");
    this.cloneAsMiniciv("miniciv_smore", "smore");
    this.cloneAsMiniciv("miniciv_acid_blob", "acid_blob");
    this.cloneAsMiniciv("miniciv_insect", "insect");
    this.cloneAsMiniciv("miniciv_unicorn", "unicorn");
  }

  private void addCreeps()
  {
    this.clone("super_pumpkin", "$TEMPLATE_MOB$");
    this.t.setIcon("ui/Icons/iconSuperPumpkin");
    this.t.addTag("sliceable");
    this.t.group_creeps = true;
    this.t.addFriendlyTag("druid");
    this.clone("tumor", "$TEMPLATE_MOB$");
    this.t.group_creeps = true;
    this.t.setIcon("ui/Icons/iconTumor");
    this.clone("biomass", "$TEMPLATE_MOB$");
    this.t.group_creeps = true;
    this.t.setIcon("ui/Icons/iconBiomass");
    this.clone("assimilators", "$TEMPLATE_MOB$");
    this.t.group_creeps = true;
    this.t.setIcon("ui/Icons/iconAssimilator");
    this.t.addFriendlyTag("aliens");
  }

  private void addUnique()
  {
    KingdomAsset pAsset1 = new KingdomAsset();
    pAsset1.id = "godfinger";
    pAsset1.nature = true;
    pAsset1.count_as_danger = false;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconGodFinger");
    KingdomAsset pAsset2 = new KingdomAsset();
    pAsset2.id = "good";
    pAsset2.mobs = true;
    pAsset2.concept = true;
    pAsset2.count_as_danger = false;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/actor_traits/iconBlessing");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("civ");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("living_houses");
    this.t.addFriendlyTag("snowman");
    this.t.addEnemyTag("wolf");
    this.t.addEnemyTag("bear");
    this.t.addEnemyTag("orc");
    this.t.addEnemyTag("bandit");
    KingdomAsset pAsset3 = new KingdomAsset();
    pAsset3.id = "mad";
    pAsset3.always_attack_each_other = true;
    pAsset3.force_look_all_chunks = true;
    pAsset3.mobs = true;
    pAsset3.units_always_looking_for_enemies = true;
    pAsset3.is_forced_by_trait = true;
    pAsset3.forced_by_trait_kingdom_id = "madness";
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/actor_traits/iconMadness");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#E53B3B");
    KingdomAsset pAsset4 = new KingdomAsset();
    pAsset4.id = "alien_mold";
    pAsset4.force_look_all_chunks = true;
    pAsset4.mobs = true;
    pAsset4.units_always_looking_for_enemies = true;
    pAsset4.is_forced_by_trait = true;
    pAsset4.forced_by_trait_kingdom_id = "desire_alien_mold";
    pAsset4.building_attractor_id = "waypoint_alien_mold";
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconWaypointAlienMold");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#C342FF");
    this.t.addFriendlyTag("aliens");
    this.t.addFriendlyTag("civ_aliens");
    KingdomAsset pAsset5 = new KingdomAsset();
    pAsset5.id = "computer";
    pAsset5.force_look_all_chunks = true;
    pAsset5.mobs = true;
    pAsset5.units_always_looking_for_enemies = true;
    pAsset5.is_forced_by_trait = true;
    pAsset5.forced_by_trait_kingdom_id = "desire_computer";
    pAsset5.building_attractor_id = "waypoint_computer";
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/iconWaypointComputer");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#5DCE2D");
    this.t.addFriendlyTag("assimilators");
    KingdomAsset pAsset6 = new KingdomAsset();
    pAsset6.id = "golden_egg";
    pAsset6.force_look_all_chunks = true;
    pAsset6.mobs = true;
    pAsset6.units_always_looking_for_enemies = true;
    pAsset6.is_forced_by_trait = true;
    pAsset6.forced_by_trait_kingdom_id = "desire_golden_egg";
    pAsset6.building_attractor_id = "waypoint_golden_egg";
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/iconWaypointGoldenEgg");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#FFEC77");
    this.t.addFriendlyTag("chicken");
    this.t.addFriendlyTag("civ_chicken");
    this.t.addFriendlyTag("miniciv_chicken");
    this.t.addFriendlyTag("sheep");
    this.t.addFriendlyTag("civ_sheep");
    this.t.addFriendlyTag("miniciv_sheep");
    KingdomAsset pAsset7 = new KingdomAsset();
    pAsset7.id = "harp";
    pAsset7.force_look_all_chunks = true;
    pAsset7.mobs = true;
    pAsset7.units_always_looking_for_enemies = true;
    pAsset7.is_forced_by_trait = true;
    pAsset7.forced_by_trait_kingdom_id = "desire_harp";
    pAsset7.building_attractor_id = "waypoint_harp";
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/iconWaypointHarp");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#FF60E9");
    this.t.addFriendlyTag("crystal_sword");
    this.t.addFriendlyTag("civ_crystal_golem");
    this.t.addFriendlyTag("miniciv_crystal_sword");
    KingdomAsset pAsset8 = new KingdomAsset();
    pAsset8.id = "possessed";
    pAsset8.force_look_all_chunks = true;
    this.add(pAsset8);
    this.t.setIcon("ui/Icons/iconPossessed2");
    this.t.addEnemyTag("nature");
    this.t.addEnemyTag("ruins");
    this.t.addEnemyTag("abandoned");
    KingdomAsset pAsset9 = new KingdomAsset();
    pAsset9.id = "crabzilla";
    pAsset9.mobs = true;
    this.add(pAsset9);
    this.t.setIcon("ui/Icons/iconCrabzilla");
    this.t.addTag("crab");
    this.t.addFriendlyTag("crab");
    this.t.addFriendlyTag("civ_crab");
    this.t.addFriendlyTag("miniciv_crab");
    KingdomAsset pAsset10 = new KingdomAsset();
    pAsset10.id = "ants";
    pAsset10.mobs = true;
    this.add(pAsset10);
    this.t.setIcon("ui/Icons/iconAntRed");
    this.t.addTag("nature_creature");
    this.t.addFriendlyTag("good");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("living_houses");
    KingdomAsset pAsset11 = new KingdomAsset();
    pAsset11.id = "golden_brain";
    pAsset11.mobs = true;
    pAsset11.brain = true;
    pAsset11.count_as_danger = false;
    this.add(pAsset11);
    this.t.setIcon("ui/Icons/iconGoldBrain");
    this.t.addTag("neutral");
    this.t.addFriendlyTag("orc");
    this.t.addFriendlyTag("bandit");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("civ");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("living_houses");
    this.t.addFriendlyTag("snowman");
    KingdomAsset pAsset12 = new KingdomAsset();
    pAsset12.id = "corrupted_brain";
    pAsset12.mobs = true;
    pAsset12.brain = true;
    this.add(pAsset12);
    this.t.setIcon("ui/Icons/iconCorruptedBrain");
    this.t.addTag("mad");
  }

  private void addNeutral()
  {
    KingdomAsset pAsset1 = new KingdomAsset();
    pAsset1.id = "neutral";
    pAsset1.civ = true;
    pAsset1.neutral = true;
    pAsset1.default_civ_color_index = 83;
    pAsset1.count_as_danger = false;
    pAsset1.concept = true;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/worldrules/icon_random_seeds");
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#AAAAAA");
    this.t.addTag("nature_creature");
    this.t.addTag("neutral");
    this.t.addFriendlyTag("good");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("neutral");
    KingdomAsset pAsset2 = new KingdomAsset();
    pAsset2.id = "neutral_animals";
    pAsset2.mobs = true;
    pAsset2.count_as_danger = false;
    pAsset2.concept = true;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/worldrules/icon_animalspawn");
    this.t.addTag("neutral");
    this.t.addTag("nature_creature");
    this.t.addFriendlyTag("good");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("nature_creature");
    this.t.addFriendlyTag("living_houses");
    this.t.addFriendlyTag("snowman");
    this.t.addFriendlyTag("civ");
    this.clone("insect", "neutral_animals");
    this.t.setIcon("ui/Icons/iconBeetle");
    this.t.concept = true;
    this.clone("fly", "insect");
    this.t.setIcon("ui/Icons/iconFly");
    KingdomAsset pAsset3 = new KingdomAsset();
    pAsset3.id = "nature";
    pAsset3.nature = true;
    pAsset3.mobs = true;
    pAsset3.count_as_danger = false;
    pAsset3.concept = true;
    this.add(pAsset3);
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#888888");
    this.t.setIcon("ui/Icons/world generation/icon_randomBiomes");
    KingdomAsset pAsset4 = new KingdomAsset();
    pAsset4.id = "ruins";
    pAsset4.nature = true;
    pAsset4.mobs = true;
    pAsset4.count_as_danger = false;
    pAsset4.concept = true;
    this.add(pAsset4);
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#444444");
    this.t.setIcon("ui/Icons/iconCityDestroyed");
    this.t.color_building = Toolbox.color_white;
    KingdomAsset pAsset5 = new KingdomAsset();
    pAsset5.id = "abandoned";
    pAsset5.nature = true;
    pAsset5.mobs = true;
    pAsset5.abandoned = true;
    pAsset5.count_as_danger = false;
    pAsset5.concept = true;
    this.add(pAsset5);
    this.t.default_kingdom_color = ColorAsset.tryMakeNewColorAsset("#888888");
    this.t.setIcon("ui/Icons/iconKingdomDestroyed");
    this.t.color_building = Toolbox.color_abandoned_building;
  }

  public override void post_init()
  {
    // ISSUE: unable to decompile the method.
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (KingdomAsset pAsset in this.list)
      this.finish(pAsset);
  }

  private void finish(KingdomAsset pAsset)
  {
  }

  public override void editorDiagnostic()
  {
    base.editorDiagnostic();
    this.generateDebugReportFile();
    foreach (KingdomAsset kingdomAsset in this.list)
    {
      if ((kingdomAsset.civ || kingdomAsset.mobs) && !kingdomAsset.concept && !kingdomAsset.nomads && !kingdomAsset.nature && !kingdomAsset.neutral && !kingdomAsset.brain && !kingdomAsset.is_forced_by_trait)
      {
        bool flag = false;
        foreach (ActorAsset actorAsset in AssetManager.actor_library.list)
        {
          if (kingdomAsset.civ && actorAsset.kingdom_id_civilization == kingdomAsset.id)
          {
            flag = true;
            break;
          }
          if (kingdomAsset.mobs && actorAsset.kingdom_id_wild == kingdomAsset.id)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          if (kingdomAsset.civ)
            BaseAssetLibrary.logAssetError("<b>KingdomLibrary</b>: <e>Civ Kingdom</e> is not used by any <e>kingdom_id_civilization</e>", kingdomAsset.id);
          else
            BaseAssetLibrary.logAssetError("<b>KingdomLibrary</b>: <e>Mob Kingdom</e> is not used by any <e>kingdom_id_wild</e>", kingdomAsset.id);
        }
      }
    }
  }

  public void checkForMissingTags()
  {
    for (int index1 = 0; index1 < this.list.Count - 1; ++index1)
    {
      KingdomAsset pTarget1 = this.list[index1];
      for (int index2 = index1 + 1; index2 < this.list.Count; ++index2)
      {
        KingdomAsset pTarget2 = this.list[index2];
        if (pTarget1 != pTarget2 && pTarget1.isFoe(pTarget2) != pTarget2.isFoe(pTarget1))
        {
          KingdomAsset kingdomAsset1 = pTarget1;
          if (kingdomAsset1.assets_discrepancies == null)
            kingdomAsset1.assets_discrepancies = new HashSet<string>();
          KingdomAsset kingdomAsset2 = pTarget2;
          if (kingdomAsset2.assets_discrepancies == null)
            kingdomAsset2.assets_discrepancies = new HashSet<string>();
          pTarget1.assets_discrepancies.Add(pTarget2.id);
          pTarget2.assets_discrepancies.Add(pTarget1.id);
          if (pTarget2.id.Contains(pTarget1.id) || pTarget1.id.Contains(pTarget2.id))
          {
            KingdomAsset kingdomAsset3 = pTarget1;
            if (kingdomAsset3.assets_discrepancies_bad == null)
              kingdomAsset3.assets_discrepancies_bad = new HashSet<string>();
            KingdomAsset kingdomAsset4 = pTarget2;
            if (kingdomAsset4.assets_discrepancies_bad == null)
              kingdomAsset4.assets_discrepancies_bad = new HashSet<string>();
            pTarget1.assets_discrepancies_bad.Add(pTarget2.id);
            pTarget2.assets_discrepancies_bad.Add(pTarget1.id);
          }
        }
      }
    }
  }

  public void generateDebugReportFile()
  {
    // ISSUE: unable to decompile the method.
  }

  public void cloneAsMiniciv(string pNew, string pFrom, bool pMakeLoveToNeutrals = false)
  {
    this.clone(pNew, pFrom);
    this.t.group_miniciv = true;
    this.t.mobs = false;
    this.t.civ = true;
    this.t.addTag(pFrom);
    this.t.addFriendlyTag(pFrom);
    this.get(pFrom).addFriendlyTag(pNew);
    if (!pMakeLoveToNeutrals)
      return;
    this.t.addTag("civ");
    this.t.addFriendlyTag("neutral");
    this.t.addFriendlyTag("nature_creature");
  }

  public override KingdomAsset clone(string pNew, string pFrom)
  {
    base.clone(pNew, pFrom);
    this.t.concept = false;
    return this.t;
  }
}
