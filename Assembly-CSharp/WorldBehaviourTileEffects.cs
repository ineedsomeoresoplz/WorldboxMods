// Decompiled with JetBrains decompiler
// Type: WorldBehaviourTileEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldBehaviourTileEffects
{
  public static void tryToStartTileEffects()
  {
    for (int index = 0; index < 5; ++index)
      WorldBehaviourTileEffects.spawnEffect();
  }

  public static void spawnEffect()
  {
    if (TrailerMonolith.enable_trailer_stuff || !World.world.zone_camera.hasVisibleZones() || World.world.stack_effects.controller_tile_effects.isLimitReached())
      return;
    WorldTile randomTile = World.world.zone_camera.getVisibleZones().GetRandom<TileZone>().getRandomTile();
    TileEffectAsset randomEffect = TileEffectsLibrary.getRandomEffect(randomTile);
    if (randomEffect == null || !Randy.randomChance(randomEffect.chance))
      return;
    foreach (WorldTile worldTile in randomTile.neighboursAll)
    {
      if (!randomEffect.tile_types.Contains(worldTile.Type.id))
        return;
    }
    TileEffect tileEffect = EffectsLibrary.spawn("fx_tile_effect", randomTile) as TileEffect;
    if (Object.op_Equality((Object) tileEffect, (Object) null))
      return;
    tileEffect.load(randomEffect);
  }

  public static void checkTileForEffectKill(WorldTile pTile, int pRadius)
  {
    BaseEffectController controllerTileEffects = World.world.stack_effects.controller_tile_effects;
    List<BaseEffect> list = controllerTileEffects.getList();
    for (int index = 0; index < list.Count; ++index)
    {
      BaseEffect pObject = list[index];
      if (pObject.active)
      {
        double x1 = (double) ((Component) pObject).transform.position.x;
        double y1 = (double) ((Component) pObject).transform.position.y;
        Vector2Int pos = pTile.pos;
        double x2 = (double) ((Vector2Int) ref pos).x;
        pos = pTile.pos;
        double y2 = (double) ((Vector2Int) ref pos).y;
        if ((double) Toolbox.Dist((float) x1, (float) y1, (float) x2, (float) y2) <= (double) pRadius)
        {
          controllerTileEffects.killObject(pObject);
          break;
        }
      }
    }
  }
}
