// Decompiled with JetBrains decompiler
// Type: CursedSacrifice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class CursedSacrifice
{
  private const int SACRIFICE_COUNT = 314;
  private const int MAX_MESSAGES = 9;
  private static int _last_message_index = -1;
  private static int _current_sacrifice_count = 0;
  private static double _cursed_world_timestamp = 0.0;
  private static bool _latest_sacrificed_was_egg;

  public static void checkGoodForSacrifice(Actor pActor)
  {
    bool flag1 = false;
    if (pActor.hasSubspecies())
    {
      bool flag2 = pActor.hasStatus("magnetized") || pActor.hasStatus("strange_urge") || pActor.hasStatus("possessed");
      if (pActor.hasSubspeciesTrait("pure") & flag2)
        flag1 = true;
    }
    if (!flag1)
      return;
    if (pActor.asset.id == "elf")
    {
      ++World.world.game_stats.data.elvesSacrificed;
      CursedSacrifice._latest_sacrificed_was_egg = pActor.isEgg();
      CursedSacrifice.spawnVoidElves();
    }
    ++World.world.game_stats.data.creaturesSacrificed;
    CursedSacrifice.countSacrifice();
  }

  public static void spawnVoidElves()
  {
    Subspecies voidElvesSubspecies = CursedSacrifice.getVoidElvesSubspecies();
    if (voidElvesSubspecies == null)
      return;
    TileZone random = World.world.zone_camera.getVisibleZones().GetRandom<TileZone>();
    if (random == null)
      return;
    World.world.units.spawnNewUnit("elf", random.getRandomTile(), pMiracleSpawn: true, pSubspecies: voidElvesSubspecies, pGiveOwnerlessItems: true, pAdultAge: true);
  }

  private static Subspecies getVoidElvesSubspecies()
  {
    using (ListPool<Subspecies> list = new ListPool<Subspecies>())
    {
      ActorAsset pAsset = AssetManager.actor_library.get("elf");
      foreach (Subspecies subspecies in (CoreSystemManager<Subspecies, SubspeciesData>) World.world.subspecies)
      {
        if (subspecies.getActorAsset() == pAsset && subspecies.hasTrait("mutation_skin_void"))
          list.Add(subspecies);
      }
      Subspecies voidElvesSubspecies;
      if (list.Count == 0)
      {
        WorldTile randomGround = World.world.islands_calculator.tryGetRandomGround();
        if (randomGround == null)
          return (Subspecies) null;
        Subspecies subspecies = World.world.subspecies.newSpecies(pAsset, randomGround);
        subspecies.addTrait("mutation_skin_void");
        subspecies.addTrait("gift_of_void");
        subspecies.addTrait("reproduction_soulborne");
        subspecies.addTrait("big_stomach");
        subspecies.addTrait("voracious");
        subspecies.addTrait("genetic_mirror");
        subspecies.addTrait("genetic_psychosis");
        subspecies.addTrait("enhanced_strength");
        subspecies.addTrait("cold_resistance");
        subspecies.addTrait("heat_resistance");
        subspecies.addTrait("adaptation_corruption");
        subspecies.addTrait("adaptation_desert");
        subspecies.addTrait("hovering");
        subspecies.removeTrait("pure");
        subspecies.removeTrait("prefrontal_cortex");
        subspecies.removeTrait("advanced_hippocampus");
        subspecies.removeTrait("amygdala");
        subspecies.removeTrait("wernicke_area");
        subspecies.addBirthTrait("desire_harp");
        subspecies.addBirthTrait("evil");
        subspecies.data.name = "Elfus Voidus";
        voidElvesSubspecies = subspecies;
      }
      else
        voidElvesSubspecies = list.GetRandom<Subspecies>();
      return voidElvesSubspecies;
    }
  }

  public static void countAllSacrificesDebug()
  {
    for (int index = 0; index < 314; ++index)
      CursedSacrifice.countSacrifice();
  }

  private static void countSacrifice()
  {
    if (CursedSacrifice._current_sacrifice_count == 314)
      return;
    ++CursedSacrifice._current_sacrifice_count;
    int num = (int) ((double) CursedSacrifice.getCurseProgressRatio() * 9.0);
    if (num <= CursedSacrifice._last_message_index)
      return;
    CursedSacrifice._last_message_index = num;
    string pColor = "#F3961F";
    if (CursedSacrifice._last_message_index > 6)
      pColor = "#FF637D";
    if (CursedSacrifice._last_message_index == 9)
      pColor = "#E060CD";
    WorldTip.showNow("world_curse_message_" + CursedSacrifice._last_message_index.ToString(), pPosition: "top", pColor: pColor);
    int lastMessageIndex = CursedSacrifice._last_message_index;
    World.world.startShake((float) (0.30000001192092896 + (double) CursedSacrifice._last_message_index * 0.10000000149011612), pIntensity: (float) (0.23000000417232513 + (double) CursedSacrifice._last_message_index * 0.019999999552965164), pShakeX: true);
  }

  public static float getCurseProgressRatio()
  {
    return (float) CursedSacrifice._current_sacrifice_count / 314f;
  }

  public static float getCurseProgressRatioForBlackhole()
  {
    return AchievementLibrary.isUnlocked("achievementCursedWorld") ? 1f : (float) CursedSacrifice._current_sacrifice_count / 314f;
  }

  public static void reset()
  {
    CursedSacrifice._current_sacrifice_count = 0;
    CursedSacrifice._last_message_index = 0;
    CursedSacrifice._latest_sacrificed_was_egg = false;
  }

  private static int getCurrentSacrificeCount() => CursedSacrifice._current_sacrifice_count;

  public static void loadAlreadyCursedState() => CursedSacrifice._current_sacrifice_count = 314;

  public static bool isWorldReadyForCURSE()
  {
    return AchievementLibrary.isUnlocked("achievementCursedWorld") || CursedSacrifice.isAllSacrificesDone();
  }

  public static bool isAllSacrificesDone() => CursedSacrifice.getCurrentSacrificeCount() >= 314;

  public static void justCursedWorld()
  {
    if (!Config.hasPremium)
      return;
    CursedSacrifice._cursed_world_timestamp = World.world.getCurSessionTime();
    AchievementLibrary.cursed_world.check();
  }

  public static bool justGotCursedWorld()
  {
    return (double) World.world.getRealTimeElapsedSince(CursedSacrifice._cursed_world_timestamp) < 1.0;
  }

  public static bool isLatestWasEgg() => CursedSacrifice._latest_sacrificed_was_egg;
}
