// Decompiled with JetBrains decompiler
// Type: FavoriteWorld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class FavoriteWorld
{
  private const int NO_WORLD_SET = -1;
  private static int _cache_favorite_world_id = -1;

  public static void checkFavoriteWorld()
  {
    if (!FavoriteWorld.hasFavoriteWorldSet(false) || SaveManager.slotExists(PlayerConfig.instance.data.favorite_world))
      return;
    FavoriteWorld.clearFavoriteWorld();
  }

  public static void clearFavoriteWorld()
  {
    PlayerConfig.instance.data.favorite_world = -1;
    PlayerConfig.saveData();
  }

  public static bool hasFavoriteWorldSet(bool pCheck = true)
  {
    if (pCheck)
      FavoriteWorld.checkFavoriteWorld();
    return PlayerConfig.instance.data.favorite_world != -1;
  }

  public static void restoreCachedFavoriteWorldOnSuccess()
  {
    if (FavoriteWorld._cache_favorite_world_id == -1)
      return;
    PlayerConfig.instance.data.favorite_world = FavoriteWorld._cache_favorite_world_id;
    PlayerConfig.saveData();
    FavoriteWorld._cache_favorite_world_id = -1;
  }

  public static void cacheSaveSlotID(int pID) => FavoriteWorld._cache_favorite_world_id = pID;
}
