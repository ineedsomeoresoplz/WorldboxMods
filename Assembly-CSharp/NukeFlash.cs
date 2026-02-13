// Decompiled with JetBrains decompiler
// Type: NukeFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NukeFlash : BaseEffect
{
  private bool _killing;
  private bool _bomb_spawned;
  private TerraformOptions _terraform_asset;

  internal void spawnFlash(WorldTile pTile, string pBomb)
  {
    this.tile = pTile;
    this._bomb_spawned = false;
    this._terraform_asset = AssetManager.terraform.get(pBomb);
    this._killing = false;
    this.prepare(pTile, 0.1f);
  }

  private void Update()
  {
    Vector3 localScale = this.m_transform.localScale;
    if (!this._killing && (double) localScale.x < 1.0)
    {
      Config.grey_goo_damaged = false;
      localScale.x += World.world.elapsed * 2.5f;
      if ((double) localScale.x >= 0.800000011920929 && !this._bomb_spawned)
      {
        this._bomb_spawned = true;
        WorldAction bombAction = this._terraform_asset.bomb_action;
        if (bombAction != null)
        {
          int num = bombAction(pTile: this.tile) ? 1 : 0;
        }
      }
      if (Config.grey_goo_damaged && !AchievementLibrary.isUnlocked(AchievementLibrary.final_resolution))
        AchievementLibrary.final_resolution.check();
      if ((double) localScale.x >= 1.0)
      {
        localScale.x = 1f;
        this._killing = true;
      }
      localScale.y = localScale.x;
      this.m_transform.localScale = localScale;
    }
    else
    {
      if (!this._killing)
        return;
      localScale.x -= World.world.elapsed * 2.5f;
      localScale.y = localScale.x;
      if ((double) localScale.x <= 0.0)
      {
        localScale.x = 0.0f;
        this.kill();
      }
      this.m_transform.localScale = localScale;
    }
  }

  internal static bool atomic_bomb_action(BaseSimObject pTarget = null, WorldTile pTile = null)
  {
    EffectsLibrary.spawnAtTileRandomScale("fx_explosion_nuke_atomic", pTile, 0.8f, 0.9f);
    if (World.world.explosion_checker.checkNearby(pTile, 30))
      return false;
    MapAction.damageWorld(pTile, 30, TerraformLibrary.atomic_bomb);
    return true;
  }

  internal static bool crabzilla_bomb_action(BaseSimObject pTarget = null, WorldTile pTile = null)
  {
    EffectsLibrary.spawnAtTileRandomScale("fx_explosion_huge", pTile, 0.8f, 0.9f);
    if (World.world.explosion_checker.checkNearby(pTile, 30))
      return false;
    MapAction.damageWorld(pTile, 30, TerraformLibrary.crabzilla_bomb);
    int num = Randy.randomInt(1, 5);
    for (int index = 0; index < num; ++index)
    {
      Actor newUnit = World.world.units.createNewUnit("crab", pTile, pAdultAge: true);
      newUnit.addTrait("fire_blood");
      newUnit.addTrait("fire_proof");
      newUnit.addTrait("evil");
      newUnit.addTrait("tough");
    }
    return true;
  }

  internal static bool czar_bomba_action(BaseSimObject pTarget = null, WorldTile pTile = null)
  {
    EffectsLibrary.spawnAtTileRandomScale("fx_explosion_huge", pTile, 1.4f, 1.6f);
    if (World.world.explosion_checker.checkNearby(pTile, 70))
      return false;
    MapAction.damageWorld(pTile, 70, TerraformLibrary.czar_bomba);
    return true;
  }
}
