// Decompiled with JetBrains decompiler
// Type: WorldLogLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldLogLibrary : AssetLibrary<WorldLogAsset>
{
  public static WorldLogAsset king_new;
  public static WorldLogAsset king_left;
  public static WorldLogAsset king_fled_city;
  public static WorldLogAsset king_fled_capital;
  public static WorldLogAsset king_dead;
  public static WorldLogAsset king_killed;
  public static WorldLogAsset favorite_dead;
  public static WorldLogAsset favorite_killed;
  public static WorldLogAsset city_new;
  public static WorldLogAsset log_city_revolted;
  public static WorldLogAsset diplomacy_war_ended;
  public static WorldLogAsset diplomacy_war_started;
  public static WorldLogAsset total_war_started;
  public static WorldLogAsset alliance_new;
  public static WorldLogAsset alliance_dissolved;
  public static WorldLogAsset kingdom_new;
  public static WorldLogAsset kingdom_destroyed;
  public static WorldLogAsset kingdom_shattered;
  public static WorldLogAsset kingdom_fractured;
  public static WorldLogAsset city_destroyed;
  public static WorldLogAsset kingdom_royal_clan_new;
  public static WorldLogAsset kingdom_royal_clan_changed;
  public static WorldLogAsset kingdom_royal_clan_dead;
  public static WorldLogAsset auto_tester;

  private void updateText(ref string pText, WorldLogMessage pMessage, string pKey, int pSpecialId)
  {
    string special = pMessage.getSpecial(pSpecialId);
    pText = pText.Replace(pKey, special);
  }

  public override void init()
  {
    base.init();
    WorldLogAsset pAsset1 = new WorldLogAsset();
    pAsset1.id = "king_new";
    pAsset1.group = "kings";
    pAsset1.path_icon = "ui/Icons/iconKings";
    pAsset1.color = Toolbox.color_log_neutral;
    pAsset1.text_replacer = new WorldLogTextFormatter(this.kingReplacer);
    WorldLogLibrary.king_new = this.add(pAsset1);
    WorldLogAsset pAsset2 = new WorldLogAsset();
    pAsset2.id = "king_left";
    pAsset2.group = "kings";
    pAsset2.path_icon = "ui/Icons/actor_traits/iconStupid";
    pAsset2.color = Toolbox.color_log_warning;
    pAsset2.text_replacer = new WorldLogTextFormatter(this.kingReplacer);
    WorldLogLibrary.king_left = this.add(pAsset2);
    WorldLogAsset pAsset3 = new WorldLogAsset();
    pAsset3.id = "king_fled_capital";
    pAsset3.group = "kings";
    pAsset3.random_ids = 3;
    pAsset3.path_icon = "ui/Icons/actor_traits/iconStupid";
    pAsset3.color = Toolbox.color_log_warning;
    pAsset3.text_replacer = new WorldLogTextFormatter(this.kingCityReplacer);
    WorldLogLibrary.king_fled_capital = this.add(pAsset3);
    WorldLogAsset pAsset4 = new WorldLogAsset();
    pAsset4.id = "king_fled_city";
    pAsset4.group = "kings";
    pAsset4.random_ids = 3;
    pAsset4.path_icon = "ui/Icons/actor_traits/iconStupid";
    pAsset4.color = Toolbox.color_log_warning;
    pAsset4.text_replacer = new WorldLogTextFormatter(this.kingCityReplacer);
    WorldLogLibrary.king_fled_city = this.add(pAsset4);
    WorldLogAsset pAsset5 = new WorldLogAsset();
    pAsset5.id = "king_dead";
    pAsset5.group = "kings";
    pAsset5.path_icon = "ui/Icons/iconDead";
    pAsset5.color = Toolbox.color_log_warning;
    pAsset5.text_replacer = new WorldLogTextFormatter(this.kingReplacer);
    WorldLogLibrary.king_dead = this.add(pAsset5);
    WorldLogAsset pAsset6 = new WorldLogAsset();
    pAsset6.id = "king_killed";
    pAsset6.group = "kings";
    pAsset6.random_ids = 3;
    pAsset6.path_icon = "ui/Icons/actor_traits/iconKingslayer";
    pAsset6.color = Toolbox.color_log_warning;
    pAsset6.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) =>
    {
      this.updateText(ref pText, pMessage, "$kingdom$", 1);
      this.updateText(ref pText, pMessage, "$king$", 2);
      this.updateText(ref pText, pMessage, "$name$", 3);
    });
    WorldLogLibrary.king_killed = this.add(pAsset6);
    WorldLogAsset pAsset7 = new WorldLogAsset();
    pAsset7.id = "favorite_dead";
    pAsset7.group = "favorite_units";
    pAsset7.random_ids = 3;
    pAsset7.path_icon = "ui/Icons/iconFavoriteKilled";
    pAsset7.color = Toolbox.color_log_warning;
    pAsset7.text_replacer = new WorldLogTextFormatter(this.nameReplacer);
    WorldLogLibrary.favorite_dead = this.add(pAsset7);
    WorldLogAsset pAsset8 = new WorldLogAsset();
    pAsset8.id = "favorite_killed";
    pAsset8.group = "favorite_units";
    pAsset8.random_ids = 3;
    pAsset8.path_icon = "ui/Icons/iconFavoriteKilled";
    pAsset8.color = Toolbox.color_log_warning;
    pAsset8.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) =>
    {
      this.updateText(ref pText, pMessage, "$name$", 1);
      this.updateText(ref pText, pMessage, "$killer$", 2);
    });
    WorldLogLibrary.favorite_killed = this.add(pAsset8);
    WorldLogAsset pAsset9 = new WorldLogAsset();
    pAsset9.id = "city_new";
    pAsset9.group = "cities";
    pAsset9.path_icon = "ui/Icons/iconCitySelect";
    pAsset9.color = Toolbox.color_log_neutral;
    pAsset9.text_replacer = new WorldLogTextFormatter(this.nameReplacer);
    WorldLogLibrary.city_new = this.add(pAsset9);
    WorldLogAsset pAsset10 = new WorldLogAsset();
    pAsset10.id = "log_city_revolted";
    pAsset10.group = "cities";
    pAsset10.path_icon = "ui/Icons/iconInspiration";
    pAsset10.color = Toolbox.color_log_good;
    pAsset10.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) =>
    {
      this.updateText(ref pText, pMessage, "$name$", 1);
      this.updateText(ref pText, pMessage, "$kingdom$", 2);
    });
    WorldLogLibrary.log_city_revolted = this.add(pAsset10);
    WorldLogAsset pAsset11 = new WorldLogAsset();
    pAsset11.id = "diplomacy_war_ended";
    pAsset11.group = "wars";
    pAsset11.path_icon = "ui/Icons/actor_traits/iconPacifist";
    pAsset11.color = Toolbox.color_log_good;
    pAsset11.text_replacer = new WorldLogTextFormatter(this.nameReplacer);
    WorldLogLibrary.diplomacy_war_ended = this.add(pAsset11);
    WorldLogAsset pAsset12 = new WorldLogAsset();
    pAsset12.id = "diplomacy_war_started";
    pAsset12.group = "wars";
    pAsset12.path_icon = "ui/Icons/iconWar";
    pAsset12.color = Toolbox.color_log_warning;
    pAsset12.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) =>
    {
      this.updateText(ref pText, pMessage, "$kingdom1$", 1);
      this.updateText(ref pText, pMessage, "$kingdom2$", 2);
    });
    WorldLogLibrary.diplomacy_war_started = this.add(pAsset12);
    WorldLogAsset pAsset13 = new WorldLogAsset();
    pAsset13.id = "total_war_started";
    pAsset13.group = "wars";
    pAsset13.path_icon = "ui/Icons/iconWar";
    pAsset13.color = Toolbox.color_log_warning;
    pAsset13.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) => this.updateText(ref pText, pMessage, "$kingdom$", 1));
    WorldLogLibrary.total_war_started = this.add(pAsset13);
    WorldLogAsset pAsset14 = new WorldLogAsset();
    pAsset14.id = "alliance_new";
    pAsset14.path_icon = "ui/Icons/iconAlliance";
    pAsset14.color = Toolbox.color_log_neutral;
    pAsset14.text_replacer = new WorldLogTextFormatter(this.nameReplacer);
    WorldLogLibrary.alliance_new = this.add(pAsset14);
    WorldLogAsset pAsset15 = new WorldLogAsset();
    pAsset15.id = "alliance_dissolved";
    pAsset15.path_icon = "ui/Icons/iconAlliance";
    pAsset15.color = Toolbox.color_log_warning;
    pAsset15.text_replacer = new WorldLogTextFormatter(this.nameReplacer);
    WorldLogLibrary.alliance_dissolved = this.add(pAsset15);
    WorldLogAsset pAsset16 = new WorldLogAsset();
    pAsset16.id = "kingdom_new";
    pAsset16.group = "kingdoms";
    pAsset16.path_icon = "ui/Icons/iconKingdom";
    pAsset16.color = Toolbox.color_log_neutral;
    pAsset16.text_replacer = new WorldLogTextFormatter(this.nameReplacer);
    WorldLogLibrary.kingdom_new = this.add(pAsset16);
    WorldLogAsset pAsset17 = new WorldLogAsset();
    pAsset17.id = "kingdom_destroyed";
    pAsset17.group = "kingdoms";
    pAsset17.path_icon = "ui/Icons/actor_traits/iconPyromaniac";
    pAsset17.color = Toolbox.color_log_warning;
    pAsset17.text_replacer = new WorldLogTextFormatter(this.nameReplacer);
    WorldLogLibrary.kingdom_destroyed = this.add(pAsset17);
    WorldLogAsset pAsset18 = new WorldLogAsset();
    pAsset18.id = "kingdom_shattered";
    pAsset18.group = "kingdoms";
    pAsset18.path_icon = "ui/Icons/actor_traits/iconPyromaniac";
    pAsset18.random_ids = 3;
    pAsset18.color = Toolbox.color_log_warning;
    pAsset18.text_replacer = new WorldLogTextFormatter(this.kingdomReplacer);
    WorldLogLibrary.kingdom_shattered = this.add(pAsset18);
    WorldLogAsset pAsset19 = new WorldLogAsset();
    pAsset19.id = "kingdom_fractured";
    pAsset19.group = "kingdoms";
    pAsset19.path_icon = "ui/Icons/actor_traits/iconPyromaniac";
    pAsset19.random_ids = 3;
    pAsset19.color = Toolbox.color_log_warning;
    pAsset19.text_replacer = new WorldLogTextFormatter(this.kingdomReplacer);
    WorldLogLibrary.kingdom_fractured = this.add(pAsset19);
    WorldLogAsset pAsset20 = new WorldLogAsset();
    pAsset20.id = "kingdom_royal_clan_new";
    pAsset20.group = "clans";
    pAsset20.path_icon = "ui/Icons/iconClan";
    pAsset20.color = Toolbox.color_log_neutral;
    pAsset20.random_ids = 3;
    pAsset20.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) =>
    {
      this.updateText(ref pText, pMessage, "$kingdom$", 1);
      this.updateText(ref pText, pMessage, "$clan$", 2);
      this.updateText(ref pText, pMessage, "$king$", 3);
    });
    WorldLogLibrary.kingdom_royal_clan_new = this.add(pAsset20);
    WorldLogAsset pAsset21 = new WorldLogAsset();
    pAsset21.id = "kingdom_royal_clan_changed";
    pAsset21.group = "clans";
    pAsset21.path_icon = "ui/Icons/iconClan";
    pAsset21.color = Toolbox.color_log_neutral;
    pAsset21.random_ids = 3;
    pAsset21.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) =>
    {
      this.updateText(ref pText, pMessage, "$kingdom$", 1);
      this.updateText(ref pText, pMessage, "$old_clan$", 2);
      this.updateText(ref pText, pMessage, "$new_clan$", 3);
    });
    WorldLogLibrary.kingdom_royal_clan_changed = this.add(pAsset21);
    WorldLogAsset pAsset22 = new WorldLogAsset();
    pAsset22.id = "kingdom_royal_clan_dead";
    pAsset22.group = "clans";
    pAsset22.path_icon = "ui/Icons/iconClan";
    pAsset22.color = Toolbox.color_log_warning;
    pAsset22.random_ids = 3;
    pAsset22.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) =>
    {
      this.updateText(ref pText, pMessage, "$kingdom$", 1);
      this.updateText(ref pText, pMessage, "$clan$", 2);
    });
    WorldLogLibrary.kingdom_royal_clan_dead = this.add(pAsset22);
    WorldLogAsset pAsset23 = new WorldLogAsset();
    pAsset23.id = "city_destroyed";
    pAsset23.group = "cities";
    pAsset23.path_icon = "ui/Icons/actor_traits/iconPyromaniac";
    pAsset23.color = Toolbox.color_log_warning;
    pAsset23.text_replacer = new WorldLogTextFormatter(this.nameReplacer);
    WorldLogLibrary.city_destroyed = this.add(pAsset23);
    WorldLogAsset pAsset24 = new WorldLogAsset();
    pAsset24.id = "auto_tester";
    pAsset24.path_icon = "ui/Icons/iconPlay";
    pAsset24.color = Toolbox.color_log_warning;
    pAsset24.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) => pText = pMessage.special1);
    WorldLogLibrary.auto_tester = this.add(pAsset24);
    this.addDisasters();
  }

  private void addDisasters()
  {
    WorldLogAsset pAsset = new WorldLogAsset();
    pAsset.id = "$basic_disaster$";
    pAsset.color = Toolbox.color_log_warning;
    pAsset.group = "disasters";
    this.add(pAsset);
    this.clone("disaster_tornado", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_tornado";
    this.t.path_icon = "ui/Icons/iconTornado";
    this.clone("disaster_meteorite", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_meteorite";
    this.t.path_icon = "ui/Icons/iconMeteorite";
    this.clone("disaster_hellspawn", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_hellspawn";
    this.t.path_icon = "ui/Icons/iconDemon";
    this.clone("disaster_earthquake", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_earthquake";
    this.t.path_icon = "ui/Icons/iconEarthquake";
    this.clone("disaster_greg_abominations", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_greg_abominations";
    this.t.path_icon = "ui/Icons/iconGreg";
    this.clone("disaster_ice_ones", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_ice_ones";
    this.t.path_icon = "ui/Icons/iconWalker";
    this.clone("disaster_sudden_snowman", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_sudden_snowman";
    this.t.path_icon = "ui/Icons/iconSnowMan";
    this.clone("disaster_garden_surprise", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_garden_surprise";
    this.t.path_icon = "ui/Icons/iconSuperPumpkin";
    this.clone("disaster_dragon_from_farlands", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_dragon_from_farlands";
    this.t.path_icon = "ui/Icons/iconDragon";
    this.clone("disaster_bandits", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_bandits";
    this.t.path_icon = "ui/Icons/iconBandit";
    this.clone("disaster_alien_invasion", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_alien_invasion";
    this.t.path_icon = "ui/Icons/iconUfo";
    this.clone("disaster_biomass", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_biomass";
    this.t.path_icon = "ui/Icons/iconBiomass";
    this.clone("disaster_tumor", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_tumor";
    this.t.path_icon = "ui/Icons/iconTumor";
    this.clone("disaster_heatwave", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_heatwave";
    this.t.path_icon = "ui/Icons/iconFire";
    this.clone("disaster_evil_mage", "$basic_disaster$");
    this.t.locale_id = "worldlog_disaster_evil_mage";
    this.t.path_icon = "ui/Icons/iconEvilMage";
    this.t.text_replacer = new WorldLogTextFormatter(this.nameReplacer);
    this.clone("$city_name_disaster$", "$basic_disaster$");
    this.t.text_replacer = (WorldLogTextFormatter) ((WorldLogMessage pMessage, ref string pText) =>
    {
      this.updateText(ref pText, pMessage, "$name$", 1);
      this.updateText(ref pText, pMessage, "$city$", 2);
    });
    this.clone("disaster_underground_necromancer", "$city_name_disaster$");
    this.t.locale_id = "worldlog_disaster_underground_necromancer";
    this.t.path_icon = "ui/Icons/iconNecromancer";
    this.clone("disaster_mad_thoughts", "$city_name_disaster$");
    this.t.locale_id = "worldlog_disaster_mad_thoughts";
    this.t.path_icon = "ui/Icons/actor_traits/iconMadness";
  }

  private void nameReplacer(WorldLogMessage pMessage, ref string pText)
  {
    this.updateText(ref pText, pMessage, "$name$", 1);
  }

  private void kingReplacer(WorldLogMessage pMessage, ref string pText)
  {
    this.updateText(ref pText, pMessage, "$kingdom$", 1);
    this.updateText(ref pText, pMessage, "$king$", 2);
  }

  private void kingCityReplacer(WorldLogMessage pMessage, ref string pText)
  {
    this.updateText(ref pText, pMessage, "$kingdom$", 1);
    this.updateText(ref pText, pMessage, "$king$", 2);
    this.updateText(ref pText, pMessage, "$city$", 3);
  }

  private void kingdomReplacer(WorldLogMessage pMessage, ref string pText)
  {
    this.updateText(ref pText, pMessage, "$kingdom$", 1);
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (WorldLogAsset pAsset in this.list)
    {
      foreach (string localeId in pAsset.getLocaleIDs())
        this.checkLocale((Asset) pAsset, localeId);
    }
  }
}
