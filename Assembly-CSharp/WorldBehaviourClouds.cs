// Decompiled with JetBrains decompiler
// Type: WorldBehaviourClouds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldBehaviourClouds
{
  public static void spawnRandomCloud()
  {
    if (World.world_era.clouds == null || World.world_era.clouds.Count == 0)
      return;
    string random = World.world_era.clouds.GetRandom<string>();
    CloudAsset cloudAsset = AssetManager.clouds.get(random);
    if (cloudAsset == null || cloudAsset.normal_cloud && !WorldLawLibrary.world_law_clouds.isEnabled() || cloudAsset.considered_disaster && !WorldLawLibrary.world_law_disasters_nature.isEnabled())
      return;
    EffectsLibrary.spawn("fx_cloud", pParam1: cloudAsset.id);
  }

  public static void setEra(WorldAgeAsset pAsset)
  {
    WorldBehaviourAsset worldBehaviourAsset = AssetManager.world_behaviours.get("clouds");
    worldBehaviourAsset.interval = pAsset.cloud_interval;
    worldBehaviourAsset.interval_random = pAsset.cloud_interval;
  }
}
